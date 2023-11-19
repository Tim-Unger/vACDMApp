using Microsoft.Maui.Controls.Shapes;

namespace VACDMApp.VACDMData.Renderer
{
    internal class SingleFlight : MainPage
    {
        internal static Grid RenderGrid(string callsign)
        {
            var flightPlan = VatsimPilots.First(x => x.callsign == callsign).flight_plan ?? throw new Exception();
            var airlines = Airlines;
            var pilot = VACDMPilots.First(x => x.Callsign == callsign);

            var grid = new Grid
            {
                Margin = 10
            };

            grid.RowDefinitions.Add(new RowDefinition(new GridLength(2, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(10, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(5, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(10, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(40, GridUnitType.Star))); //Placeholder bottom

            var placeholderRectangle = new Rectangle() { BackgroundColor = new Color(23, 23, 23), HeightRequest = 300 };
            grid.Children.Add(placeholderRectangle);
            grid.SetRow(placeholderRectangle, 6);

            var airport = new Label() { Text = $"From: {pilot.FlightPlan.Departure}", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 15 };
            grid.Children.Add(airport);
            grid.SetRow(airport, 0);

            var flightInfoGrid = new Grid() { Background = new Color(28, 40, 54) };
            flightInfoGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(5, GridUnitType.Star)));
            flightInfoGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(10, GridUnitType.Star)));

            var timeDateGrid = new Grid();
            timeDateGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            timeDateGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));

            var eobtLabel = new Label() { Text = pilot.Vacdm.Eobt.ToString("HH:mmZ"), TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };
            var dateLabel = new Label() { Text = DateOnly.FromDateTime(DateTime.UtcNow).ToShortDateString(), TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };

            timeDateGrid.Children.Add(eobtLabel);
            timeDateGrid.Children.Add(dateLabel);

            timeDateGrid.SetRow(eobtLabel, 0);
            timeDateGrid.SetRow(dateLabel, 1);

            flightInfoGrid.Children.Add(timeDateGrid);
            flightInfoGrid.SetColumn(timeDateGrid, 0);

            var flightDetailsGrid = new Grid();
            flightDetailsGrid.RowDefinitions.Add(new RowDefinition(new GridLength(3, GridUnitType.Star)));
            flightDetailsGrid.RowDefinitions.Add(new RowDefinition(new GridLength(2, GridUnitType.Star)));
            flightDetailsGrid.RowDefinitions.Add(new RowDefinition(new GridLength(2, GridUnitType.Star)));

            var callsignLabel = new Label() { Text = pilot.Callsign, TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 25 };

            var icao = pilot.Callsign[..3].ToUpper();
            var airline = airlines.First(x => x.icao == icao) ?? throw new Exception("ICAO not found");
            var flightNumberOnly = pilot.Callsign.Remove(0, 3);
            var flightData = $"{airline.iata} {flightNumberOnly}, {pilot.FlightPlan.Arrival}, {flightPlan.aircraft_short}";

            var flightDataLabel = new Label() { Text = flightData, TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };

            var statusLabel = new Label() { Text = FlightInfos.GetFlightStatus(pilot), TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };

            flightDetailsGrid.Children.Add(callsignLabel);
            flightDetailsGrid.Children.Add(flightDataLabel);
            flightDetailsGrid.Children.Add(statusLabel);

            flightDetailsGrid.SetRow(callsignLabel, 0);
            flightDetailsGrid.SetRow(flightDataLabel, 1);
            flightDetailsGrid.SetRow(statusLabel, 2);

            flightInfoGrid.Children.Add(flightDetailsGrid);
            flightInfoGrid.SetColumn(flightDetailsGrid, 1);

            var vacdm = pilot.Vacdm;

