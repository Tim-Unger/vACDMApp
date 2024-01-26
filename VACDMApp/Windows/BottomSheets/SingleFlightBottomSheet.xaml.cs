using The49.Maui.BottomSheet;
using VACDMApp.Data.Renderer;
using static VACDMApp.VACDMData.Data;

namespace VACDMApp.Windows.BottomSheets;

public partial class SingleFlightBottomSheet : BottomSheet
{
    public static string SelectedCallsign = "";

    private bool _isFirstLoad = true;

    public SingleFlightBottomSheet()
    {
        InitializeComponent();
        LoadContent();

        SenderPage = VACDMData.SenderPage.SingleFlight;
        Sender = this;
    }

    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        await FlightsView.HideKeyboardAsync();
        await UpdateDataContinuously();
    }

    private async Task UpdateDataContinuously()
    {
        //TODO Pause on lost focus/Cancellation Token
        while (true)
        {
            if (_isFirstLoad)
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
                _isFirstLoad = false;
            }

            await MainPage.GetAllData();

            SingleFlightGrid.Children.Clear();

            var content = SingleFlight.RenderGrid(SelectedCallsign);

            SenderPage = VACDMData.SenderPage.SingleFlight;
            Sender = this;

            SingleFlightGrid.Children.Add(content);

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }

    private void LoadContent()
    {
        var content = SingleFlight.RenderGrid(SelectedCallsign);
        
        SingleFlightGrid.Children.Add(content);
    }
}
