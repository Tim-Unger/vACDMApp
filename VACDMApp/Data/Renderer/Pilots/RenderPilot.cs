using System.Text.RegularExpressions;
using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static readonly Color DarkBlue = new(28, 40, 54);

        private static readonly GridLength OneStar = new(3, GridUnitType.Star);

        private static Border RenderPilot(VACDMPilot pilot)
        {
            //TODO Error handling here when ACDM FP not found
            var flightPlan =
                VACDMData.Data.VatsimPilots.FirstOrDefault(x => x.callsign == pilot.Callsign).flight_plan
                ?? throw new Exception();
            var airlines = VACDMData.Data.Airlines;

            var border = new Border() { StrokeThickness = 0, Stroke = Color.FromArgb("#454545") };
            var parentGridContainer = new Grid() { Background = DarkBlue };
            var grid = new Grid() { Margin = new Thickness(10) };

            grid.ColumnDefinitions.Add(new ColumnDefinition(OneStar));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(3, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(0.5, GridUnitType.Star))
            );

            var timeGrid = new Grid();
            timeGrid.RowDefinitions.Add(new RowDefinition(new GridLength(4, GridUnitType.Star)));
            timeGrid.RowDefinitions.Add(new RowDefinition(OneStar));

            grid.SetColumn(timeGrid, 0);

            var eobt = new Label()
            {
                Text = pilot.Vacdm.Eobt.ToString("HH:mmZ"),
                Margin = new Thickness(20, 0, 0, 0),
                TextColor = Colors.White,
                Background = DarkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.End
            };

            timeGrid.Children.Add(eobt);
            timeGrid.SetRow(eobt, 0);

            var flightGrid = new Grid();
            flightGrid.RowDefinitions.Add(new RowDefinition(OneStar));
            flightGrid.RowDefinitions.Add(new RowDefinition(OneStar));
            flightGrid.RowDefinitions.Add(new RowDefinition(OneStar));
            flightGrid.RowDefinitions.Add(new RowDefinition(OneStar));

            grid.SetColumn(flightGrid, 1);

            var airport = $"From {pilot.FlightPlan.Departure}";
            var airportLabel = new Label()
            {
                Text = airport,
                TextColor = Colors.White,
                Background = DarkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18
            };

            var callsign = pilot.Callsign;
            var callsignLabel = new Label()
            {
                Text = callsign,
                TextColor = Colors.White,
                Background = DarkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 25
            };

            var icao = pilot.Callsign[..3].ToUpper();
            var airline =
                airlines.FirstOrDefault(x => x.icao == icao)
                ?? new Airline()
                {
                    callsign = "",
                    country = "",
                    iata = icao,
                    icao = icao,
                    name = ""
                };

            var flightNumberOnly = pilot.Callsign.Remove(0, 3);
            var flightData =
                $"{airline.iata} {flightNumberOnly}, {pilot.FlightPlan.Arrival}, {flightPlan.aircraft_short}";

            var regRegex = new Regex(@"REG/([A-Z0-9-]{3,6})");
            var hasRegFiled = regRegex.IsMatch(flightPlan.remarks);

            if (hasRegFiled)
            {
                var reg = regRegex.Match(flightPlan.remarks).Groups[0].Value;
                flightData += $", {reg}";
            }

            var flightDataLabel = new Label()
            {
                Text = flightData,
                TextColor = Colors.White,
                Background = DarkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15
            };
            var statusLabel = new Label()
            {
                Text = GetFlightStatus(pilot),
                TextColor = Colors.White,
                Background = DarkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15
            };

            flightGrid.Children.Add(airportLabel);
            flightGrid.Children.Add(callsignLabel);
            flightGrid.Children.Add(flightDataLabel);
            flightGrid.Children.Add(statusLabel);

            flightGrid.SetRow(airportLabel, 0);
            flightGrid.SetRow(callsignLabel, 1);
            flightGrid.SetRow(flightDataLabel, 2);
            flightGrid.SetRow(statusLabel, 3);

            grid.Children.Add(timeGrid);
            grid.Children.Add(flightGrid);

            var button = new Button() { BackgroundColor = Colors.Transparent };
            button.Clicked += Button_Clicked;
            grid.Children.Add(button);

            grid.SetRowSpan(button, 5);
            grid.SetColumnSpan(button, 5);


            //TODO Padding is fucked up
            var bookmarkGrid = new Grid
            {
                Padding = new Thickness(5),
            };

            var bookmarkButton = new ImageButton()
            {

                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
                Source = "bookmark_outline.svg",
                HeightRequest = 25,
                WidthRequest = 25
            };

            bookmarkButton.Clicked += BookmarkButton_Clicked;

            bookmarkGrid.Children.Add(bookmarkButton);

            grid.Children.Add(bookmarkGrid);
            grid.SetColumn(bookmarkGrid, 2);

            parentGridContainer.Children.Add(grid);

            border.Content = parentGridContainer;
            return border;
        }
    }
}
