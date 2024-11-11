using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using System.Text.Json.Serialization;
using XRFID.Demo.Common.Enumerations;
using XRFID.Demo.Modules.Mqtt.Contracts;
using XRFID.Demo.Modules.Mqtt.Payloads;
using XRFID.Server.Demo.V2.DataStructures.SignalR;
using static XRFID.Server.Demo.V2.DataStructures.Misc.PlotDataObject;

namespace XRFID.Server.Demo.V2.Components.Locationing;

public partial class Locationing : ComponentBase, IDisposable
{
    private const int DELETE_TIMER = 3000;
    private const int SETUP_TIMER = 2000;

    private HubConnection? connection;

    public void Navigate(PlotDataObj p) => _navigation.NavigateTo($"/Locationing/{selectedReader.Name}/{p.Epc}");

    public async Task InitializeConnectionAsync()
    {
        try
        {
            connection = new HubConnectionBuilder()
           .WithUrl(_navigationManager.ToAbsoluteUri("/uimessagehub"))
           .Build();

            await connection.StartAsync();
            await connection.SendAsync("AddToGroup", selectedReader.Name);

            connection.On<PointMessage>("ReceiveMessage", (message) =>
            {
                OnMessageReceived(message);
            });

            connection.On("RefreshState", async () =>
            {
                SetState();

                await InvokeAsync(StateHasChanged);
            });

            SetState();
        }
        catch (Exception e)
        {

            _logger.LogError(e, "");
        }
    }

    private void SetState()
    {
        var state = _worker.GetSMStatus();

        switch (state)
        {
            case StateMachineStateEnum.Ready:
                Reading = true;
                break;
            case StateMachineStateEnum.Reading:
                Reading = true;
                break;
            default:
                Reading = false;
                break;
        }
    }

    public async Task CheckReadersAsync()
    {
        var readers = await _readerRepo.GetAsync();

        foreach (var reader in readers)
        {

            readerList.Add(reader);
        }

    }

    public async Task FetchDataAsync()
    {

        var tableData = await _tagRepo.AsQueryable()
            .AsNoTracking()
            .Where(t => t.ReaderName == selectedReader.Name && DateTimeOffset.Compare(t.Timestamp, DateTimeOffset.Now.AddMinutes(-10)) > 0)
            .GroupBy(t => t.Epc)
            .Select(g => new { Epc = g.Key, Timestamp = g.Max(t => t.Timestamp) })
            .OrderByDescending(p => p.Timestamp).ToArrayAsync();

        foreach (var data in tableData)
        {
            tablePoints.Add(new PlotDataObj { Epc = data.Epc, Timestamp = data.Timestamp });
        }
    }

    private void OnMessageReceived(PointMessage value)
    {
        lock (dicts)
        {
            bool found = false;
            List<PointMessage>? positionData;
            PlotDataObj plotDataObj = new PlotDataObj { Epc = value.EPC, Timestamp = value.Timestamp };


            foreach (PlotDataObj point in tablePoints)
            {
                if (plotDataObj.Equals(point))
                {
                    point.Timestamp = value.Timestamp;
                    found = true;

                    break;
                }
            }

            if (!found)
            {
                _logger.LogTrace("add {abc}", value.EPC);
                tablePoints.Add(new PlotDataObj { Epc = value.EPC, Timestamp = value.Timestamp });
            }

            if (dicts.TryGetValue(plotDataObj, out positionData))
            {
                if (!positionData.Contains(value))
                {

                    positionData.Add(value);

                }
            }
            else
            {


                positionData = [];

                if (!positionData.Contains(value))
                {


                    positionData.Add(value);
                    dicts.Add(plotDataObj, positionData);

                }
            }
            Task t = Task.Run(() => DeletePoint(DELETE_TIMER, value));

            InvokeAsync(StateHasChanged);
        }
    }

    public async void DbSave(bool v)
    {
        _pointDataStores.dbSave = v;
    }

