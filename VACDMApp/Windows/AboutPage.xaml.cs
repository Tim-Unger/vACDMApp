using VACDMApp.VACDMData;

namespace VACDMApp;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
	}

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        TitleLabel.Text = "Virtual\nAirport\nCollaborative\nDecision\nMaking";
        VACDMData.Data.SenderPage = SenderPage.About;
        var now = DateTime.UtcNow;
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
        await Browser.Default.OpenAsync("https://knowledgebase.vatsim-germany.org/books/vacdm/page/vacdm-pilot-guide");
    }

    private async void GithubButton_Clicked(object sender, EventArgs e)
    {
        await Browser.Default.OpenAsync("https://github.com/Tim-Unger/vacdm-app");
    }
}