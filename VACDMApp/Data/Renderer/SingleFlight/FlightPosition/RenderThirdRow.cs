using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Grid FlightPositionThirdRowGrid(VACDMPilot pilot)
        {
            var flightPositionThirdGrid = new Grid();
            flightPositionThirdGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            flightPositionThirdGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(2, GridUnitType.Star))
            );
            flightPositionThirdGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            flightPositionThirdGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(2, GridUnitType.Star))
            );

            var sidTextLabel = new Label()
            {
                Text = $"SID:",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                VerticalTextAlignment = TextAlignment.Center
            };
            var sidDataLabel = new Label()
            {
                Text = pilot.Clearance.Sid,
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = _timeMargin
            };
            var squawkTextLabel = new Label()
            {
                Text = $"Squawk:",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                VerticalTextAlignment = TextAlignment.Center
            };
            var squawkDataLabel = new Label()
            {
                Text = pilot.Clearance.AssignedSquawk,
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = _timeMargin
            };

            flightPositionThirdGrid.Children.Add(sidTextLabel);
            flightPositionThirdGrid.Children.Add(sidDataLabel);
            flightPositionThirdGrid.Children.Add(squawkTextLabel);
            flightPositionThirdGrid.Children.Add(squawkDataLabel);

            flightPositionThirdGrid.SetColumn(sidTextLabel, 0);
            flightPositionThirdGrid.SetColumn(sidDataLabel, 1);
            flightPositionThirdGrid.SetColumn(squawkTextLabel, 2);
            flightPositionThirdGrid.SetColumn(squawkDataLabel, 3);

            return flightPositionThirdGrid;
        }
    }
}
