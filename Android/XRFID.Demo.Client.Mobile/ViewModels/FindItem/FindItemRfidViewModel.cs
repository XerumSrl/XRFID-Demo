﻿using Com.Zebra.Rfid.Api3;
using XRFID.Demo.Client.Mobile.Base;
using XRFID.Demo.Client.Mobile.Data.Services.Interfaces;
using XRFID.Demo.Client.Mobile.Data.ViewData;
using XRFID.Demo.Client.Mobile.Platforms.Android.Interfaces;
using Microsoft.Maui.Storage;

namespace XRFID.Demo.Client.Mobile.ViewModels;

[QueryProperty(nameof(Product), nameof(Product))]
public partial class FindItemRfidViewModel : BaseRfidViewModel
{
    #region properties
    private const int HYSTERESIS = 25;
    private const int TIMER_DELAY = 2500;

    private short rssiLow = -80;
    private short rssiHigh = -30;
    private short rssiMagicNumber;//Calculate constant to sum to make rssi always positive   
    private float rssiScaleFactor;//calculte scale factor from normalized rssi values to Percentage values, this is basically the *100/maxvalue part of the proportion

    private const short PERC_LOW = 33;
    private const short PERC_HIGH = 66;

    private readonly SolidColorBrush PercLow = new SolidColorBrush(new Color(231, 29, 54)); //red //cannot be const due to c# limitations
    private readonly SolidColorBrush PercMid = new SolidColorBrush(new Color(253, 197, 0)); //yellow
    private readonly SolidColorBrush PercHigh = new SolidColorBrush(new Color(70, 200, 53)); //green?

    private Queue<double> percentQueue = new Queue<double>();

    private Timer resetTimer;

    private FindProductViewData product = new FindProductViewData();
    public FindProductViewData Product
    {
        get => product;
        set => SetProperty(ref product, value);
    }

    private Brush color;
    public Brush Color
    {
        get => color;
        set => SetProperty(ref color, value);
    }

    private double percent;
    public double Percent
    {
        get => percent;
        set => SetProperty(ref percent, value);
    }

    private bool isReading = false;
    #endregion

    public FindItemRfidViewModel(IRFIDService rfidService, IGeneralSettings generalSettings) : base(rfidService, generalSettings)
    {
        resetTimer = new Timer(ResetPercentage, new AutoResetEvent(false), Timeout.Infinite, Timeout.Infinite);
        Color = PercLow;

        rssiMagicNumber = (short)(rssiLow * -1);
        rssiScaleFactor = 100f / (float)(rssiHigh + rssiMagicNumber);


    }

    #region view methods
    public override Task OnAppearing()
    {
        Percent = 0;
        isReading = false;
        rssiHigh = (short)Preferences.Default.Get<int>("findsensitivity", -30);
        rssiMagicNumber = (short)(rssiLow * -1);
        rssiScaleFactor = 100f / (float)(rssiHigh + rssiMagicNumber);

        percentQueue.Clear();
        for (int i = 0; i < HYSTERESIS; i++)
        {
            percentQueue.Enqueue(0);
        }

        short[] antennas = rfidService.GetAntennas();

        foreach (short antenna in antennas)
        {
            PreFilters preFilters = new PreFilters();
            PreFilters.PreFilter preFilter = new PreFilters.PreFilter(preFilters);
            preFilter.AntennaID = antenna;
            preFilter.SetTagPattern(product.Epc);
            preFilter.TagPatternBitCount = product.Epc.Length * 4;
            preFilter.BitOffset = 32; // skip PC bits
            preFilter.MemoryBank = MEMORY_BANK.MemoryBankEpc;
            preFilter.FilterAction = FILTER_ACTION.FilterActionStateAware;
            preFilter.StateAwareAction.Target = TARGET.TargetInventoriedStateS0;
            preFilter.StateAwareAction.StateAwareAction = STATE_AWARE_ACTION.StateAwareActionInvA;
            rfidService.AddFilter(preFilter);
            Antennas.SingulationControl sc = rfidService.GetSingulation(antenna);
            sc.Session = SESSION.SessionS0;
            sc.Action.InventoryState = INVENTORY_STATE.InventoryStateA;
            sc.Action.SetPerformStateAwareSingulationAction(true);
            rfidService.SetSingulation(antenna, sc);
        }

        UpdateIn();
        return base.OnAppearing();
    }
    public override Task OnDisappearing()
    {
        rfidService.RemoveFilter();
        //rfidService.SetFilters(preFilters);
        Dispose(); //This dispose method is in base, but due to inherithance implementation details it calls the overridden UpdateOut()
        return base.OnDisappearing();
    }
    #endregion

    #region WorkFlow Methods
    public void ContinousRead(bool pressed)
    {
        if (pressed)
        {
            PerformInventory();
        }
        else
        {
            StopInventory();
        }
    }

    private void ResetPercentage(object e)
    {
        Percent = 0;

        percentQueue.Clear();
        for (int i = 0; i < HYSTERESIS; i++)
        {
            percentQueue.Enqueue(0);
        }
    }

    private void PercentageCalculation(short rssi)
    {
        if (rssi > rssiHigh)
        {
            rssi = rssiHigh;
        }
        else if (rssi < rssiLow)
        {
            rssi = rssiLow;
        }

        // rssi to positive
        rssi += rssiMagicNumber;

        //Calculate scaled percentage with " oneHundredPerc:100 = rssi:X "
        var percent = rssi * rssiScaleFactor;

        percentQueue.Enqueue(percent);

        while (percentQueue.Count > HYSTERESIS)
        {
            percentQueue.Dequeue();
        }

        double sum = 0d;
        foreach (var item in percentQueue)
        {
            sum += item;
        }

        Percent = Math.Round(sum / HYSTERESIS);

        switch (Percent)
        {
            case <= PERC_LOW:
                Color = PercLow;
                break;
            case > PERC_HIGH:
                Color = PercHigh;
                break;
            default:
                Color = PercMid;
                break;
        }

    }
    #endregion

    #region RFID method
    public override void UpdateIn()
    {
        rfidService.TagRead += TagReadEvent;
        rfidService.TriggerEvent += HHTriggerEvent;
        rfidService.ReaderConnectionEvent += ReaderConnectionEvent;

        disposedValue = false;
    }
    public override void UpdateOut()
    {
        rfidService.TagRead -= TagReadEvent;
        rfidService.TriggerEvent -= HHTriggerEvent;
        rfidService.ReaderConnectionEvent -= ReaderConnectionEvent;
    }

    protected override void HHTriggerEvent(bool pressed)
    {
        if (pressed)
        {
            isReading = true;
            Task.Run(ContinousInventory);
        }
        else
        {
            isReading = false;
            StopInventory();
        }
    }

    protected override void UpdateUi(string id, int count, short? rssi)
    {
        if (Product.Epc.Equals(id))
        {
            PercentageCalculation(rssi ?? rssiLow);
            resetTimer.Change(TIMER_DELAY, Timeout.Infinite);
        }
    }

    private void ContinousInventory()
    {
        do
        {
            PerformInventory();
            Thread.Sleep(100);
            StopInventory();

        } while (isReading);
    }
    #endregion
}