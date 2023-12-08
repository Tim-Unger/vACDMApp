using CommunityToolkit.Maui.Alerts;
//using FluentScheduler;
using VACDMApp.VACDMData;
using VACDMApp.Data.Renderer;
using VACDMApp.Windows.BottomSheets;
using static VACDMApp.VACDMData.Data;
using Plugin.LocalNotification;
using Java.Nio.Channels;
using VACDMApp.Data.PushNotifications;

namespace VACDMApp.Windows.Views;

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
            FontAttributes = FontAttributes.Bold,
            Text = "All Airports",
            TextColor = _white
        };

    private static readonly Button _dayButton =
        new()
        {
            Margin = new Thickness(10, 5, 10, 5),
            Background = _grey,
            FontAttributes = FontAttributes.Bold,
            Text = "Today",
            TextColor = _white
        };

    private static readonly Button _timeFormatButton =
        new()
        {
            Margin = new Thickness(10, 5, 10, 5),
            Background = _grey,
            FontAttributes = FontAttributes.Bold,
            Text = "Today",
            TextColor = _white
        };

    private static readonly Button _flightsTimePicker =
        new()
        {
            Margin = new Thickness(10, 5, 10, 5),
            Background = _grey,
            FontAttributes = FontAttributes.Bold,
            IsVisible = true,
            TextColor = _white
        };

    private bool _isFirstLoad = true;

    private async void ContentView_Loaded(object sender, EventArgs e)
    {
        if (_isFirstLoad)
        {
            Routing.RegisterRoute("AboutPage", typeof(AboutPage));

            var flights = Pilots.Render(null);
            flights.ForEach(FlightsStackLayout.Children.Add);
            GetNearestTime();

            ButtonsStackLayout.Children.Add(_airportsButton);
            _airportsButton.Clicked += AirportsButton_Clicked;
            ButtonsStackLayout.Children.Add(_dayButton);
            _dayButton.Clicked += DayButton_Clicked;
            ButtonsStackLayout.Children.Add(_flightsTimePicker);
            _flightsTimePicker.Clicked += TimeButton_Clicked;
            ButtonsStackLayout.Children.Add(_timeFormatButton);
            _timeFormatButton.Clicked += TimeFormatButton_Clicked;

            //GetCurrentTime();
            _isFirstLoad = false;
            await UpdateDataContinuously();
        }

        _isFirstLoad = false;

        //FlightsSearchBar.Text = Properties.Settings.Default.Cid;
    }

    private void GetNearestTime()
    {
        var now = DateTime.UtcNow;

        _flightsTimePicker.Text = $"{now.Hour}:00Z";
    }

    //private async Task GetCurrentTime()
    //{
    //    while (true)
    //    {
    //        var now = DateTime.UtcNow;

    //        var hasColon = now.Second % 2 == 0;

    //        var time = $"{now.Hour}{(hasColon ? ":" : " ")}{now.Minute}Z";

    //        TimeLabel.Text = time;

    //        await Task.Delay(200);
    //    }
    //}

    private async Task UpdateDataContinuously()
    {
        //TODO Pause on lost focus
        while (true)
        {
            await MainPage.GetAllData();

            var selectedAirport = AirportsBottomSheet.GetClickedAirport();

            if (string.IsNullOrEmpty(selectedAirport) || selectedAirport == "ALL AIRPORTS")
            {
                selectedAirport = null;
            }

            FlightsStackLayout.Children.Clear();
            var allFlights = Pilots.Render(selectedAirport);
            allFlights.ForEach(FlightsStackLayout.Children.Add);
            FlightsScrollView.Content = FlightsStackLayout;

            await PushNotificationHandler.CheckTimeWindowAndPushMessage(BookmarkedPilots);

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }

    private async void AirportsButton_Clicked(object sender, EventArgs e)
    {
        var airportsSheet = new AirportsBottomSheet();

        await airportsSheet.ShowAsync();
    }

    private void DayButton_Clicked(object sender, EventArgs e)
    {
        //FlightsDatePicker.IsVisible = true;
        //FlightsDatePicker.
    }

    private void TimeButton_Clicked(object sender, EventArgs e)
    {
        //TODO BottomSheet
        var possibleTimes = VACDMPilots
            .Where(x => VatsimPilots.Exists(y => y.callsign == x.Callsign)) //Only Pilots that are connected to Vatsim
            .Where(x => VatsimPilots.First(y => y.callsign == x.Callsign).flight_plan != null) //Only Pilots that have a flight plan
            .Where(x => x.Vacdm.Eobt.Hour >= DateTime.UtcNow.AddHours(-1).Hour) //Only Pilots whose EOBT is earliest 1 hour in the past (removes weird filed EOBTs)
            .Select(x => x.Vacdm.Eobt) //Only get the EOBT
            .DistinctBy(x => x.Hour) //Only get each value once
            .Select(x => x.Hour) //Only get the hour
            .Order()//Order by time
            .ToList();
    }

    private async void TimeFormatButton_Clicked(object sender, EventArgs e) { throw new NotImplementedException(); }

    internal void GetFlightsFromSelectedAirport()
    {
        var selectedAirport = AirportsBottomSheet.GetClickedAirport();

        if (selectedAirport == "ALL AIRPORTS")
        {
            var allFlights = Pilots.Render(null);
            FlightsStackLayout.Children.Clear();
            allFlights.ForEach(FlightsStackLayout.Children.Add);
            FlightsScrollView.Content = FlightsStackLayout;

            return;
        }

        var flights = Pilots.Render(selectedAirport);
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

        FlightsStackLayout.Children.Clear();

        await RefreshDataAndView();

        FlightsRefreshView.IsRefreshing = false;
    }

    private async Task RefreshDataAndView()
    {
        await MainPage.GetAllData();

        var selectedAirport = AirportsBottomSheet.GetClickedAirport();

        if (string.IsNullOrEmpty(selectedAirport) || selectedAirport == "ALL AIRPORTS")
        {
            selectedAirport = null;
        }

        var allFlights = Pilots.Render(selectedAirport);
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

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return;
        }

        if (searchText.Length <= 4)
        {
            //Check if search is an airport
            var depAirports = VACDMPilots
                .Select(x => x.FlightPlan.Departure)
                .DistinctBy(x => x.ToUpper())
                .ToList();

            var arrAirports = VACDMPilots
                .Select(x => x.FlightPlan.Arrival)
                .DistinctBy(x => x.ToUpper())
                .ToList();

            var airports = depAirports.Concat(arrAirports).Distinct();

            if (airports.Any(searchText.StartsWith)) 
            {
                var children = FlightsStackLayout.Children;

                var searchedChildren = new List<Border>();

                foreach(var child in children)
                {
                    var castChild = (Border)child;
                    var borderGrid = (Grid)castChild.Content;

                    //TODO not working

                    var callsignGrid = (Grid)borderGrid.Children[2];
                    var callsignLabel = (Label)callsignGrid.Children[1];
                    var callsign = callsignLabel.Text;
                }

                FlightsScrollView.Content = FlightsStackLayout;
                return;
            }
        }
    }
}
