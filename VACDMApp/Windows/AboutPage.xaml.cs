using VacdmApp.Data;

namespace VacdmApp;

public partial class AboutPage : ContentPage
{
    public AboutPage()
    {
        InitializeComponent();
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        TitleLabel.Text = "VATSIM\nAirport\nCollaborative\nDecision\nMaking";
        Data.Data.SenderPage = SenderPage.About;
        VersionLabel.Text = $"Tim Unger (1468997) -- V {AppInfo.Current.VersionString}";
    }

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", true);
    }

    private async void VacdmGithubButton_Clicked(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://github.com/vACDM");
    }

    private async void PilotguideButton_Clicked(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://vacdm.net/docs/pilot/use-vacdm");
    }

    private async void GithubButton_Clicked(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://github.com/Tim-Unger/vacdm-app");
    }

    private void WaypointleButton_Clicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("WaypointlePage");
    }
}
