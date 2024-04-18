using The49.Maui.BottomSheet;
using VacdmApp.Data.Renderer;
using static VacdmApp.Data.Data;

namespace VacdmApp.Windows.BottomSheets;

public partial class SingleFlightBottomSheet : BottomSheet
{
    //TODO fix
    public static string SelectedCallsign = "";

    private bool _isFirstLoad = true;

    public SingleFlightBottomSheet()
    {
        InitializeComponent();
        LoadContent();

        SenderPage = Data.SenderPage.SingleFlight;
        Sender = this;
    }

    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        await FlightsView.HideKeyboardAsync();
        await UpdateDataContinuously();
    }

    private async Task UpdateDataContinuously()
    {
        while (Settings.UpdateAutomatically)
        {
            if (_isFirstLoad)
            {
                await Task.Delay(TimeSpan.FromMinutes(1));
                _isFirstLoad = false;
            }

            SingleFlightGrid.Children.Clear();

            var content = SingleFlight.RenderGrid(SelectedCallsign);

            SenderPage = Data.SenderPage.SingleFlight;
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
