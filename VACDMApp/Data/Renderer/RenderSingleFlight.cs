using Microsoft.Maui.Controls.Shapes;

namespace VACDMApp.VACDMData.Renderer
{
    internal class SingleFlight : MainPage
    {
        private static readonly Color DarkBlue = new(28, 40, 54);

        private static readonly Color DarkGrey = new(23, 23, 23);

        internal static Grid RenderGrid(string callsign)
        {
            var flightPlan = Data.VatsimPilots.First(x => x.callsign == callsign).flight_plan ?? throw new Exception();
            var airlines = Data.Airlines;
            var pilot = Data.VACDMPilots.First(x => x.Callsign == callsign);

            var grid = new Grid
            {
                BackgroundColor = DarkGrey
            };

            grid.RowDefinitions.Add(new RowDefinition(new GridLength(10, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(20, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(10, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(20, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(10, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(20, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(50, GridUnitType.Star))); //Placeholder bottom

            var placeholderRectangle = new Rectangle() { BackgroundColor = DarkGrey, HeightRequest = 300 };
            grid.Children.Add(placeholderRectangle);
            grid.SetRow(placeholderRectangle, 6);

            var airport = new Label() { Text = $"From: {pilot.FlightPlan.Departure}", TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 15 };
            grid.Children.Add(airport);
            grid.SetRow(airport, 0);

            var flightInfoGrid = new Grid() { Padding = 10, Margin = 10, BackgroundColor = DarkBlue };
            flightInfoGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(5, GridUnitType.Star)));
            flightInfoGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(10, GridUnitType.Star)));

            var timeDateGrid = new Grid();
            timeDateGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            timeDateGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));

            var eobtLabel = new Label() { Text = pilot.Vacdm.Eobt.ToString("HH:mmZ"), TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 25, VerticalTextAlignment = TextAlignment.Center };
            var dateLabel = new Label() { Text = DateOnly.FromDateTime(DateTime.UtcNow).ToShortDateString(), TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 25, VerticalTextAlignment = TextAlignment.Center };

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

            var callsignLabel = new Label() { Text = pilot.Callsign, TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 25, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center };

            var icao = pilot.Callsign[..3].ToUpper();
            var airline = airlines.Find(x => x.icao == icao) ?? new Airline() { callsign = "", country = "", iata = icao, icao = icao, name = "" };
            var flightNumberOnly = pilot.Callsign.Remove(0, 3);
            var flightData = $"{airline.iata} {flightNumberOnly}, {pilot.FlightPlan.Arrival}, {flightPlan.aircraft_short}";

            var flightDataLabel = new Label() { Text = flightData, TextColor = Colors.White, Background = DarkBlue, FontSize = 17, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center };

            //TODO Different Background Color
            var statusLabel = new Label() { Text = FlightInfos.GetFlightStatus(pilot), TextColor = Colors.White, Background = DarkBlue, FontSize = 20, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center };

            flightDetailsGrid.Children.Add(callsignLabel);
            flightDetailsGrid.Children.Add(flightDataLabel);
            flightDetailsGrid.Children.Add(statusLabel);

            flightDetailsGrid.SetRow(callsignLabel, 0);
            flightDetailsGrid.SetRow(flightDataLabel, 1);
            flightDetailsGrid.SetRow(statusLabel, 2);

            flightInfoGrid.Children.Add(flightDetailsGrid);
            flightInfoGrid.SetColumn(flightDetailsGrid, 1);

            var vacdm = pilot.Vacdm;

            var timesInfoGrid = new Grid() { Padding = 10, Margin = 10, BackgroundColor = DarkBlue };
            timesInfoGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            timesInfoGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            timesInfoGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));

            var timesFirstColumnGrid = new Grid();
            timesFirstColumnGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            timesFirstColumnGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            timesFirstColumnGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            timesFirstColumnGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            var timeMargin = new Thickness(0, 10, 10, 10);

            var eobtTextLabel = new Label() { Text = $"EOBT:", TextColor = Colors.White, Background = DarkBlue, FontSize = 20, VerticalTextAlignment = TextAlignment.Center };
            var eobtTimeLabel = new Label() { Text = $"{vacdm.Eobt:HH:mmZ}", TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, Margin = timeMargin };

            var ctot = vacdm.Ctot;

            if(ctot.Hour == 23 && ctot.Minute == 59)
            {
                ctot = vacdm.Tsat.AddMinutes(vacdm.exot);
            }

            var ctotTextLabel = new Label() { Text = $"CTOT:", TextColor = Colors.White, Background = DarkBlue, FontSize = 20, VerticalTextAlignment = TextAlignment.Center };
            var ctotTimeLabel = new Label() { Text = $"{ctot:HH:mmZ}", TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, Margin = timeMargin };
            

            timesFirstColumnGrid.Children.Add(eobtTextLabel);
            timesFirstColumnGrid.Children.Add(eobtTimeLabel);
            timesFirstColumnGrid.Children.Add(ctotTextLabel);
            timesFirstColumnGrid.Children.Add(ctotTimeLabel);

