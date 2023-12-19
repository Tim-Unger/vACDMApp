using Microsoft.Maui.Controls.Shapes;
using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial  class SingleFlight
    {
        internal static readonly Color _darkBlue = new(28, 40, 54);

        private static readonly Color _darkGrey = new(23, 23, 23);

        private static readonly Thickness _timeMargin = new(0, 10, 10, 10);

        internal static Grid RenderGrid(string callsign)
        {
            var flightPlan =
                VACDMData.Data.VatsimPilots.FirstOrDefault(x => x.callsign == callsign).flight_plan
                ?? throw new Exception();
            var airlines = VACDMData.Data.Airlines;

            var pilot = VACDMData.Data.VACDMPilots.First(x => x.Callsign == callsign);

            var grid = new Grid { BackgroundColor = _darkGrey };

            grid.RowDefinitions.Add(new RowDefinition(new GridLength(10, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(20, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(10, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(20, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(10, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(20, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(50, GridUnitType.Star))); //Placeholder bottom

            var placeholderRectangle = new Rectangle()
            {
                BackgroundColor = _darkGrey,
                HeightRequest = 300
            };

            grid.Children.Add(placeholderRectangle);
            grid.SetRow(placeholderRectangle, 6);

            var airport = new Label()
            {
                Text = $"From: {pilot.FlightPlan.Departure}",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15
            };
            grid.Children.Add(airport);
            grid.SetRow(airport, 0);

            var flightInfoGrid = FlightInfoGrid(pilot);

            var flightDetailsGrid = FlightDetailsGrid(pilot, airlines, flightPlan);

            flightInfoGrid.Children.Add(flightDetailsGrid);
            flightInfoGrid.SetColumn(flightDetailsGrid, 1);

            var timesInfoGrid = TimesInfoGrid(pilot);

            var flightPositionGrid = FlightPositionGrid(pilot, pilot.Vacdm);

            var placeholderRectangleOne = new Rectangle()
            {
                Background = _darkGrey,
                HeightRequest = 25
            };
            var placeholderRectangleTwo = new Rectangle()
            {
                Background = _darkGrey,
                HeightRequest = 25
            };
            var placeholderRectangleThree = new Rectangle()
            {
                Background = _darkGrey,
                HeightRequest = 25
            };

            grid.Children.Add(placeholderRectangleOne);
            grid.Children.Add(placeholderRectangleTwo);
            grid.Children.Add(placeholderRectangleThree);

            grid.SetRow(placeholderRectangleOne, 0);
            grid.SetRow(placeholderRectangleTwo, 2);
            grid.SetRow(placeholderRectangleThree, 4);

            grid.Children.Add(flightInfoGrid);
            grid.SetRow(flightInfoGrid, 1);

            grid.Children.Add(timesInfoGrid);
            grid.SetRow(timesInfoGrid, 3);

            grid.Children.Add(flightPositionGrid);
            grid.SetRow(flightPositionGrid, 5);

            return grid;
        }
    }
}
