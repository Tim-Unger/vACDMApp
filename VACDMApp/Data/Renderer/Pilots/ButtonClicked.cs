using VACDMApp.Windows.BottomSheets;

namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var parentGrid = (Grid)button.Parent;
            var callsignGrid = (Grid)parentGrid.Children[1];
            var callsignLabel = (Label)callsignGrid.Children[1];
            var callsign = callsignLabel.Text;

            var singleFlightSheet = new SingleFlightBottomSheet();

            SingleFlightBottomSheet.SelectedCallsign = callsign;

            //var currentWindow = Shell.Current.Window;
            singleFlightSheet.ShowAsync();
        }
    }
}
