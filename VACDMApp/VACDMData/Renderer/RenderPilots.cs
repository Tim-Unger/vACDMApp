using VACDMApp.Windows.BottomSheets;
using VACDMApp.Windows.Views;

namespace VACDMApp.VACDMData.Renderer
{
    internal class FlightInfos : MainPage
    {
        internal static List<Grid> Render()
        {
            var elements = VACDMPilots.Select(RenderPilot).ToList();

            return elements;
        }

        private static Grid RenderPilot(VACDMPilot pilot)
        {
            var flightPlan = VatsimPilots.First(x => x.callsign == pilot.Callsign).flight_plan ?? throw new Exception();
            var airlines = Airlines;
            var grid = new Grid() { Background = new Color(23, 23, 23), Margin = 10 };
            
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(3, GridUnitType.Star)));

            var timeGrid = new Grid();
            timeGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            timeGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));

            grid.SetColumn(timeGrid, 0);

            var eobt = new Label() { Text = pilot.Vacdm.Eobt.ToString("HH:mmZ"), TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };
            var tsat = new Label() { Text = pilot.Vacdm.Tsat.ToString("HH:mmZ"), TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.Center, VerticalTextAlignment = TextAlignment.Center };

            timeGrid.Children.Add(eobt);
            timeGrid.Children.Add(tsat);
            timeGrid.SetRow(eobt, 0);
            timeGrid.SetRow(tsat, 1);

            var flightGrid = new Grid();
            flightGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            flightGrid.RowDefinitions.Add(new RowDefinition(new GridLength(2, GridUnitType.Star)));
            flightGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            flightGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));

            grid.SetColumn(flightGrid, 1);

            var airport = $"From {pilot.FlightPlan.Departure}";
            var airportLabel = new Label() { Text = airport, TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 18 };

            var callsign = pilot.Callsign;
            var callsignLabel = new Label() { Text = callsign, TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 25 };

            var icao = pilot.Callsign[..3].ToUpper();
            var airline = airlines.First(x => x.icao == icao) ?? throw new Exception("ICAO not found");
            var flightNumberOnly = pilot.Callsign.Remove(0, 3);
            var flightData = $"{airline.iata} {flightNumberOnly}, {pilot.FlightPlan.Arrival}, {flightPlan.aircraft_short}";
            var flightDataLabel = new Label() { Text = flightData, TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 15 };
            var statusLabel = new Label() { Text = GetFlightStatus(pilot), TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 15 };

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

            return grid;
        }

        private static void Button_Clicked(object sender, EventArgs e)
        {
            var button = (Button)sender;

            var parentGrid = (Grid)button.Parent;

            var callsignGrid = (Grid)parentGrid.Children[1];

            var callsignLabel = (Label)callsignGrid.Children[1];

            var callsign = callsignLabel.Text;

            SingleFlightBottomSheet.SelectedCallsign = callsign;

            var singleFlightSheet = new SingleFlightBottomSheet();

            singleFlightSheet.ShowAsync();
            
        }

        public static string GetFlightStatus(VACDMPilot pilot)
        {
            var vatsimPilot = MainPage.VatsimPilots.First(x => x.callsign == pilot.Callsign);
            if (vatsimPilot.groundspeed > 50)
            {
                return "Departed";
            }

            var vacdm = pilot.Vacdm;

            if(vacdm.Sug.Year == 1969)
            {
                return "Preflight/Boarding";
            }

            if (vacdm.Pbg.Year == 1969)
            {
                return "Startup given";
            }

            if (vacdm.Txg.Year == 1969)
            {
                return "Pushback";
            }

            return "Taxi Out";
        }
        
    }
}
