namespace VacdmApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static Border RenderNoFlightsFound()
        {
            var border = new Border() { Background = Colors.Transparent, StrokeThickness = 0 };
            var grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition(new GridLength(5, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(7, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(_oneStar));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(6, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(_oneStar));
            grid.RowDefinitions.Add(new RowDefinition(_oneStar));

            var noFlightsImage = new Image()
            {
                Source = "noflights.svg",
                HeightRequest = 100,
                WidthRequest = 100
            };

            var noFlightsText = "No vACDM Flights found\nCheck back later or refresh to try again";

            if(Data.Settings.DataSource is null)
            {
                noFlightsText = "Please select a Data-Source\nto show relevant flights";
            }

            var noFlightsLabel = new Label()
            {
                LineBreakMode = LineBreakMode.WordWrap,
                Text = noFlightsText,
                TextColor = Colors.White,
                Background = Colors.Black,
                Margin = new Thickness(0, 40),
                HeightRequest = 150,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.None,
                FontSize = 30
            };

            //TODO Button try again or open Settings

            grid.Children.Add(noFlightsLabel);
            grid.Children.Add(noFlightsImage);

            grid.SetRow(noFlightsImage, 1);
            grid.SetRow(noFlightsLabel, 4);

            border.Content = grid;
            return border;
        }
    }
}
