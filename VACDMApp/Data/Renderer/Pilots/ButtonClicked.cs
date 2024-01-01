using VACDMApp.Windows.BottomSheets;

namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var parentGrid = (Grid)button.Parent;
            var callsignGrid = (Grid)((Grid)parentGrid.Children[1]).Children[1];
            var callsignLabel = (Label)callsignGrid.Children[1];
            var callsign = callsignLabel.Text;

            SingleFlightBottomSheet.SelectedCallsign = callsign;

            var singleFlightSheet = new SingleFlightBottomSheet();

            //var currentWindow = Shell.Current.Window;
            singleFlightSheet.ShowAsync();
        }
    }
}
