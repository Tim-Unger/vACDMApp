namespace VacdmApp.Data.Renderer
{
    internal partial class Pilots
    {
        internal static List<(Border Border, DateTime DateTime)> TimeValues = new();

        private static Border RenderTimeSeperator(DateTime eobtTime)
        {
            var border = new Border()
            {
                Stroke = Colors.Transparent,
                BackgroundColor = Colors.Transparent
            };

            var grid = new Grid() { Margin = new Thickness(0, 20, 0, 20) };

            var timeLabel = new Label()
            {
                Text = $"{eobtTime.Hour}:00Z",
                Margin = new Thickness(5, 0, 0, 0),
                Padding = new Thickness(10, 0, 0, 0),
                TextColor = Colors.White,
                FontAttributes = FontAttributes.None,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                FontSize = 17
            };

            var dateLabel = new Label()
            {
                Text = eobtTime.ToShortDateString(),
                Margin = new Thickness(0, 0, 5, 0),
                Padding = new Thickness(0, 0, 10, 0),
                TextColor = Colors.White,
                FontAttributes = FontAttributes.None,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                FontSize = 17
            };

            grid.Children.Add(timeLabel);
            grid.Children.Add(dateLabel);

            border.Content = grid;

            TimeValues.Add(
                (
                    border,
                    new DateTime(
                        eobtTime.Year,
                        eobtTime.Month,
                        eobtTime.Day,
                        eobtTime.Hour,
                        0,
                        0,
                        DateTimeKind.Utc
                    )
                )
            );

            return border;
        }
    }
}
