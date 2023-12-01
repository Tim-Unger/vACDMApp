using The49.Maui.BottomSheet;
using VACDMApp.VACDMData;
using static VACDMApp.VACDMData.Data;
namespace VACDMApp.Windows.BottomSheets;

public partial class AirportsBottomSheet : BottomSheet
{
	public AirportsBottomSheet()
	{
		InitializeComponent();
        GetAirports();
	}

	internal static string SelectedAirport = "";

	private void GetAirports()
	{
        var airports = VACDMPilots
            .Select(x => x.FlightPlan.Departure)
            .DistinctBy(x => x.ToUpper())
            .ToList();

        airports.Add("ALL AIRPORTS");

        if (airports.Count() == 0)
        {
            var noAirports = new Button() { Text = "No Airports found", TextColor = Colors.Black };
            AirportsStackLayout.Children.Add(noAirports);
        }
        airports.ForEach(x => AirportsStackLayout.Children.Add(RenderAirport(x)));
    }

	private Button RenderAirport(string icao)
	{
		var airport = new Button() { Text = icao.ToUpperInvariant(), TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, Padding = 20, Margin = 10 };
        airport.Clicked += Airport_Clicked;

		return airport;
	}

    private void Airport_Clicked(object sender, EventArgs e)
    {
		var selectedAirport = sender as Button;

		SelectedAirport = selectedAirport.Text.ToUpper();

		DismissAsync();
        FlightsView.SetAirportText(selectedAirport.Text);
        FlightsView.GetFlightsFromSelectedAirport();
    }

	internal static string GetClickedAirport() => SelectedAirport;
}