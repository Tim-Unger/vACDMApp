using The49.Maui.BottomSheet;
using VACDMApp.VACDMData.Renderer;

namespace VACDMApp.Windows.BottomSheets;

public partial class SingleFlightBottomSheet : BottomSheet
{
    public static string SelectedCallsign = "";

	public SingleFlightBottomSheet()
	{
		InitializeComponent();
    }

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        var content = SingleFlight.RenderGrid(SelectedCallsign);
        SingleFlightGrid.Children.Add(content);
    }
}