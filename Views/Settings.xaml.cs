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
        string Cinfo = "it";
        switch (PickerLanguage.SelectedItem.ToString())
        {
            case "Italiano":
                Cinfo = "it";
                break;
            case "Inglese":
                Cinfo = "en-Us";
                break;
        }
        //var switchToCulture = Language.Culture.TwoLetterISOLanguageName
        //    .Equals("it", StringComparison.InvariantCultureIgnoreCase) ?
        //    new CultureInfo("en-US") : new CultureInfo("it");
        var switchToCulture = new CultureInfo(Cinfo);
        LocalizationResourceManager.Instance.SetCulture(switchToCulture);

        Preferences.Default.Set("language", switchToCulture.TwoLetterISOLanguageName);

        //CambiaLinguaButton.Text = LocalizationResourceManager.Instance["TextButton"].ToString();
        //SemanticScreenReader.Announce(CambiaLinguaButton.Text);
    }
}