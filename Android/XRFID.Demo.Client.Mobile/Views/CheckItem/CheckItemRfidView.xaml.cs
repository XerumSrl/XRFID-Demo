using Android.OS;
using XRFID.Demo.Client.Mobile.ViewModels;

namespace XRFID.Demo.Client.Mobile.Views.CheckItem;

public partial class CheckItemRfidView
{

    private readonly CheckItemRfidViewModel viewModel;

    public CheckItemRfidView(CheckItemRfidViewModel viewModel)
    {
        try
        {
            InitializeComponent();
            BindingContext = viewModel;
            this.viewModel = viewModel;
        }
        catch (Exception e)
        {
            App.Current.MainPage.DisplayAlert("Page Error", e.StackTrace, "OK");
            throw;
        }

    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }

    protected override bool OnBackButtonPressed()
    {
        Dispatcher.Dispatch(async () =>
        {
            bool leave = await DisplayAlert("Application Closing", "Are you sure you want to close the application?", "Yes", "No");

            if (leave)
            {
                Process.KillProcess(Process.MyPid());
            }
        });

        return true;
    }
}