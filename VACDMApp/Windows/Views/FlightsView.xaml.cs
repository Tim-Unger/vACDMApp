using VacdmApp.Data;
using VacdmApp.Data.Renderer;
using VacdmApp.Windows.BottomSheets;
using static VacdmApp.Data.Data;
using static VacdmApp.Data.Renderer.Pilots;
using CommunityToolkit.Maui.Core.Platform;

namespace VacdmApp.Windows.Views;

public partial class FlightsView : ContentView
{
    public FlightsView()
    {
        InitializeComponent();
    }

    private static readonly Color _grey = Color.FromArgb("#232323");

    private static readonly Color _white = Color.FromArgb("FEFEFE");

    private static readonly Button _airportsButton =
        new()
        {
            Margin = new Thickness(10, 5, 10, 5),
            Background = _grey,
            FontAttributes = FontAttributes.None,
            Text = "All Airports",
            TextColor = _white
        };

    private static readonly Button _timeFormatButton =
        new()
        {
            Margin = new Thickness(10, 5, 10, 5),
            Background = _grey,
            FontAttributes = FontAttributes.None,
            Text = "UTC",
            TextColor = _white
        };

    private static readonly Button _timeButton =
        new()
        {
            Margin = new Thickness(10, 5, 10, 5),
            Background = _grey,
            FontAttributes = FontAttributes.None,
            IsVisible = true,
            TextColor = _white
        };

    private bool _isFirstLoad = true;

    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        if (_isFirstLoad)
        {
            Routing.RegisterRoute("AboutPage", typeof(AboutPage));

            var flights = Pilots.Render(null, null);
            flights.ForEach(FlightsStackLayout.Children.Add);
            GetNearestTime();
            ScrollToCurrentTime();

            ButtonsStackLayout.Children.Add(_airportsButton);
            _airportsButton.Clicked += async (sender, e) => await AirportsButton_Clicked(sender, e);

            ButtonsStackLayout.Children.Add(_timeButton);
            _timeButton.Clicked += TimeButton_Clicked;

            ButtonsStackLayout.Children.Add(_timeFormatButton);
            _timeFormatButton.Clicked += async (sender, e) =>
                await TimeFormatButton_Clicked(sender, e);

            await UpdateDataContinuously();

            _isFirstLoad = false;
        }

