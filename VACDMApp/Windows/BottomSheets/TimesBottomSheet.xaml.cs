using Microsoft.Maui.Controls.Shapes;
using The49.Maui.BottomSheet;
using static VacdmApp.Data.Data;

namespace VacdmApp.Windows.BottomSheets;

public partial class TimesBottomSheet : BottomSheet
{
    private static readonly double _width = Shell.Current.CurrentPage.Width;

    public TimesBottomSheet()
    {
        InitializeComponent();
        GetTimes();

        Sender = this;

        Data.Data.SenderPage = Data.SenderPage.Time;
    }

    public static int SelectedTime = 0;

    private void GetTimes()
    {
        var pilotsWithFP = VacdmPilots
            .Where(x => VatsimPilots.Exists(y => y.callsign == x.Callsign)) //Only Pilots that have are in the Vatsim Datafeed
            .Where(x => VatsimPilots.First(y => y.callsign == x.Callsign).flight_plan != null); //Only Pilots that have filed a flight plan

        var possibleTimes = pilotsWithFP
            .Select(x => x.Vacdm.Eobt) //Only get the EOBT
            .DistinctBy(x => x.Hour) //Only get each value once
            .Select(x => x.Hour) //Only get the hour
            .Order() //Order by time
            .Select(x => x.ToString()) //We can't .Cast<string> the Collection, so we have to run ToString() on each item
            .ToList();

        var handleBar = new RoundRectangle()
        {
            CornerRadius = 10,
            Background = Colors.White,
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center,
            WidthRequest = 50,
            HeightRequest = 10
        };

        TimesStackLayout.Children.Add(handleBar);

        var titleLabel = new Label()
        {
            Text = "Please choose a Time Window",
            Padding = 20,
            TextColor = Colors.White,
            Background = Colors.Transparent,
            FontSize = 20,
            FontAttributes = FontAttributes.None,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        TimesStackLayout.Children.Add(titleLabel);

        var timeLabels = new List<Grid> { GetTimeGrid("ALL TIMES") };

        timeLabels.AddRange(possibleTimes.Select(GetTimeGrid).ToList());

        timeLabels.ForEach(TimesStackLayout.Children.Add);
    }

    private void BottomSheet_Focused(object sender, FocusEventArgs e) { }

    private Grid GetTimeGrid(string time)
    {
        var grid = new Grid() { Padding = 20 };
        var timeButton = new Button()
        {
            BackgroundColor = Colors.Transparent,
            WidthRequest = _width
        };

        var timeText = "ALL TIMES";

        if (int.TryParse(time, out var timeVal))
        {
            timeText = $"{timeVal}:00Z";

            if (timeVal < 10)
            {
                timeText = $"0{timeVal}:00Z";
            }
        }

        var timeLabel = new Label()
        {
            Text = timeText,
            TextColor = Colors.White,
            Background = Colors.Transparent,
            FontSize = 15,
            FontAttributes = FontAttributes.None,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Center
        };

        grid.Children.Add(timeLabel);
        grid.Children.Add(timeButton);

        timeButton.Clicked += TimeButton_Clicked;

        return grid;
    }

    private async void TimeButton_Clicked(object sender, EventArgs e)
    {
        var selectedAirport = (Button)sender;
        var selectedParent = (Grid)selectedAirport.Parent;

        var loadingIndicator = new ActivityIndicator()
        {
            Color = Colors.White,
            IsRunning = true,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Center,
            Background = Colors.Transparent,
            Scale = 0.75
        };

        selectedParent.Children.Add(loadingIndicator);
        
        var childLabel = selectedParent.Children[0];
        var childText = ((Label)childLabel).Text;

        if (childText == "ALL TIMES")
        {
            FlightsView.SetTimeText(childText);
            await DismissAsync();
            return;
        }

        //Whether the time is greater 10 or smaller 10;
        var timeValue = childText.Length == 5 ? childText[..1] : childText[..2];

        FlightsView.SetTimeText(timeValue);
        await DismissAsync();
    }
}
