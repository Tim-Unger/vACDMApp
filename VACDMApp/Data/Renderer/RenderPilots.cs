using CommunityToolkit.Maui.Converters;
using Microsoft.Maui.Controls.Shapes;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using VACDMApp.Windows.BottomSheets;
using VACDMApp.Windows.Views;

namespace VACDMApp.VACDMData.Renderer
{
    internal class FlightInfos : MainPage
    {
        private static readonly Color DarkBlue = new(28, 40, 54);

        private static readonly GridLength OneStar = new(3, GridUnitType.Star);

        internal static List<Border> Render(string? airport)
        {
            var pilotsWithFP = Data.VACDMPilots
                //Only Pilots that have are in the Vatsim Datafeed
                .Where(x => Data.VatsimPilots.Exists(y => y.callsign == x.Callsign))
                //Only Pilots that have filed a flight plan
                .Where(
                    x => Data.VatsimPilots.First(y => y.callsign == x.Callsign).flight_plan != null
                )
                //Only pilots whose Eobt and TSAT lie within the future or max 5 minutes ago
                .Where(
                    x =>
                        x.Vacdm.Eobt.Hour >= DateTime.UtcNow.AddHours(-1).Hour
                        && x.Vacdm.Tsat >= DateTime.UtcNow.AddMinutes(-6)
                );

            if (pilotsWithFP.Count() == 0)
            {
                return new(1) { RenderNoFlightsFound() };
            }

            if (airport is not null)
            {
                var pilotsFromAirport = pilotsWithFP.Where(x => x.FlightPlan.Departure == airport);
                return SplitAndRenderGrid(pilotsFromAirport);
            }

            return SplitAndRenderGrid(pilotsWithFP);
        }

        private static List<Border> SplitAndRenderGrid(IEnumerable<VACDMPilot> pilots)
        {
            var sortByTime = pilots.OrderBy(x => x.Vacdm.Eobt).GroupBy(x => x.Vacdm.Eobt.Hour);

            var splitGrid = new List<Border>();

            foreach (var hourWindow in sortByTime)
            {
                splitGrid.Add(RenderTimeSeperator(hourWindow.Key));

                splitGrid.AddRange(hourWindow.Select(RenderPilot));
            }

            return splitGrid;
        }

        private static Border RenderTimeSeperator(int hour)
        {
            var border = new Border() { Stroke = Colors.Transparent, BackgroundColor = Colors.Transparent };
            var grid = new Grid();
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(10, GridUnitType.Star)));
            grid.ColumnDefinitions.Add(new ColumnDefinition(new GridLength(2, GridUnitType.Star)));

            var timeLabel = new Label()
            {
                //TODO Local Time as well
                Text = $"{hour}:00Z",
                Padding = new Thickness(10, 0, 0, 0),
                TextColor = Colors.White,
                Background = Colors.Black,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = 17
            };

            //var now = DateTime.Now;

            //var dateLabel = new Label()
            //{
            //    Text = now.ToShortDateString(),
            //    Padding = new Thickness(0, 0, 10, 0),
            //    TextColor = Colors.White,
            //    Background = Colors.Black,
            //    FontAttributes = FontAttributes.Bold,
            //    VerticalOptions = LayoutOptions.Center,
            //    HorizontalOptions = LayoutOptions.Center,
            //    FontSize = 17
            //};

            grid.Children.Add(timeLabel);
            //grid.Children.Add(dateLabel);


            grid.SetColumn(timeLabel, 0);
            //grid.SetColumn(dateLabel, 2);

            border.Content = grid;

            return border;
        }

        private static Border RenderPilot(VACDMPilot pilot)
        {
            //TODO Error handling here when ACDM FP not found
            var flightPlan =
                Data.VatsimPilots.FirstOrDefault(x => x.callsign == pilot.Callsign).flight_plan
                ?? throw new Exception();
            var airlines = Data.Airlines;

            var border = new Border() { StrokeThickness = 0, Stroke = Color.FromArgb("#454545")};
            var parentGridContainer = new Grid() { Background = DarkBlue};
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

        private static Border RenderNoFlightsFound()
        {
            var border = new Border() { Background = Colors.Transparent, StrokeThickness = 0};
            var grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition(new GridLength(5, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(7, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(OneStar));
            grid.RowDefinitions.Add(new RowDefinition(new GridLength(7, GridUnitType.Star)));
            grid.RowDefinitions.Add(new RowDefinition(OneStar));

            var noFlightsImage = new Image()
            {
                Source = "noflights.svg",
                HeightRequest = 100,
                WidthRequest = 100
            };

            var noFlightsLabel = new Label()
            {
                LineBreakMode = LineBreakMode.WordWrap,
                Text = "No vACDM Flights found\nCheck back later or refresh to try again",
                TextColor = Colors.White,
                Background = Colors.Black,
                Margin = new Thickness(0, 40),
                HeightRequest = 150,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Bold,
                FontSize = 30
            };

            grid.Children.Add(noFlightsLabel);
            grid.Children.Add(noFlightsImage);

            grid.SetRow(noFlightsImage, 1);
            grid.SetRow(noFlightsLabel, 4);

            border.Content = grid;
            return border;
        }

        private static void BookmarkButton_Clicked(object sender, EventArgs e)
        {
            var button = (ImageButton)sender;
            var parentGrid = (Grid)((Grid)button.Parent).Parent;
            var callsignGrid = (Grid)parentGrid.Children[1];
            var callsignLabel = (Label)callsignGrid.Children[1];
            var callsign = callsignLabel.Text;

            var pilot = Data.VACDMPilots.First(x => x.Callsign == callsign);

            if (Data.BookmarkedPilots.Contains(pilot))
            {
                button.Source = "bookmark_outline.svg";
                Data.BookmarkedPilots.Remove(pilot);
                return;
            }

            button.Source = "bookmark.svg";
            Data.BookmarkedPilots.Add(pilot);
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
            var vatsimPilot = Data.VatsimPilots.First(x => x.callsign == pilot.Callsign);
            if (vatsimPilot.groundspeed > 50)
            {
                return "Departed";
            }

            var vacdm = pilot.Vacdm;

            if (vacdm.Asrt.Year == 1969)
            {
                return "Preflight/Boarding";
            }

            if (vacdm.Pbg.Year == 1969)
            {
                return "Startup given";
            }

            if (vacdm.Txg.Year == 1969)
            {
                return "Offblock";
            }

            return "Taxi Out";
        }
    }
}
