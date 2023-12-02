using VACDMApp.VACDMData;

namespace VACDMApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static Grid FlightDetailsGrid(VACDMPilot pilot, List<Airline> airlines, Flight_Plan flightPlan)
        {
            var flightDetailsGrid = new Grid();
            flightDetailsGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(3, GridUnitType.Star))
            );

            flightDetailsGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(2, GridUnitType.Star))
            );

            flightDetailsGrid.RowDefinitions.Add(
                new RowDefinition(new GridLength(2, GridUnitType.Star))
            );

            var callsignLabel = new Label()
            {
                Text = pilot.Callsign,
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.Bold,
                FontSize = 25,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center
            };

            var icao = pilot.Callsign[..3].ToUpper();
            var airline =
                airlines.Find(x => x.icao == icao)
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

            var flightDataLabel = new Label()
            {
                Text = flightData,
                TextColor = Colors.White,
                Background = _darkBlue,
                FontSize = 17,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center
            };

            //TODO Different Background Color
            var statusLabel = new Label()
            {
                Text = Pilots.GetFlightStatus(pilot),
                TextColor = Colors.White,
                Background = _darkBlue,
                FontSize = 20,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center
            };

            flightDetailsGrid.Children.Add(callsignLabel);
            flightDetailsGrid.Children.Add(flightDataLabel);
            flightDetailsGrid.Children.Add(statusLabel);

            flightDetailsGrid.SetRow(callsignLabel, 0);
            flightDetailsGrid.SetRow(flightDataLabel, 1);
            flightDetailsGrid.SetRow(statusLabel, 2);

            return flightDetailsGrid;
        }
    }
}