            var timesInfoGrid = new Grid();
            timesInfoGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            timesInfoGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            timesInfoGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));

            var timesFirstColumnGrid = new Grid();
            timesFirstColumnGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            timesFirstColumnGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            var eobtTimeLabel = new Label() { Text = $"EOBT: {vacdm.Eobt:HH:mmZ}", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };
            var tobtTimeLabel = new Label() { Text = $"TOBT: {vacdm.Tobt:HH:mmZ}", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };

            timesFirstColumnGrid.Children.Add(eobtTimeLabel);
            timesFirstColumnGrid.Children.Add(tobtTimeLabel);

            timesFirstColumnGrid.SetColumn(eobtTimeLabel, 0);
            timesFirstColumnGrid.SetColumn(tobtTimeLabel, 1);

            var timesSecondRowGrid = new Grid();
            timesSecondRowGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            timesSecondRowGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            var ctotTimeLabel = new Label() { Text = $"CTOT: {vacdm.Ctot:HH:mmZ}", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };
            var tsatTimeLabel = new Label() { Text = $"TSAT: {vacdm.Tsat:HH:mmZ}", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };

            timesSecondRowGrid.Children.Add(ctotTimeLabel);
            timesSecondRowGrid.Children.Add(tsatTimeLabel);

            timesSecondRowGrid.SetColumn(ctotTimeLabel, 0);
            timesSecondRowGrid.SetColumn(tsatTimeLabel, 1);


            var timesThirdColumnGrid = new Grid();
            timesThirdColumnGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            var delayLabel = new Label() { Text = $"Delay: {vacdm.DelayMin} min.", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };

            timesThirdColumnGrid.Children.Add(delayLabel);

            timesThirdColumnGrid.SetColumn(timesThirdColumnGrid, 0);

            timesInfoGrid.Children.Add(timesFirstColumnGrid);
            timesInfoGrid.SetRow(timesFirstColumnGrid, 0);

            timesInfoGrid.Children.Add(timesSecondRowGrid);
            timesInfoGrid.SetRow(timesSecondRowGrid, 1);

            timesInfoGrid.Children.Add(timesThirdColumnGrid);
            timesInfoGrid.SetRow(timesThirdColumnGrid, 2);


            var flightPositionGrid = new Grid();
            flightPositionGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));

            var flightPositionFirstGrid = new Grid();
            flightPositionFirstGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionFirstGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            var gateLabel = new Label() { Text = $"Gate-Area: {vacdm.TaxiZone}", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };
            var sidLabel = new Label() { Text = $"SID: {pilot.Clearance.Sid}", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };

            flightPositionFirstGrid.Children.Add(gateLabel);
            flightPositionFirstGrid.Children.Add(sidLabel);

            flightPositionFirstGrid.SetColumn(gateLabel, 0);
            flightPositionFirstGrid.SetColumn(sidLabel, 1);

            var flightPositionSecondGrid = new Grid();
            flightPositionSecondGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionSecondGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            var rwyLabel = new Label() { Text = $"RWY: {pilot.Clearance.DepRwy}", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };
            var taxitimeLabel = new Label() { Text = $"Taxi-Time: {pilot.Vacdm.exot} min.", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };

            flightPositionSecondGrid.Children.Add(rwyLabel);
            flightPositionSecondGrid.Children.Add(taxitimeLabel);

            flightPositionSecondGrid.SetColumn(rwyLabel, 0);
            flightPositionSecondGrid.SetColumn(taxitimeLabel, 1);

            var flightPositionThirdGrid = new Grid();
            flightPositionThirdGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionThirdGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            var squawkLabel = new Label() { Text = $"Squawk: {pilot.Clearance.AssignedSquawk}", TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };
            var correctSquawkText = pilot.Clearance.AssignedSquawk == pilot.Clearance.CurrentSquawk ? "Correct Squawk Set" : "Wrong Squawk Set";
            var correctSquawkLabel = new Label() { Text = correctSquawkText, TextColor = Colors.White, Background = new Color(23, 23, 23), FontAttributes = FontAttributes.Bold, FontSize = 20 };

            flightPositionThirdGrid.Children.Add(squawkLabel);
            flightPositionThirdGrid.Children.Add(correctSquawkLabel);

            flightPositionThirdGrid.SetColumn(squawkLabel, 0);
            flightPositionThirdGrid.SetColumn(correctSquawkLabel, 1);

            flightPositionGrid.Children.Add(flightPositionFirstGrid);
            flightPositionGrid.SetRow(flightPositionFirstGrid, 0);
            flightPositionGrid.Children.Add(flightPositionSecondGrid);
            flightPositionGrid.SetRow(flightPositionSecondGrid, 1);
            flightPositionGrid.Children.Add(flightPositionThirdGrid);
            flightPositionGrid.SetRow(flightPositionThirdGrid, 2);

            grid.Children.Add(flightInfoGrid);
            grid.SetRow(flightInfoGrid, 1);

            grid.Children.Add(timesInfoGrid);
            grid.SetRow(timesInfoGrid, 3);

            grid.Children.Add(flightPositionGrid);
            grid.SetRow(flightPositionGrid, 5);

            return grid;
        }
    }
}
