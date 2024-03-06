using CommunityToolkit.Maui.Views;
using Java.Lang;
using Javax.Security.Auth;
using VacdmApp.Data;
using VacdmApp.Windows.Views;

namespace VacdmApp.Windows.BottomSheets;

public partial class FirBottomSheet : Popup
{
    public FirBottomSheet()
    {
        InitializeComponent();
    }

    private List<string> _addedFirs = new();

    private int _allFirsStartIndex = 0;

    private enum SortType
    {
        NameAZ,
        NameZA,
        IdentAZ,
        IdentZA
    };

    private void Popup_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        Data.Data.Sender = this;

        Data.Data.SenderPage = Data.SenderPage.FirSettings;

        RenderFirs();
    }

    private void RenderFirs()
    {
        var firs = Data.Data.FlowMeasureFirs;

        var addedFirsRaw = Preferences.Get("flow_measure_push_firs", "");

        if (!string.IsNullOrWhiteSpace(addedFirsRaw))
        {
            _addedFirs = JsonSerializer.Deserialize<List<string>>(addedFirsRaw);

            FirStackLayout.Children.Add(
                new Label()
                {
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Start,
                    Text = "Added"
                }
            );

            var addedFirGrids = _addedFirs
                .Select(x => RenderFir(firs.First(y => y.Identifier == x), isAdded: true))
                .ToList();

            addedFirGrids.ForEach(FirStackLayout.Children.Add);

            FirStackLayout.Children.Add(new Label() { FontSize = 20, Text = "" });

            FirStackLayout.Children.Add(
                new Label()
                {
                    FontSize = 20,
                    FontAttributes = FontAttributes.Bold,
                    HorizontalOptions = LayoutOptions.Start,
                    Text = "All FIRs"
                }
            );
        }

        FirStackLayout.Children.Add(CreateOrderButtonsStackLayout());

        _allFirsStartIndex = FirStackLayout.Children.Count;

        var nonAddedFirs = GetAndOrderFirs(SortType.NameAZ);

        var firGrids = nonAddedFirs.Select(x => RenderFir(x, isAdded: false)).ToList();

        firGrids.ForEach(FirStackLayout.Children.Add);
    }

    private Grid RenderFir(Fir fir, bool isAdded)
    {
        var grid = new Grid();

        var firLabel = new Label()
        {
            Text = $"{fir.Name} ({fir.Identifier})",
            TextColor = Colors.White,
            FontSize = 20,
            Margin = 5,
            HorizontalOptions = LayoutOptions.Start
        };

        var firCheckBox = new CheckBox()
        {
            HorizontalOptions = LayoutOptions.End,
            IsChecked = isAdded
        };

        var firbutton = new Button() { BackgroundColor = Colors.Transparent };

        grid.Children.Add(firLabel);
        grid.Children.Add(firbutton);
        grid.Children.Add(firCheckBox);

        firCheckBox.CheckedChanged += (sender, e) => FirCheckBox_CheckedChanged(sender, e, fir);
        firbutton.Clicked += (sender, e) => Firbutton_Clicked(sender, e, firCheckBox, fir);

        return grid;
    }

    private void Firbutton_Clicked(object sender, EventArgs e, CheckBox firCheckBox, Fir fir)
    {
        firCheckBox.IsChecked = !firCheckBox.IsChecked;

        //CheckedChanged Event fires automatically since we are changing the state
    }

    private void FirCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e, Data.Fir fir)
    {
        if (_addedFirs.Any(x => x == fir.Identifier))
        {
            var removeIndex = _addedFirs.FindIndex(x => x == fir.Identifier);

            _addedFirs.RemoveAt(removeIndex);

            return;
        }

        _addedFirs.Add(fir.Identifier);
    }

    private void SaveButton_Clicked(object sender, EventArgs e)
    {
        var savedFirsString = JsonSerializer.Serialize(_addedFirs);

        Preferences.Set("flow_measure_push_firs", savedFirsString);

        Close();
    }

    private void FirSearchEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entryText = ((Entry)sender).Text;

        var nonAddedFirs = GetAndOrderFirs(SortType.NameAZ);

        if (string.IsNullOrWhiteSpace(entryText))
        {
            FirStackLayout.Children.Add(CreateOrderButtonsStackLayout());

            _allFirsStartIndex = FirStackLayout.Children.Count;

            var firGrids = nonAddedFirs.Select(x => RenderFir(x, isAdded: false)).ToList();

            firGrids.ForEach(FirStackLayout.Children.Add);

            return;
        }

        var count = FirStackLayout.Children.Count;

        for (var i = _allFirsStartIndex - 2; i <= count; i++)
        {
            //we are removing the first item of the list as long as there are still items left
            FirStackLayout.Children.RemoveAt(_allFirsStartIndex - 2);
        }

        FirStackLayout.Children.Add(new Label() { FontSize = 20, Text = "" });

        FirStackLayout.Children.Add(
            new Label()
            {
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Start,
                Text = "Search Results"
            }
        );

        var concernedFirs = nonAddedFirs.Where(
            x => x.Identifier.Contains(entryText, StringComparison.InvariantCultureIgnoreCase) || x.Name.Contains(entryText, StringComparison.InvariantCultureIgnoreCase)
        );

        var ccount = concernedFirs.Count();

        var resultFirGrids = concernedFirs.Select(x => RenderFir(x, isAdded: false)).ToList();

        resultFirGrids.ForEach(FirStackLayout.Children.Add);
    }

    private HorizontalStackLayout CreateOrderButtonsStackLayout()
    {
        var stackLayout = new HorizontalStackLayout();

        var nameAZButton = CreateOrderButton("Name A>Z");
        var nameZAButton = CreateOrderButton("Name Z>A");
        var identAZButton = CreateOrderButton("ICAO A>Z");
        var identZAButton = CreateOrderButton("ICAO Z>A");

        nameAZButton.Background = Colors.White;
        nameAZButton.TextColor = Color.FromArgb("#404040");

        stackLayout.Children.Add(nameAZButton);
        stackLayout.Children.Add(nameZAButton);
        stackLayout.Children.Add(identAZButton);
        stackLayout.Children.Add(identZAButton);

        return stackLayout;
    }

    private Button CreateOrderButton(string text)
    {
        var button = new Button()
        {
            Margin = new Thickness(5, 10, 5, 10),
            FontAttributes = FontAttributes.Bold,
            VerticalOptions = LayoutOptions.Center,
            Background = Color.FromArgb("#404040"),
            TextColor = Colors.White,
            Text = text
        };

        button.Clicked += OrderButton_Clicked;

        return button;
    }

    private void OrderButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        var buttons = ((HorizontalStackLayout)button.Parent).Children.OfType<Button>().ToList();

        buttons.ForEach(
            x =>
            {
                x.Background = Color.FromArgb("#404040");
                x.TextColor = Colors.White;
            }
        );

        button.Background = Colors.White;
        button.TextColor = Color.FromArgb("#404040");

        var sortType = button.Text switch
        {
            "Name A>Z" => SortType.NameAZ,
            "Name Z>A" => SortType.NameZA,
            "ICAO A>Z" => SortType.IdentAZ,
            "ICAO Z>A" => SortType.IdentZA,
        };

        var count = FirStackLayout.Children.Count;

        for (var i = _allFirsStartIndex; i <= count; i++)
        {
            //we are removing the first item of the list as long as there are still items left
            FirStackLayout.Children.RemoveAt(_allFirsStartIndex);
        }

        var nonAddedFirs = GetAndOrderFirs(sortType);

        var firGrids = nonAddedFirs.Select(x => RenderFir(x, false)).ToList();

        firGrids.ForEach(FirStackLayout.Children.Add);
    }

    private IEnumerable<Fir> GetAndOrderFirs(SortType sortType)
    {
        var firs = Data.Data.FlowMeasureFirs.Where(
            x => !_addedFirs.Any(y => y == x.Identifier)
        );

        if (sortType == SortType.NameAZ)
        {
            return firs.OrderBy(x => x.Name);
        }

        if (sortType == SortType.NameZA)
        {
            return firs.OrderByDescending(x => x.Name);
        }

        if (sortType == SortType.IdentAZ)
        {
            return firs.OrderBy(x => x.Identifier);
        }

        if (sortType == SortType.IdentZA)
        {
            return firs.OrderByDescending(x => x.Identifier);
        }

        throw new ArgumentOutOfRangeException(nameof(SortType));
    }
}
