using CommunityToolkit.Maui.ImageSources;
using Microsoft.Maui.Controls.Shapes;
using static Android.Print.PrintAttributes;

namespace VacdmApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        internal static readonly Color _darkBlue = new(28, 40, 54);

        private static readonly Color _darkGrey = new(23, 23, 23);

        private static readonly Thickness _timeMargin = new(0, 10, 10, 10);

        internal static Grid RenderGrid(string callsign)
        {
            var flightPlan =
                Data.VatsimPilots.FirstOrDefault(x => x.callsign == callsign)?.flight_plan
                ?? throw new Exception();
            var airlines = Data.Airlines;

            var pilot = Data.VacdmPilots.First(x => x.Callsign == callsign);

            var grid = new Grid { BackgroundColor = _darkGrey };

            grid.RowDefinitions.Add(new RowDefinition(new GridLength(0.5, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(2, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star))); //Flight Info
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star))); //Times Info
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star))); 
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star))); 
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(2, GridUnitType.Star))); //Flight Position
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(4, GridUnitType.Star))); //Placeholder bottom

            var placeholderRectangle = new Rectangle()
            {
                BackgroundColor = _darkGrey,
                HeightRequest = 200
            };

            grid.Children.Add(placeholderRectangle);
            grid.SetRow(placeholderRectangle, 9);

            var airportData = Data.Airports.FirstOrDefault(
                x => x.Icao == pilot.FlightPlan.Departure
            );

            var airportText =
                $"From {airportData?.Icao ?? pilot.FlightPlan.Departure} ({airportData?.Iata ?? ""})";

            var airportLabel = new Label()
            {
                Text = airportText,
                TextColor = Colors.White,
                FontSize = 20,
                Margin = new Thickness(10, 0, 0, 0)
            };

            var timesLabel = new Label()
            {
                Text = "Times",
                TextColor = Colors.White,
                FontSize = 20,
                Margin = new Thickness(10, 0, 0, 0),
                VerticalOptions = LayoutOptions.Center
            };

            var moreTimesButton = new Button()
            {
                Text = "More Times",
                TextColor = _darkBlue,
                FontSize = 12,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(0, 0, 10, 0),
            };

            moreTimesButton.Clicked += MoreTimesButton_Clicked;

            var resourcesLabel = new Label()
            {
                Text = "Resources",
                TextColor = Colors.White,
                FontSize = 20,
                Margin = new Thickness(10, 0, 0, 0)
            };

            grid.Children.Add(airportLabel);
            grid.SetRow(airportLabel, 1);

            var topBarGrid = TopBarGrid(pilot);

            var flightInfoGrid = FlightInfoGrid(pilot);

            var flightDetailsGrid = FlightDetailsGrid(pilot, airlines, flightPlan);

            flightInfoGrid.Children.Add(flightDetailsGrid);
            flightInfoGrid.SetColumn(flightDetailsGrid, 1);

            var timesInfoGrid = TimesInfoGrid(pilot);

            var flightPositionGrid = FlightPositionGrid(pilot, pilot.Vacdm);

            grid.Children.Add(topBarGrid);
            grid.SetRow(topBarGrid, 0);

            grid.Children.Add(flightInfoGrid);
            grid.SetRow(flightInfoGrid, 2);

            grid.Children.Add(timesLabel);
            grid.SetRow(timesLabel, 4);
            grid.Children.Add(moreTimesButton);
            grid.SetRow(moreTimesButton, 4);
            grid.Children.Add(timesInfoGrid);
            grid.SetRow(timesInfoGrid, 5);

            grid.Children.Add(resourcesLabel);
            grid.SetRow(resourcesLabel, 6);
            grid.Children.Add(flightPositionGrid);
            grid.SetRow(flightPositionGrid, 7);

            return grid;
        }

        //TODO implement button click
        private static void MoreTimesButton_Clicked(object? sender, EventArgs e)
        {

        }
    }
}
