namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static Border RenderTimeSeperator(int value, bool isDay)
        {
            var border = new Border()
            {
                Stroke = Colors.Transparent,
                BackgroundColor = Colors.Transparent
            };
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(10, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));

            var now = DateTime.UtcNow;

            var labelText = isDay ? new DateOnly(now.Year, now.Month, value).ToShortDateString() : $"{value}:00Z";

            var timeLabel = new Label()
            {
                Text = labelText,
                Padding = new Thickness(10, 0, 0, 0),
                TextColor = Colors.White,
                Background = Colors.Black,
                FontAttributes = FontAttributes.None,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 17
            };

            if (isDay)
            {
                grid.SetColumnSpan(timeLabel, 2);
                timeLabel.HorizontalOptions = LayoutOptions.Start;
            }

            grid.Children.Add(timeLabel);

            grid.SetColumn(timeLabel, 0);

            border.Content = grid;

            return border;
        }


    }
}