            timesFirstColumnGrid.SetColumn(eobtTextLabel, 0);
            timesFirstColumnGrid.SetColumn(eobtTimeLabel, 1);
            timesFirstColumnGrid.SetColumn(ctotTextLabel, 2);
            timesFirstColumnGrid.SetColumn(ctotTimeLabel, 3);

            var timesSecondRowGrid = new Grid();
            timesSecondRowGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            timesSecondRowGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            timesSecondRowGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            timesSecondRowGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            var tobtTextLabel = new Label() { Text = $"TOBT:", TextColor = Colors.White, Background = DarkBlue, FontSize = 20, VerticalTextAlignment = TextAlignment.Center };
            var tobtTimeLabel = new Label() { Text = $"{vacdm.Tobt:HH:mmZ}", TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, Margin = timeMargin };
            var tsatTextLabel = new Label() { Text = $"TSAT:", TextColor = Colors.White, Background = DarkBlue, FontSize = 20, VerticalTextAlignment = TextAlignment.Center };
            var tsatBackgroundColor = GetTsatBackgroundColor(vacdm.Tsat, vacdm.Tobt);
            var tsatTimeLabel = new Label() { Text = $"{vacdm.Tsat:HH:mmZ}", BackgroundColor = tsatBackgroundColor, TextColor = Colors.White, FontAttributes = FontAttributes.Bold, FontSize = 20, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.End, Margin = timeMargin };

            timesSecondRowGrid.Children.Add(tobtTextLabel);
            timesSecondRowGrid.Children.Add(tobtTimeLabel);
            timesSecondRowGrid.Children.Add(tsatTextLabel);
            timesSecondRowGrid.Children.Add(tsatTimeLabel);

            timesSecondRowGrid.SetColumn(tobtTextLabel, 0);
            timesSecondRowGrid.SetColumn(tobtTimeLabel, 1);
            timesSecondRowGrid.SetColumn(tsatTextLabel, 2);
            timesSecondRowGrid.SetColumn(tsatTimeLabel, 3);