        _isFirstLoad = false;
    }

    private void GetNearestTime()
    {
        var now = DateTime.UtcNow;

        var hourString = now.Hour < 10 ? $"0{now.Hour}" : now.Hour.ToString();

        _timeButton.Text = $"{hourString}:00Z";
    }

    private async void ScrollToCurrentTime()
    {
        var now = DateTime.UtcNow;

        var currentTimeSeparator = TimeValues.FirstOrDefault(x => x.DateTime.Day == now.Day && x.DateTime.Hour == now.Hour);

        //TODO coordinates not working yet
        if(currentTimeSeparator.Border is null)
        {
            return;
        }

        (var x, var y) = (currentTimeSeparator.Border.X, currentTimeSeparator.Border.Y);

        await FlightsScrollView.ScrollToAsync(x, y, true);
    }

    private async Task UpdateDataContinuously()
    {
        //TODO Pause on lost focus
        while (true)
        {
            if (_isFirstLoad)
            {
                _isFirstLoad = false;
                await Task.Delay(TimeSpan.FromSeconds(30));
            }

            var selectedAirport = AirportsBottomSheet.GetClickedAirport();

            if (string.IsNullOrEmpty(selectedAirport) || selectedAirport == "ALL AIRPORTS")
            {
                selectedAirport = null;
            }

            FlightsStackLayout.Children.Clear();
            var allFlights = Pilots.Render(FilterKind.Airport, selectedAirport);
            allFlights.ForEach(FlightsStackLayout.Children.Add);
            FlightsScrollView.Content = FlightsStackLayout;

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }

    private async Task AirportsButton_Clicked(object sender, EventArgs e)
    {
        var airportsSheet = new AirportsBottomSheet();

        await airportsSheet.ShowAsync();

        Data.Data.SenderPage = Data.SenderPage.Airport;
        Data.Data.Sender = airportsSheet;
    }

    private void DayButton_Clicked(object sender, EventArgs e)
    {
        //FlightsDatePicker.IsVisible = true;
        //FlightsDatePicker.
    }

    private void TimeButton_Clicked(object sender, EventArgs e)
    {
        var timesBottomSheet = new TimesBottomSheet();

        timesBottomSheet.ShowAsync();
    }

    private async Task TimeFormatButton_Clicked(object sender, EventArgs e) { }

    internal void GetFlightsFromSelectedAirport()
    {
        var selectedAirport = AirportsBottomSheet.GetClickedAirport();

        if (selectedAirport == "ALL AIRPORTS")
        {
            var allFlights = Pilots.Render(null, null);
            FlightsStackLayout.Children.Clear();
            allFlights.ForEach(FlightsStackLayout.Children.Add);
            FlightsScrollView.Content = FlightsStackLayout;

            return;
        }

        var flights = Pilots.Render(FilterKind.Airport, selectedAirport);
        FlightsStackLayout.Children.Clear();
        flights.ForEach(FlightsStackLayout.Children.Add);
        FlightsScrollView.Content = FlightsStackLayout;
    }

    internal void SetAirportText(string selectedAirport)
    {
        if (string.IsNullOrEmpty(selectedAirport))
        {
            _airportsButton.Text = "All Airports";
            return;
        }

        if (selectedAirport == "ALL AIRPORTS")
        {
            _airportsButton.Text = "All Airports";
            return;
        }

        _airportsButton.Text = $"From {selectedAirport}";
    }

    private async void RefreshView_Refreshing(object sender, EventArgs e)
    {
        FlightsRefreshView.IsRefreshing = true;

        await RefreshDataAndView();

        FlightsRefreshView.IsRefreshing = false;
    }

    internal async Task RefreshDataAndView()
    {
        FlightsStackLayout.Children.Clear();

        var dataTask = GetVatsimData.GetVatsimDataAsync();
        var vacdmTask = VACDMPilotsData.GetVACDMPilotsAsync();

        await Task.WhenAll(dataTask, vacdmTask);

        VatsimPilots = dataTask.Result.pilots.ToList();
        VacdmPilots = vacdmTask.Result;

        var selectedAirport = AirportsBottomSheet.GetClickedAirport();

        if (string.IsNullOrEmpty(selectedAirport) || selectedAirport == "ALL AIRPORTS")
        {
            selectedAirport = null;
        }

        var allFlights = Pilots.Render(FilterKind.Airport, selectedAirport);
        allFlights.ForEach(FlightsStackLayout.Children.Add);

        FlightsScrollView.Content = FlightsStackLayout;
    }

    private async void AboutButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("AboutPage");
    }

    private void FlightsSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchText = ((SearchBar)sender).Text.ToUpperInvariant();

        //Check if search is nothing (User deleted/erased Search Text)
        if (string.IsNullOrWhiteSpace(searchText))
        {
            FlightsStackLayout.Children.Clear();
            var pilots = Pilots.Render(null, null);

            pilots.ForEach(FlightsStackLayout.Children.Add);

            return;
        }

        //Check if search is a CID
        if (int.TryParse(searchText, out var cid))
        {
            FlightsStackLayout.Children.Clear();

            var pilots = Pilots.Render(FilterKind.Cid, cid.ToString());

            pilots.ForEach(FlightsStackLayout.Children.Add);

            return;
        }

        //Check if Search is an Airline ICAO
        if (searchText.Length <= 3)
        {
            var airlineIcao = searchText.ToUpperInvariant();

            var isAirline = Airlines.Any(x => x.icao == airlineIcao);

            if (!isAirline)
            {
                FlightsStackLayout.Children.Clear();
                //TODO nothing found screen
                return;
            }

            FlightsStackLayout.Children.Clear();
            var pilots = Pilots.Render(FilterKind.Airline, airlineIcao);

            pilots.ForEach(FlightsStackLayout.Children.Add);

            return;
        }

        //Check if search is an airport
        if (searchText.Length <= 4)
        {
            var depAirports = VacdmPilots
                .Select(x => x.FlightPlan.Departure)
                .DistinctBy(x => x.ToUpper())
                .ToList();

            var arrAirports = VacdmPilots
                .Select(x => x.FlightPlan.Arrival)
                .DistinctBy(x => x.ToUpper())
                .ToList();

            var airports = depAirports.Concat(arrAirports).Distinct();

            if (airports.Any(searchText.StartsWith))
            {
                FlightsStackLayout.Children.Clear();

                var pilots = Pilots.Render(FilterKind.Airport, searchText.ToUpper());

                pilots.ForEach(FlightsStackLayout.Children.Add);

                FlightsScrollView.Content = FlightsStackLayout;
                return;
            }
        }

        //Check if search is a single callsign
        if (VacdmPilots.Any(x => x.Callsign == searchText.ToUpperInvariant()))
        {
            FlightsStackLayout.Children.Clear();

            var pilots = Pilots.Render(FilterKind.Callsign, searchText.ToUpperInvariant());

            pilots.ForEach(FlightsStackLayout.Children.Add);
        }
    }

    internal void SetTimeText(string selectedTime)
    {
        if (selectedTime == "ALL TIMES")
        {
            GetNearestTime();

            FlightsStackLayout.Children.Clear();

            var allPilots = Pilots.Render(null, null);

            allPilots.ForEach(FlightsStackLayout.Children.Add);
            return;
        }

        var timeVal = int.Parse(selectedTime);
        var timeString = timeVal < 10 ? $"0{timeVal}:00Z" : $"{timeVal}:00Z";
        _timeButton.Text = timeString;

        FlightsStackLayout.Children.Clear();

        var pilots = Pilots.Render(FilterKind.Time, selectedTime.ToString());

        pilots.ForEach(FlightsStackLayout.Children.Add);
    }

    private void SetLoadingScreen(bool isLoading)
    {
        LoadingGrid.IsVisible = isLoading;
    }

    internal async Task HideKeyboardAsync()
    {
        await FlightsSearchBar.HideKeyboardAsync(CancellationToken.None);
    }
}
