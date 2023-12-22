using System.Text.RegularExpressions;
using VACDMApp.VACDMData;
using VACDMApp.Windows.BottomSheets;

namespace VACDMApp.Data.Renderer
{
    internal partial class Bookmarks
    {
        private static readonly Color _darkBlue = new(28, 40, 54);

        private static readonly GridLength _oneStar = new(3, GridUnitType.Star);

        private static Grid RenderBookmark(VACDMPilot pilot)
        {
            var grid = new Grid() { Background = _darkBlue, Margin = new Thickness(10) };

            grid.ColumnDefinitions.Add(new ColumnDefinition(_oneStar));
            grid.ColumnDefinitions.Add(new ColumnDefinition(_oneStar));
            grid.ColumnDefinitions.Add(
                new ColumnDefinition(new GridLength(0.3, GridUnitType.Star))
            );

            var timeGrid = new Grid();

            timeGrid.RowDefinitions.Add(new RowDefinition(new GridLength(2, GridUnitType.Star)));
            timeGrid.RowDefinitions.Add(new RowDefinition(_oneStar));
            timeGrid.RowDefinitions.Add(new RowDefinition(_oneStar));

            var eobtLabel = new Label()
            {
                Text = pilot.Vacdm.Eobt.ToString("HH:mmZ"),
                Margin = new Thickness(20, 0, 0, 0),
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.End
            };

            timeGrid.Children.Add(eobtLabel);
            timeGrid.SetRow(eobtLabel, 0);

            var tobtLabel = new Label()
            {
                Text = pilot.Vacdm.Tobt.ToString("HH:mmZ"),
                Margin = new Thickness(20, 0, 0, 0),
                TextColor = Colors.White,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.End
            };

            timeGrid.Children.Add(tobtLabel);
            timeGrid.SetRow(tobtLabel, 1);

            var tsatColor = SingleFlight.GetTsatBackgroundColor(pilot.Vacdm.Tsat, pilot.Vacdm.Tobt);

            var tsatLabel = new Label()
            {
                Text = pilot.Vacdm.Tsat.ToString("HH:mmZ"),
                Margin = new Thickness(20, 0, 0, 0),
                TextColor = tsatColor,
                FontAttributes = FontAttributes.Bold,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center
            };

            timeGrid.Children.Add(tsatLabel);
            timeGrid.SetRow(tsatLabel, 2);

            var flightGrid = new Grid();
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
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18
            };

            var callsign = pilot.Callsign;
            var callsignLabel = new Label()
            {
                Text = callsign,
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 25
            };

            var airlines = VACDMData.Data.Airlines;
            var flightPlan = VACDMData.Data.VatsimPilots
                .First(x => x.callsign == pilot.Callsign)
                .flight_plan;

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
                $"{airline.iata} {flightNumberOnly}, {pilot.FlightPlan.Arrival} ({airportData.Iata}), {flightPlan.aircraft_short}";

            //TODO Fix
            //var regRegex = new Regex(@"REG/([A-Z0-9-]{3,6})");
            //var hasRegFiled = regRegex.IsMatch(flightPlan.remarks);

            //if (hasRegFiled)
            //{
            //    var reg = regRegex.Match(flightPlan.remarks).Groups[1].Value;
            //    flightData += $", {reg}";
            //}

            var flightDataLabel = new Label()
            {
                Text = flightData,
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15
            };
            var statusLabel = new Label()
            {
                Text = Pilots.GetFlightStatus(pilot),
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 15
            };

            var button = new Button() { BackgroundColor = Colors.Transparent };
            button.Clicked += async (sender, e) => await Button_Clicked(sender, e);
            grid.Children.Add(button);

            grid.SetRowSpan(button, 5);
            grid.SetColumnSpan(button, 5);

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

            grid.SetColumn(timeGrid, 0);
            grid.SetColumn(flightGrid, 1);

            return grid;
        }

        private static async Task Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var parentGrid = (Grid)button.Parent;
            var callsignGrid = (Grid)parentGrid.Children[2];
            var callsignLabel = (Label)callsignGrid.Children[1];
            var callsign = callsignLabel.Text;

            var singleFlightSheet = new SingleFlightBottomSheet();

            SingleFlightBottomSheet.SelectedCallsign = callsign;

            singleFlightSheet.ShowAsync();
        }
    }
}
