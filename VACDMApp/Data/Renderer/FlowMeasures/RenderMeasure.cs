namespace VACDMApp.Data.Renderer
{
    internal partial class FlowMeasures
    {
        private static readonly Color DarkBlue = new Color(28, 40, 54);

        private static readonly GridLength OneStar = new GridLength(3, GridUnitType.Star);

        private static Grid RenderMeasure(FlowMeasure measure)
        {
            var grid = new Grid() { Background = DarkBlue, Margin = 10 };

            var status = GetStatusColor(measure);

            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(20, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(OneStar));

            var contentGrid = new Grid();
            contentGrid.RowDefinitions.Add(new RowDefinition(OneStar));
            contentGrid.RowDefinitions.Add(new RowDefinition(OneStar));
            contentGrid.RowDefinitions.Add(new RowDefinition(OneStar));

            var nameGrid = new Grid();

            var nameText = $@"{measure.Ident} {(status.IsActive ? "Active" : "Inactive")}";
            var nameLabel = new Label()
            {
                Text = nameText,
                TextColor = Colors.White,
                Background = DarkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            nameGrid.Children.Add(nameLabel);
            nameGrid.SetRow(nameLabel, 0);

            contentGrid.Children.Add(nameGrid);
            contentGrid.SetRow(nameGrid, 0);

            grid.Children.Add(contentGrid);
            grid.SetColumn(contentGrid, 0);

            var statusColorGrid = new Grid() { Background = status.Color };
            grid.Children.Add(statusColorGrid);
            grid.SetColumn(statusColorGrid, 1);

            return grid;
        }
    }
}
