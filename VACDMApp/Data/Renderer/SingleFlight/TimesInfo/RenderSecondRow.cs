using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Grid FlightInfoSecondRowGrid(Vacdm vacdm)
        {
            var timesSecondRowGrid = new Grid();

            timesSecondRowGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            timesSecondRowGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            timesSecondRowGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            timesSecondRowGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );

            var tobtTextLabel = new Label()
            {
                Text = $"TOBT:",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Center
            };
            var tobtTimeLabel = new Label()
            {
                Text = $"{vacdm.Tobt:HH:mmZ}",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = _timeMargin
            };
            var tsatTextLabel = new Label()
            {
                Text = $"TSAT:",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Center
            };
            var tsatBackgroundColor = GetTsatBackgroundColor(vacdm.Tsat, vacdm.Tobt);
            var tsatTimeLabel = new Label()
            {
                Text = $"{vacdm.Tsat:HH:mmZ}",
                BackgroundColor = tsatBackgroundColor,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.None,
                FontSize = 20,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalTextAlignment = TextAlignment.End,
                Margin = _timeMargin
            };

            timesSecondRowGrid.Children.Add(tobtTextLabel);
            timesSecondRowGrid.Children.Add(tobtTimeLabel);
            timesSecondRowGrid.Children.Add(tsatTextLabel);
            timesSecondRowGrid.Children.Add(tsatTimeLabel);

            timesSecondRowGrid.SetColumn(tobtTextLabel, 0);
            timesSecondRowGrid.SetColumn(tobtTimeLabel, 1);
            timesSecondRowGrid.SetColumn(tsatTextLabel, 2);
            timesSecondRowGrid.SetColumn(tsatTimeLabel, 3);

            return timesSecondRowGrid;
        }
    }
}
