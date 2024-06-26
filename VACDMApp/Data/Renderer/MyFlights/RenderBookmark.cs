﻿using System.Text.RegularExpressions;
using VacdmApp.Data;
using VacdmApp.Windows.BottomSheets;

namespace VacdmApp.Data.Renderer
{
    internal partial class Bookmarks
    {
        private static readonly Color _darkBlue = new(28, 40, 54);

        private static readonly GridLength _oneStar = new(3, GridUnitType.Star);

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

        internal static Grid RenderBookmark(VacdmPilot pilot)
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
                FontAttributes = FontAttributes.None,
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
                FontAttributes = FontAttributes.None,
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
                FontAttributes = FontAttributes.None,
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

            var airportData = Data.Airports.FirstOrDefault(
                x => x.Icao == pilot.FlightPlan.Departure
            );

            var airport =
                $"From {airportData?.Icao ?? pilot.FlightPlan.Departure} ({airportData?.Iata ?? ""})";

            var airportLabel = new Label()
            {
                Text = airport,
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
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

            var airlines = Data.Airlines;
            var flightPlan = Data.VatsimPilots
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
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
                FontSize = 15
            };
            var statusLabel = new Label()
            {
                Text = Pilots.GetFlightStatus(pilot),
                TextColor = Colors.White,
                Background = _darkBlue,
                FontAttributes = FontAttributes.None,
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

            SingleFlightBottomSheet.SelectedCallsign = callsign;

            var singleFlightSheet = new SingleFlightBottomSheet();

            await singleFlightSheet.ShowAsync();
        }
    }
}
