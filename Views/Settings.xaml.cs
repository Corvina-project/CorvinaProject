using MauiAuth0App.Resources.Languages;
using System.Globalization;

namespace MauiAuth0App.Views;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
	}
    private async void OpenVillaGreppi(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://www.istitutogreppi.edu.it/");
    }
    private async void OpenCorvinaCloud(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://app.corvina.io/#/");
    }
    private async void OpenGitHub(object sender, EventArgs e)
    {
        await Launcher.OpenAsync("https://github.com/Corvina-project/MauiAuth0App");
    }

    private void CambiaLinguaClicked(object sender, EventArgs e)
    {
        var switchToCulture = Language.Culture.TwoLetterISOLanguageName
            .Equals("it", StringComparison.InvariantCultureIgnoreCase) ?
            new CultureInfo("en-US") : new CultureInfo("it-IT");
        LocalizationResourceManager.Instance.SetCulture(switchToCulture);

        CambiaLinguaButton.Text = LocalizationResourceManager.Instance["TextButton"].ToString();
        SemanticScreenReader.Announce(CambiaLinguaButton.Text);
    }
}