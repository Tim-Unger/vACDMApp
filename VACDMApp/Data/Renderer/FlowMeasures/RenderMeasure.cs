namespace VACDMApp.Data.Renderer
{
    internal partial class FlowMeasures
    {
        private static readonly Color _darkBlue = new Color(28, 40, 54);

        private static readonly GridLength _oneStar = new GridLength(3, GridUnitType.Star);

        private static Grid RenderMeasure(FlowMeasure measure)
        {
            var grid = new Grid() { Background = _darkBlue, Margin = 10 };

            var status = GetStatusColor(measure);

            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(20, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(_oneStar));

            var contentGrid = new Grid();
            contentGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            contentGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            contentGrid.RowDefinitions.Add(new RowDefinition(_oneStar));

            var nameGrid = new Grid();

            var nameText = $@"{measure.Ident} -- {(status.IsActive ? "Active" : "Inactive")}";
            var nameLabel = new Label()
            {
                Text = nameText,
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            nameGrid.Children.Add(nameLabel);
            nameGrid.SetRow(nameLabel, 0);

            contentGrid.Children.Add(nameGrid);
            contentGrid.SetRow(nameGrid, 0);

            var dateGrid = new Grid();
            dateGrid.ColumnDefinitions.Add(new ColumnDefinition(_oneStar));
            dateGrid.ColumnDefinitions.Add(new ColumnDefinition(_oneStar));

            var timeSpanLabel = new Label()
            {
                Text = $"{measure.StartTime:dd.MM HH:mmZ} - {measure.EndTime:dd.MM HH:mmZ}",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            var typeLabel = new Label()
            {
                Text = $"{measure.Measure}",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            dateGrid.Children.Add(timeSpanLabel);

            dateGrid.SetColumn(timeSpanLabel, 1);

            contentGrid.Children.Add(dateGrid);
            grid.SetRow(dateGrid, 1);

            grid.Children.Add(contentGrid);
            grid.SetColumn(contentGrid, 0);

            var statusColorGrid = new Grid() { Background = status.Color };
            grid.Children.Add(statusColorGrid);
            grid.SetColumn(statusColorGrid, 1);

            return grid;
        }
    }
}
