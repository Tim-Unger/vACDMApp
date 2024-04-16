using System.Drawing;
using System.Text.RegularExpressions;
using VacdmApp.Data;
using Microsoft.Maui.Graphics;

namespace VacdmApp.Data.Renderer
{
    internal partial class SingleFlight
    {
        private static readonly string[] _defaultRegs = new string[]
        {
            "N172SP",
            "GFENX",
            "PMDG737",
            "ASXGS",
            "PMDG73",
            "N320SB",
            "N321SB",
            "N319SB",
            "PMDG"
        };

        private static Grid FlightDetailsGrid(
            VacdmPilot pilot,
            List<Airline> airlines,
            Flight_Plan flightPlan
        )
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
                Margin = new Thickness(20, 0, 0, 10),
                TextColor = Colors.White,
                Background = Colors.Transparent,
                FontAttributes = FontAttributes.Bold,
                FontSize = 30,
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

            if (string.IsNullOrEmpty(airline.iata))
            {
                airline.iata = icao;
            }

            var flightNumberOnly = pilot.Callsign.Remove(0, 3);

            var arrivalIcao = pilot.FlightPlan.Arrival;

            var arrAirportData =
                Data.Airports.FirstOrDefault(x => x.Icao == arrivalIcao)
                ?? new Airport() { Iata = arrivalIcao, Icao = arrivalIcao };

            var flightData =
                $"{airline.iata} {flightNumberOnly}, {pilot.FlightPlan.Arrival} ({arrAirportData.Iata}), {flightPlan.aircraft_short}";

            var regRegex = new Regex(@"REG/([A-Z0-9-]{3,6})");
            var isRegFiled = regRegex.IsMatch(flightPlan.remarks);

            if (isRegFiled)
            {
                var reg = regRegex.Match(flightPlan.remarks).Groups[1].Value.ToUpperInvariant();

                if (!_defaultRegs.Any(x => x == reg))
                {
                    flightData += $", {regRegex.Match(flightPlan.remarks).Groups[1].Value}";
                }
            }

            var flightDataLabel = new Label()
            {
                Text = flightData,
                TextColor = Colors.White,
                Background = Colors.Transparent,
                FontSize = 17,
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center
            };

            var statusLabel = new Label()
            {
                Margin = new Thickness(0, 10, 0, 10),
                TextColor = Colors.White,
                Background = Colors.Transparent,
                FontSize = 20,
                Text = "",
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.Center,
                FormattedText = new FormattedString()
            };

            statusLabel.FormattedText.Spans.Add(new Span() { BackgroundColor = Colors.White, TextColor = Microsoft.Maui.Graphics.Color.FromRgb(28, 40, 54), Text = $"  {Pilots.GetFlightStatus(pilot)}  " });

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
