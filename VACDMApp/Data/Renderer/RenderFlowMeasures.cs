using static VACDMApp.VACDMData.Data;

namespace VACDMApp.Data.Renderer
{
    internal class FlowMeasures
    {
        private static readonly Color DarkBlue = new Color(28, 40, 54);

        private static readonly GridLength OneStar = new GridLength(3, GridUnitType.Star);

        internal static List<Grid> Render()
        {
            //TODO Update Data
            var measures = VACDMData.Data.FlowMeasures;

            if (measures.Count == 0)
            {
                return new() { RenderNoMeasuresFound() };
            }

            return measures.Select(RenderMeasure).ToList();
        }

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

        private static Grid RenderNoMeasuresFound()
        {
            var grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition(new GridLength(5, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(5, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(OneStar));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(5, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(OneStar));

            var noMeasuresImage = new Image() { Source = "noflights.svg", HeightRequest = 100, WidthRequest = 100 };

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
                FontAttributes = FontAttributes.Bold,
                FontSize = 30
            };

            grid.Children.Add(noMeasuresImage);
            grid.Children.Add(noMeasuresLabel);

            grid.SetRow(noMeasuresImage, 1);
            grid.SetRow(noMeasuresLabel, 4);

            return grid;
        }

        private static (Color Color, bool IsActive) GetStatusColor(FlowMeasure measure)
        {
            var now = DateTime.UtcNow;
            var startDate = measure.StartTime;
            var endDate = measure.EndTime;

            //Mesure is withdrawn
            if (measure.WithdrawnAt is not null)
            {
                return (Colors.Red, false);
            }

            //Now is later than EndDate => Measure is in the past
            if (now > endDate)
            {
                return (Colors.Red, false);
            }

            //Now is earlier than 24 hours before the Measure
            if (now < startDate.AddHours(-24))
            {
                return (Colors.Red, false);
            }

            //Now is less than 24 hours before the measure (no AddHours needed since we have the guard clause above
            if (now < startDate)
            {
                return (Colors.Yellow, false);
            }

            //if(now > startDate && now < endDate)
            //{
            return (Colors.Green, true);
            //}
        }
    }
}
