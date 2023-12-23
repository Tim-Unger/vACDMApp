namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static Border RenderTimeSeperator(int hour)
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

            var timeLabel = new Label()
            {
                //TODO Local Time as well
                Text = $"{hour}:00Z",
                Padding = new Thickness(10, 0, 0, 0),
                TextColor = Colors.White,
                Background = Colors.Black,
                FontAttributes = FontAttributes.None,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 17
            };

            grid.Children.Add(timeLabel);

            grid.SetColumn(timeLabel, 0);

            border.Content = grid;

            return border;
        }
    }
}
