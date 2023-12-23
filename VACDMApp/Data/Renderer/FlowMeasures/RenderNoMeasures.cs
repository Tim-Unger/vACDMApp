namespace VACDMApp.Data.Renderer
{
    internal partial class FlowMeasures
    {
        private static Grid RenderNoMeasuresFound()
        {
            var grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition(new GridLength(5, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(5, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(_oneStar));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(5, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(_oneStar));

            var noMeasuresImage = new Image()
            {
                Source = "noflights.svg",
                HeightRequest = 100,
                WidthRequest = 100
            };

            var noMeasuresLabel = new Label()
            {
                LineBreakMode = LineBreakMode.WordWrap,
                Text = "No ECFMP Measures found\nCheck back later or refresh to try again",
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

            grid.Children.Add(noMeasuresImage);
            grid.Children.Add(noMeasuresLabel);

            grid.SetRow(noMeasuresImage, 1);
            grid.SetRow(noMeasuresLabel, 4);

            return grid;
        }
    }
}