    public async void SendCommand(bool v)
    {
        Reading = v;

        await _commandPublisher.Publish(new RAWMQTTCommands
        {
            ReaderId = selectedReader.Id,
            Topic = "mcmds",
            HostName = selectedReader.Name,
            Command = "set_mode",
            CommandId = Guid.NewGuid().ToString(),
            Payload = new SetModePayload
            {
                Filter = new Filter()
                {
                    Match = "prefix",
                    Operation = "include",
                    Value = _pointDataStores.EpcFilter,
                },
                ModeSpecificSettings = new OperatingModeSpecificSettingsPayload()
                {
                    AdvancedConfig = new DirectionalityAdvancedConfigPayload()
                    {
                        ReportRaw = v,
                        tagTimeoutSecondsMin = 5,
                        tagTimeoutSecondsDefault = 5,
                        tagTimeoutSecondsMax = 10

                    }
                },
                Type = "DIRECTIONALITY"
            }
        });


        string state = v ? "low" : "high";
        int pin1;
        int pin2;

        if (v)
        {
            pin1 = 1;
            pin2 = 2;
        }
        else
        {
            pin1 = 2;
            pin2 = 1;
        }


        new Thread(async () =>
        {
            await _zebraEventPublisher.Publish(new ZebraGpiData
            {
                HostName = selectedReader.Name,
                Pin = pin1,
                State = state,
                Timestamp = DateTime.Now,
            });


            Thread.Sleep(SETUP_TIMER);

            await _zebraEventPublisher.Publish(new ZebraGpiData
            {
                HostName = selectedReader.Name,
                Pin = pin2,
                State = state,
                Timestamp = DateTime.Now,
            });
        }).Start();

    }

    public async Task DeletePoint(int timer, PointMessage point)
    {
        await Task.Delay(timer);
        lock (dicts)
        {
            PlotDataObj plotDataObj = new PlotDataObj { Epc = point.EPC };
            dicts[plotDataObj].Remove(point);
            if (dicts[plotDataObj].Count == 0)
            {
                dicts.Remove(plotDataObj);

            }
            InvokeAsync(StateHasChanged);
        }
    }

    public void Dispose()
    {
        _worker.Reset();
        connection?.StopAsync();
        sfChart.Dispose();
    }

    public class SetModePayload
    {
        /// <summary>
        /// ModeSpecificSettings
        /// </summary>
        [JsonPropertyName("filter")]
        public Filter? Filter { get; set; }

        [JsonPropertyName("modeSpecificSettings")]
        public OperatingModeSpecificSettingsPayload? ModeSpecificSettings { get; set; }

        [JsonPropertyName("type")]
        public required string Type { get; set; }

    }

    public class Filter
    {
        [JsonPropertyName("match")]
        public string Match { get; set; }
        [JsonPropertyName("operation")]
        public string Operation { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }

    public class OperatingModeSpecificSettingsPayload
    {

        /// <summary>
        /// Gets or Sets AdvancedConfig
        /// </summary>
        [JsonPropertyName("advancedConfig")]
        public DirectionalityAdvancedConfigPayload? AdvancedConfig { get; set; }

    }

    public partial class DirectionalityAdvancedConfigPayload
    {
        /// <summary>
        /// Report (or do not report) raw bearing/location messages
        /// </summary>
        /// <value>Report (or do not report) raw bearing/location messages</value>

        [JsonPropertyName("report_raw")]
        public bool? ReportRaw { get; set; }
        [JsonPropertyName("tag_timeout_seconds_min")]
        public int tagTimeoutSecondsMin { get; set; }
        [JsonPropertyName("tag_timeout_seconds_max")]
        public int tagTimeoutSecondsMax { get; set; }
        [JsonPropertyName("tag_timeout_seconds_default")]
        public int tagTimeoutSecondsDefault { get; set; }
    }
}
