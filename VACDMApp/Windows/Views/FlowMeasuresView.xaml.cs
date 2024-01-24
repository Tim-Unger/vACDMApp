using VACDMApp.Data.Renderer;

namespace VACDMApp.Windows.Views;

public partial class FlowMeasuresView : ContentView
{
    public FlowMeasuresView()
    {
        InitializeComponent();
    }

    private bool _isFirstLoad = true;

    private void ContentView_Loaded(object sender, EventArgs e)
    {
        if (_isFirstLoad)
        {
            var measures = FlowMeasures.Render();

            measures.ForEach(FlowMeasuresStackLayout.Children.Add);

            _isFirstLoad = false;
        }

        _isFirstLoad = false;
    }

    private async void RefreshView_Refreshing(object sender, EventArgs e)
    {
        FlowMeasuresRefreshView.IsRefreshing = true;

        //TODO implement
        await Task.Delay(1000);

        FlowMeasuresRefreshView.IsRefreshing = false;
    }
}
