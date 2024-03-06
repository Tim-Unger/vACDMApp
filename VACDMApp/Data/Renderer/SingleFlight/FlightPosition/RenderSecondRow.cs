using VacdmApp.Data;

namespace VacdmApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Grid FlightPositionSecondRowGrid(VacdmPilot pilot, Vacdm vacdm)
        {
            var flightPositionSecondGrid = new Grid();
            flightPositionSecondGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            flightPositionSecondGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            flightPositionSecondGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            flightPositionSecondGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );

            var rwy = pilot.Clearance.DepRwy;

            if (pilot.FlightPlan.Departure == "EDDF" && pilot.Clearance.DepRwy == "18")
            {
                rwy = "18W";
            }

            var rwyTextLabel = new Label()
            {
                Text = $"RWY:",
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                Background = _darkBlue,
                FontSize = 20,
                VerticalTextAlignment = TextAlignment.Center
            };

            var rwyDataLabel = new Label()
            {
                Text = rwy,
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = _timeMargin
            };

            var taxitimeTextLabel = new Label()
            {
                Text = $"Taxi-Time:",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                VerticalTextAlignment = TextAlignment.Center
            };
            var taxitimeDataLabel = new Label()
            {
                Text = $"{vacdm.Exot} min.",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = _timeMargin
            };

            flightPositionSecondGrid.Children.Add(rwyTextLabel);
            flightPositionSecondGrid.Children.Add(rwyDataLabel);
            flightPositionSecondGrid.Children.Add(taxitimeDataLabel);
            flightPositionSecondGrid.Children.Add(taxitimeTextLabel);

            flightPositionSecondGrid.SetColumn(rwyTextLabel, 0);
            flightPositionSecondGrid.SetColumn(rwyDataLabel, 1);
            flightPositionSecondGrid.SetColumn(taxitimeTextLabel, 2);
            flightPositionSecondGrid.SetColumn(taxitimeDataLabel, 3);

            return flightPositionSecondGrid;
        }
    }
}
