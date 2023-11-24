using VACDMApp.VACDMData.Renderer;
using VACDMApp.Windows.BottomSheets;

namespace VACDMApp.Windows.Views;

public partial class FlightsView : ContentView
{
	public FlightsView()
	{
		InitializeComponent();
	}

    private static readonly Color _grey = Color.FromArgb("#232323");

    private static readonly Color _white = Color.FromArgb("FEFEFE");


    private static readonly Button _airportsButton = new() { Margin = new Thickness(10, 5, 10, 5), Background = _grey, FontAttributes = FontAttributes.Bold, Text = "All Airports", TextColor = _white };
    
    private static readonly Button _dayButton = new() { Margin = new Thickness(10, 5, 10, 5), Background = _grey, FontAttributes = FontAttributes.Bold, Text = "Today", TextColor = _white };

    private static readonly Button _timeFormatButton = new() { Margin = new Thickness(10, 5, 10, 5), Background = _grey, FontAttributes = FontAttributes.Bold, Text = "Today", TextColor = _white };

    private static TimePicker _flightsTimePicker = new() { Margin = new Thickness(10, 5, 10, 5), Background = _grey, FontAttributes = FontAttributes.Bold, IsVisible = true, TextColor = _white };

    private static bool _isFirstLoad = true;

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        if (_isFirstLoad)
        {
            var flights = FlightInfos.Render();
            flights.ForEach(FlightsStackLayout.Children.Add);
            GetNearestTime();

            ButtonsStackLayout.Children.Add(_airportsButton);
            _airportsButton.Clicked += AirportsButton_Clicked;
            ButtonsStackLayout.Children.Add(_dayButton);
            _dayButton.Clicked += DayButton_Clicked;
            ButtonsStackLayout.Children.Add(_flightsTimePicker);
            ButtonsStackLayout.Children.Add(_timeFormatButton);
            _timeFormatButton.Clicked += TimeFormatButton_Clicked;
        }

        _isFirstLoad = false;
    }

    private void GetNearestTime()
    {
        var now = DateTime.UtcNow;

        _flightsTimePicker.Time = new TimeSpan(now.Hour, 0, 0);
    }

    private void AirportsButton_Clicked(object sender, EventArgs e)
    {
        var airportsSheet = new AirportsBottomSheet();

        airportsSheet.ShowAsync();
    }

    private void DayButton_Clicked(object sender, EventArgs e)
    {
        //FlightsDatePicker.IsVisible = true;
        //FlightsDatePicker.
    }

    //TODO
    private void TimeButton_Clicked(object sender, EventArgs e)
    {

    }

    private void TimeFormatButton_Clicked(object sender, EventArgs e)
    {

    }
}