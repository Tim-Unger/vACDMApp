using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Grid FlightPositionFirstRowGrid(Vacdm vacdm)
        {
            var flightPositionFirstGrid = new Grid();
            flightPositionFirstGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(2, GridUnitType.Star))
            );
            flightPositionFirstGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(2, GridUnitType.Star))
            );

            var gateArea = vacdm.TaxiZone == "default taxitime" ? "N/A" : vacdm.TaxiZone;

            var gateTextLabel = new Label()
            {
                Text = $"Gate-Area:",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                VerticalTextAlignment = TextAlignment.Center
            };

            var gateDataLabel = new Label()
            {
                Text = gateArea,
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = _timeMargin
            };

            flightPositionFirstGrid.Children.Add(gateTextLabel);
            flightPositionFirstGrid.Children.Add(gateDataLabel);

            flightPositionFirstGrid.SetColumn(gateTextLabel, 0);
            flightPositionFirstGrid.SetColumn(gateDataLabel, 1);

            return flightPositionFirstGrid;
        }
    }
}
