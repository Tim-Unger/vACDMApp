using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Grid FlightInfoGrid(VACDMPilot pilot)
        {
            var flightInfoGrid = new Grid()
            {
                Padding = 10,
                Margin = 10,
                BackgroundColor = _darkBlue
            };

            flightInfoGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(5, GridUnitType.Star))
            );

            flightInfoGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(10, GridUnitType.Star))
            );

            var timeDateGrid = new Grid();
            timeDateGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            );
            timeDateGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(1, GridUnitType.Star))
            );

            var eobtLabel = new Label()
            {
                Text = pilot.Vacdm.Eobt.ToString("HH:mmZ"),
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                VerticalTextAlignment = TextAlignment.Center
            };
            var dateLabel = new Label()
            {
                Text = DateOnly.FromDateTime(DateTime.UtcNow).ToShortDateString(),
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                VerticalTextAlignment = TextAlignment.Center
            };

            timeDateGrid.Children.Add(eobtLabel);
            timeDateGrid.Children.Add(dateLabel);

            timeDateGrid.SetRow(eobtLabel, 0);
            timeDateGrid.SetRow(dateLabel, 1);

            flightInfoGrid.Children.Add(timeDateGrid);
            flightInfoGrid.SetColumn(timeDateGrid, 0);
            flightInfoGrid.SetColumn(timeDateGrid, 0);

            return flightInfoGrid;
        }
    }
}
