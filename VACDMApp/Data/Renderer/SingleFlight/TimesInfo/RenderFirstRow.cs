using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Grid FlightInfoFirstRowGrid(Vacdm vacdm)
        {
            var timesFirstColumnGrid = new Grid();
            timesFirstColumnGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            timesFirstColumnGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            timesFirstColumnGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );
            timesFirstColumnGrid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(1, GridUnitType.Star))
            );

            var _timeMargin = new Thickness(0, 10, 10, 10);

            var eobtTextLabel = new Label()
            {
                Text = $"EOBT:",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                VerticalTextAlignment = TextAlignment.Center
            };
            var eobtTimeLabel = new Label()
            {
                Text = $"{vacdm.Eobt:HH:mmZ}",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = _timeMargin
            };

            var ctot = vacdm.Ctot;

            if (ctot.Hour == 23 && ctot.Minute == 59)
            {
                ctot = vacdm.Tsat.AddMinutes(vacdm.Exot);
            }

            var ctotTextLabel = new Label()
            {
                Text = $"CTOT:",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                VerticalTextAlignment = TextAlignment.Center
            };
            var ctotTimeLabel = new Label()
            {
                Text = $"{ctot:HH:mmZ}",
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                Margin = _timeMargin
            };

            timesFirstColumnGrid.Children.Add(eobtTextLabel);
            timesFirstColumnGrid.Children.Add(eobtTimeLabel);
            timesFirstColumnGrid.Children.Add(ctotTextLabel);
            timesFirstColumnGrid.Children.Add(ctotTimeLabel);

            timesFirstColumnGrid.SetColumn(eobtTextLabel, 0);
            timesFirstColumnGrid.SetColumn(eobtTimeLabel, 1);
            timesFirstColumnGrid.SetColumn(ctotTextLabel, 2);
            timesFirstColumnGrid.SetColumn(ctotTimeLabel, 3);

            return timesFirstColumnGrid;
        }
    }
}
