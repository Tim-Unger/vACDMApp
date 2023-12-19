using Microsoft.Maui.Controls.Shapes;
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

    private static readonly double _width = Shell.Current.CurrentPage.Width;

    private void GetAirports()
    {
        var airports = VACDMPilots
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
            FontAttributes = FontAttributes.Bold,
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
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center
        };

        grid.Children.Add(airport);
        grid.Children.Add(text);

        airport.Clicked += Airport_Clicked;

        return grid;
    }

    private void Airport_Clicked(object sender, EventArgs e)
    {
        var selectedAirport = (Button)sender;
        var selectedParent = selectedAirport.Parent;
        var childLabel = ((Grid)selectedParent).Children[1];

        var childText = ((Label)childLabel).Text;

        SelectedAirport = childText.ToUpper();

        DismissAsync();
        FlightsView.SetAirportText(SelectedAirport);
        FlightsView.GetFlightsFromSelectedAirport();
    }

    internal static string GetClickedAirport() => SelectedAirport;

    private void BottomSheet_Focused(object sender, FocusEventArgs e)
    {
        Sender = this;

        VACDMData.Data.SenderPage = VACDMData.SenderPage.About;
    }

    private void BottomSheet_Unfocused(object sender, FocusEventArgs e)
    {

    }
}
