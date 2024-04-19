using Microsoft.Maui.Controls.Shapes;
using The49.Maui.BottomSheet;
using static VacdmApp.Data.Data;

namespace VacdmApp.Windows.BottomSheets;

public partial class AirportsBottomSheet : BottomSheet
{
    public AirportsBottomSheet()
    {
        InitializeComponent();
        GetAirports();
    }

    internal static string SelectedAirport = "";

    private static readonly double _width = Shell.Current.CurrentPage.Width;

    private void GetAirports()
    {
        var pilotsWithFP = VacdmPilots
               //Only Pilots that have are in the Vatsim Datafeed
               .Where(x => VatsimPilots.Exists(y => y.callsign == x.Callsign))
               //Only Pilots that have filed a flight plan
               .Where(
                   x =>
                       VatsimPilots.First(y => y.callsign == x.Callsign).flight_plan
                       != null
               );

        var airports = pilotsWithFP
            .Select(x => x.FlightPlan.Departure)
            .DistinctBy(x => x.ToUpper())
            .ToList();

        var handleBar = new RoundRectangle()
        {
            CornerRadius = 10,
            Background = Colors.White,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 50,
            HeightRequest = 10
        };

        AirportsStackLayout.Children.Add(handleBar);

        var titleLabel = new Label()
        {
            Text = "Please choose an Airport",
            Padding = 20,
            TextColor = Colors.White,
            Background = Colors.Transparent,
            FontSize = 20,
            FontAttributes = FontAttributes.None,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        AirportsStackLayout.Children.Add(titleLabel);
        AirportsStackLayout.Children.Add(RenderAirport("ALL AIRPORTS"));

        if (airports.Count() == 0)
        {
            AirportsStackLayout.Children.Add(
                new Rectangle() { Background = Colors.Transparent, HeightRequest = 50 }
            );
            return;
        }

        airports.ForEach(x => AirportsStackLayout.Children.Add(RenderAirport(x)));
    }

    private Grid RenderAirport(string icao)
    {
        var grid = new Grid() { Padding = 20 };

        var airport = new Button() { BackgroundColor = Colors.Transparent, WidthRequest = _width };
        var text = new Label()
        {
            Text = icao.ToUpperInvariant(),
            TextColor = Colors.White,
            Background = Colors.Transparent,
            FontSize = 17,
            FontAttributes = FontAttributes.None,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center
        };

        grid.Children.Add(airport);
        grid.Children.Add(text);

        airport.Clicked += Airport_Clicked;

        return grid;
    }

    private async void Airport_Clicked(object sender, EventArgs e)
    {
        var selectedAirport = (Button)sender;
        var selectedParent = (Grid)selectedAirport.Parent;

        var loadingIndicator = new ActivityIndicator()
        {
            Color = Colors.White,
            IsRunning = true,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center,
            Background = Colors.Transparent,
            Scale = 0.75
        };

        selectedParent.Children.Add(loadingIndicator);

        var childLabel = selectedParent.Children[1];

        var childText = ((Label)childLabel).Text;

        SelectedAirport = childText.ToUpper();

        await DismissAsync();
        FlightsView.SetAirportText(SelectedAirport);
        FlightsView.GetFlightsFromSelectedAirport();
    }

    internal static string GetClickedAirport() => SelectedAirport;

    private void BottomSheet_Focused(object sender, FocusEventArgs e)
    {
        Sender = this;

        Data.Data.SenderPage = Data.SenderPage.Airport;
    }
}
