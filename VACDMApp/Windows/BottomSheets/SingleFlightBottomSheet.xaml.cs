using The49.Maui.BottomSheet;
using VACDMApp.VACDMData.Renderer;
using static VACDMApp.VACDMData.Data;

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
        SenderPage = VACDMData.SenderPage.SingleFlight;
        Sender = this;
        SingleFlightGrid.Children.Add(content);
    }

    private void BottomSheet_Unfocused(object sender, FocusEventArgs e)
    {
        DismissAsync();
    }
}