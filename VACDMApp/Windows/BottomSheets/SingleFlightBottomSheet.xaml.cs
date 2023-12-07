using Plugin.LocalNotification;
using The49.Maui.BottomSheet;
using VACDMApp.Data.PushNotifications;
using VACDMApp.Data.Renderer;
using static VACDMApp.VACDMData.Data;

namespace VACDMApp.Windows.BottomSheets;

public partial class SingleFlightBottomSheet : BottomSheet
{
    public static string SelectedCallsign = "";

	public SingleFlightBottomSheet()
	{
		InitializeComponent();
    }

    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        await UpdateDataContinuously();

        var content = SingleFlight.RenderGrid(SelectedCallsign);
        SenderPage = VACDMData.SenderPage.SingleFlight;
        Sender = this;
        SingleFlightGrid.Children.Add(content);

        //TODO remove
        var pilot = VACDMPilots.First(x => x.Callsign == SelectedCallsign);
        await PushNotificationHandler.SendPushNotificationAsync(pilot, PushNotificationHandler.NotificationType.SlotNow);
    }

    private void BottomSheet_Unfocused(object sender, FocusEventArgs e)
    {
        DismissAsync();
    }

    private async Task UpdateDataContinuously()
    {

        //TODO Pause on lost focus/Cancellation Token
        while (true)
        {
            await MainPage.GetAllData();

            SingleFlightGrid.Children.Clear();

            var content = SingleFlight.RenderGrid(SelectedCallsign);
            SenderPage = VACDMData.SenderPage.SingleFlight;
            Sender = this;
            SingleFlightGrid.Children.Add(content);

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }
}