            var timesThirdColumnGrid = new Grid();
            timesThirdColumnGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            timesThirdColumnGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            timesThirdColumnGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));

            var delayTextLabel = new Label() { Text = $"Delay:", TextColor = Colors.White, Background = DarkBlue, FontSize = 20, VerticalTextAlignment = TextAlignment.Center};
            var delayDataLabel = new Label() { Text = $"{vacdm.DelayMin} min.", TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, Margin = timeMargin };

            timesThirdColumnGrid.Children.Add(delayTextLabel);
            timesThirdColumnGrid.Children.Add(delayDataLabel);

            timesFirstColumnGrid.SetColumn(delayTextLabel, 0);
            timesFirstColumnGrid.SetColumn(delayDataLabel, 1);

            timesThirdColumnGrid.SetColumn(timesThirdColumnGrid, 0);

            timesInfoGrid.Children.Add(timesFirstColumnGrid);
            timesInfoGrid.SetRow(timesFirstColumnGrid, 0);

            timesInfoGrid.Children.Add(timesSecondRowGrid);
            timesInfoGrid.SetRow(timesSecondRowGrid, 1);

            timesInfoGrid.Children.Add(timesThirdColumnGrid);
            timesInfoGrid.SetRow(timesThirdColumnGrid, 2);


            var flightPositionGrid = new Grid() { Padding = 10, Margin = 10, BackgroundColor = DarkBlue };
            flightPositionGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionGrid.RowDefinitions.Add(new RowDefinition(new GridLength(1, GridUnitType.Star)));

            var flightPositionFirstGrid = new Grid();
            flightPositionFirstGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionFirstGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));
            flightPositionFirstGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionFirstGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));

            var gateArea = vacdm.TaxiZone == "default taxitime" ? "N/A" : vacdm.TaxiZone;

            var gateTextLabel = new Label() { Text = $"Gates:", TextColor = Colors.White, Background = DarkBlue,  FontSize = 20, VerticalTextAlignment = TextAlignment.Center };
            var gateDataLabel = new Label() { Text = gateArea, TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, Margin = timeMargin };
            var sidTextLabel = new Label() { Text = $"SID:", TextColor = Colors.White, Background = DarkBlue,  FontSize = 20, VerticalTextAlignment = TextAlignment.Center };
            var sidDataLabel = new Label() { Text = pilot.Clearance.Sid, TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, Margin = timeMargin };

            flightPositionFirstGrid.Children.Add(gateTextLabel);
            flightPositionFirstGrid.Children.Add(gateDataLabel);
            flightPositionFirstGrid.Children.Add(sidTextLabel);
            flightPositionFirstGrid.Children.Add(sidDataLabel);

            flightPositionFirstGrid.SetColumn(gateTextLabel, 0);
            flightPositionFirstGrid.SetColumn(gateDataLabel, 1);
            flightPositionFirstGrid.SetColumn(sidTextLabel, 2);
            flightPositionFirstGrid.SetColumn(sidDataLabel, 3);

            var flightPositionSecondGrid = new Grid();
            flightPositionSecondGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionSecondGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionSecondGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionSecondGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            var rwy = pilot.Clearance.DepRwy;

            if(pilot.FlightPlan.Departure == "EDDF" && pilot.Clearance.DepRwy == "18")
            {
                rwy = "18W";
            }

            var rwyTextLabel = new Label() { Text = $"RWY:", TextColor = Colors.White, Background = DarkBlue, FontSize = 20, VerticalTextAlignment = TextAlignment.Center };
            var rwyDataLabel = new Label() { Text = rwy, TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, Margin = timeMargin };
            var taxitimeTextLabel = new Label() { Text = $"Taxi-Time:", TextColor = Colors.White, Background = DarkBlue, FontSize = 20, VerticalTextAlignment = TextAlignment.Center };
            var taxitimeDataLabel = new Label() { Text = $"{vacdm.exot} min.", TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, Margin = timeMargin };

            flightPositionSecondGrid.Children.Add(rwyTextLabel);
            flightPositionSecondGrid.Children.Add(rwyDataLabel);
            flightPositionSecondGrid.Children.Add(taxitimeDataLabel);
            flightPositionSecondGrid.Children.Add(taxitimeTextLabel);

            flightPositionSecondGrid.SetColumn(rwyTextLabel, 0);
            flightPositionSecondGrid.SetColumn(rwyDataLabel, 1);
            flightPositionSecondGrid.SetColumn(taxitimeTextLabel, 2);
            flightPositionSecondGrid.SetColumn(taxitimeDataLabel, 3);

            var flightPositionThirdGrid = new Grid();
            flightPositionThirdGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionThirdGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));
            flightPositionThirdGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));

            var squawkTextLabel = new Label() { Text = $"Squawk:", TextColor = Colors.White, Background = DarkBlue, FontSize = 20, VerticalTextAlignment = TextAlignment.Center };
            var squawkDataLabel = new Label() { Text = pilot.Clearance.AssignedSquawk, TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 20, HorizontalTextAlignment = TextAlignment.End, VerticalTextAlignment = TextAlignment.Center, Margin = timeMargin };

            flightPositionThirdGrid.Children.Add(squawkTextLabel);
            flightPositionThirdGrid.Children.Add(squawkDataLabel);

            flightPositionThirdGrid.SetColumn(squawkTextLabel, 0);
            flightPositionThirdGrid.SetColumn(squawkDataLabel, 1);

            //TODO?
            //var flightPositionFourthGrid = new Grid();
            //flightPositionThirdGrid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(1, GridUnitType.Star)));

            //var correctSquawkText = pilot.Clearance.AssignedSquawk == pilot.Clearance.CurrentSquawk ? "Correct Squawk Set" : "Wrong Squawk Set";
            //var correctSquawkLabel = new Label() { Text = correctSquawkText, TextColor = Colors.White, Background = DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = 20, VerticalTextAlignment = TextAlignment.Center };

            //flightPositionFourthGrid.Children.Add(correctSquawkLabel);
            //flightPositionFourthGrid.SetColumn(correctSquawkLabel, 0);

            flightPositionGrid.Children.Add(flightPositionFirstGrid);
            flightPositionGrid.SetRow(flightPositionFirstGrid, 0);
            flightPositionGrid.Children.Add(flightPositionSecondGrid);
            flightPositionGrid.SetRow(flightPositionSecondGrid, 1);
            flightPositionGrid.Children.Add(flightPositionThirdGrid);
            flightPositionGrid.SetRow(flightPositionThirdGrid, 2);
            //flightPositionGrid.Children.Add(flightPositionFourthGrid);
            //flightPositionGrid.SetRow(flightPositionFourthGrid, 3);

            var placeholderRectangleOne = new Rectangle() { Background = DarkGrey, HeightRequest = 25 };
            var placeholderRectangleTwo = new Rectangle() { Background = DarkGrey, HeightRequest = 25 };
            var placeholderRectangleThree = new Rectangle() { Background = DarkGrey, HeightRequest = 25 };

            grid.Children.Add(placeholderRectangleOne);
            grid.Children.Add(placeholderRectangleTwo);
            grid.Children.Add(placeholderRectangleThree);

            grid.SetRow(placeholderRectangleOne, 0);
            grid.SetRow(placeholderRectangleTwo, 2);
            grid.SetRow(placeholderRectangleThree, 4);

            grid.Children.Add(flightInfoGrid);
            grid.SetRow(flightInfoGrid, 1);

            grid.Children.Add(timesInfoGrid);
            grid.SetRow(timesInfoGrid, 3);

            grid.Children.Add(flightPositionGrid);
            grid.SetRow(flightPositionGrid, 5);

            return grid;
        }

        private static Color GetTsatBackgroundColor(DateTime tsat, DateTime tobt)
        {
            var now = DateTime.UtcNow;

            //Startup Delay
            if(tsat > tobt.AddMinutes(5))
            {
                return Colors.Red;
            }

            //IN the Window (+/-5)
            if(tsat.AddMinutes(-5) <= now && tsat.AddMinutes(5) >= now)
            {
                return Colors.LightGreen;
            }

            //Outside of the Window
            return Colors.Red;
        }
    }
}
