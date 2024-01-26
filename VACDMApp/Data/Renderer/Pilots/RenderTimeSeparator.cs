namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static Border RenderTimeSeperator(int value)
        {
            var border = new Border()
            {
                Stroke = Colors.Transparent,
                BackgroundColor = Colors.Transparent
            };

            var grid = new Grid() { Margin = new Thickness(0, 20, 0, 20) };

            var now = DateTime.UtcNow;

            var timeLabel = new Label()
            {
                Text = $"{value}:00Z",
                Margin = new Thickness(5,0,0,0),
                Padding = new Thickness(10, 0, 0, 0),
                TextColor = Colors.White,
                Background = Colors.Black,
                FontAttributes = FontAttributes.None,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                FontSize = 17
            };

            var dateLabel = new Label()
            {
                Text = now.ToShortDateString(),
                Margin = new Thickness(0, 0, 5, 0),
                Padding = new Thickness(0, 0, 10, 0),
                TextColor = Colors.White,
                Background = Colors.Black,
                FontAttributes = FontAttributes.None,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                FontSize = 17
            };

            grid.Children.Add(timeLabel);
            grid.Children.Add(dateLabel);

            border.Content = grid;

            return border;
        }


    }
}
