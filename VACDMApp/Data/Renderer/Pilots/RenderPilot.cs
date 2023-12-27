using System.Text.RegularExpressions;
using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class Pilots
    {
        private static readonly Color _darkBlue = new(28, 40, 54);

        private static readonly GridLength _oneStar = new(3, GridUnitType.Star);

        private static Border RenderPilot(VACDMPilot pilot)
        {
            //TODO Error handling here when ACDM FP not found
            var flightPlan =
                VACDMData.Data.VatsimPilots.Find(x => x.callsign == pilot.Callsign).flight_plan
                ?? throw new InvalidDataException("");
            var airlines = VACDMData.Data.Airlines;

            var border = new Border() { StrokeThickness = 0, Stroke = Color.FromArgb("#454545") };
            var parentGridContainer = new Grid() { Background = _darkBlue };
            var grid = new Grid() { Margin = new Thickness(10) };

            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(3, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(0.3, GridUnitType.Star))
            );
            grid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(0.3, GridUnitType.Star))
            );

            var timeGrid = new Grid();

            grid.SetColumn(timeGrid, 0);

            var eobt = new Label()
            {
                Text = pilot.Vacdm.Eobt.ToString("HH:mmZ"),
                Margin = new Thickness(20, 0, 0, 0),
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            timeGrid.Children.Add(eobt);
            timeGrid.SetRow(eobt, 0);

            var flightGrid = new Grid() { HorizontalOptions = LayoutOptions.Start };
            flightGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            flightGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            flightGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            flightGrid.RowDefinitions.Add(new RowDefinition(_oneStar));

            grid.SetColumn(flightGrid, 1);

            var airportData = VACDMData.Data.Airports.FirstOrDefault(
                x => x.Icao == pilot.FlightPlan.Departure
            );

            var airport =
                $"From {airportData?.Icao ?? pilot.FlightPlan.Departure} ({airportData?.Iata ?? ""})";

            var airportLabel = new Label()
            {
                Text = airport,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start
            };

            var callsign = pilot.Callsign;
            var callsignLabel = new Label()
            {
                Text = callsign,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start
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

            if (string.IsNullOrEmpty(airline.iata))
            {
                airline.iata = icao;
            }

            var flightNumberOnly = pilot.Callsign.Remove(0, 3);

            var arrivalIcao = pilot.FlightPlan.Arrival;

            var arrAirportData = VACDMData.Data.Airports.FirstOrDefault(
                x => x.Icao == arrivalIcao
            ) ?? new Airport() { Iata = arrivalIcao, Icao = arrivalIcao };

            var flightData =
                $"{airline.iata} {flightNumberOnly}, {pilot.FlightPlan.Arrival} ({arrAirportData.Iata}), {flightPlan.aircraft_short}";

            var regRegex = new Regex(@"REG/([A-Z0-9-]{3,6})");
            var hasRegFiled = regRegex.IsMatch(flightPlan.remarks);

            var regMatch = regRegex.Match(flightPlan.remarks)?.Groups[1].Value;

            var defaultRegs = new string[] { "N172SP", "GFENX", "PMDG737", "ASXGS" };

            if (defaultRegs.Any(x => x == regMatch))
            {
                hasRegFiled = false;
            }

            flightData = hasRegFiled ? $"{flightData}, {regMatch}" : $"{flightData}     ";

            var flightDataLabel = new Label()
            {
                Text = flightData,
                TextColor = Colors.White,
                FontAttributes = FontAttributes.None,
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Start
            };
            var statusLabel = new Label()
            {
                Text = GetFlightStatus(pilot),
                TextColor = Colors.White,
                FontAttributes = FontAttributes.None,
                FontSize = 15,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Start
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

            //TODO Padding is fucked up
            //TODO Button Alignment
            var bookmarkGrid = new Grid() { Padding = new Thickness(10)};

            bookmarkGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            bookmarkGrid.RowDefinitions.Add(new RowDefinition(_oneStar));

            //TODO not woking
            var isPilotBookmarked =
                VACDMData.Data.BookmarkedPilots.FirstOrDefault(x => x.Callsign == pilot.Callsign)
                != null;
            var bookmarkImageSource = isPilotBookmarked ? "bookmark.svg" : "bookmark_outline.svg";

            var bookmarkImage = new Image() { Source = bookmarkImageSource, Scale = 0.5};

            var bookmarkButton = new Button() { Background = Colors.Transparent, Scale = 1.5 };

            bookmarkButton.Clicked += async (sender, e) => await BookmarkButton_Clicked(sender, e);

            bookmarkGrid.Children.Add(bookmarkButton);
            bookmarkGrid.Children.Add(bookmarkImage);

            grid.Children.Add(bookmarkGrid);
            grid.SetColumn(bookmarkGrid, 3);

            parentGridContainer.Children.Add(grid);
            parentGridContainer.Children.Add(button);

            border.Content = parentGridContainer;
            return border;
        }
    }
}
