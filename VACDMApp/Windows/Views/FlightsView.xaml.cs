using VACDMApp.VACDMData.Renderer;

namespace VACDMApp.Windows.Views;

public partial class FlightsView : ContentView
{
	public FlightsView()
	{
		InitializeComponent();
	}

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        var flights = FlightInfos.Render();
        flights.ForEach(FlightsStackLayout.Children.Add);
    }
}