using VACDMApp.Data.Renderer;

namespace VACDMApp.Windows.Views;

public partial class FlowMeasuresView : ContentView
{
    private bool _showActive = true;

    private bool _showNotified = true;

    private bool _showExpired = false;

    private bool _showWithdrawn = false;

    private static readonly SolidColorBrush _green = new(Colors.DarkGreen);

    private static readonly SolidColorBrush _red = new(Colors.DarkRed);

    public FlowMeasuresView()
    {
        InitializeComponent();
    }

    private bool _isFirstLoad = true;

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        if (_isFirstLoad)
        {
            RenderMeasures();
            _isFirstLoad = false;
        }

        _isFirstLoad = false;
    }

    private async void RefreshView_Refreshing(object sender, EventArgs e)
    {
        //FlowMeasuresRefreshView.IsRefreshing = true;

        RenderMeasures();
        await Task.Delay(1000);

        //FlowMeasuresRefreshView.IsRefreshing = false;
    }

    private void ActiveButton_Clicked(object sender, EventArgs e)
    {
        _showActive = !_showActive;

        ActiveButton.Background = _showActive ? _green : _red;

        RenderMeasures();
    }

    private void NotifiedButton_Clicked(object sender, EventArgs e)
    {
        _showNotified = !_showNotified;

        NotifiedButton.Background = _showNotified ? _green : _red;
        
        RenderMeasures();
    }

    private void ExpiredButton_Clicked(object sender, EventArgs e)
    {
        _showExpired = !_showExpired;

        ExpiredButton.Background = _showExpired ? _green : _red;

        RenderMeasures();
    }

    private void WithdrawnButton_Clicked(object sender, EventArgs e)
    {
        _showWithdrawn = !_showWithdrawn;

        WithdrawnButton.Background = _showWithdrawn ? _green : _red;

        RenderMeasures();
    }

    internal void RenderMeasures()
    {
        var measures = FlowMeasures.Render(_showActive, _showNotified, _showExpired, _showWithdrawn);

        FlowMeasuresStackLayout.Children.Clear();

        measures.ForEach(FlowMeasuresStackLayout.Children.Add);

        FlowMeasuresScrollView.Content = FlowMeasuresStackLayout;
    }
}
