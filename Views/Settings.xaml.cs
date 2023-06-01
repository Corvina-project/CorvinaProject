using MauiAuth0App.Resources.Languages;
using System.Globalization;

namespace MauiAuth0App.Views;

public partial class Settings : ContentPage
{
	public Settings()
	{
		InitializeComponent();
	}

    protected override void OnAppearing()
    {
        string s = null;
        s = Preferences.Default.Get("language", "it-IT");
        switch (s)
        {
            case "it":
            case "it-IT":
                PickerLanguage.SelectedItem = "Italiano 🇮🇹";
                break;
            case "en":
            case "en-Us":
                PickerLanguage.SelectedItem = "English 🇬🇧 🇺🇸";
                break;
            //TODO: french case "French 🇫🇷":
            //    cultureInfo = "fr-FR";
            //    break;
            case "es":
                PickerLanguage.SelectedItem = "Spanish 🇪🇸";
                break;
            case "fr":
                PickerLanguage.SelectedItem = "French 🇫🇷";
                break;
            case "pt":
                PickerLanguage.SelectedItem = "Portuguese 🇵🇹";
                break;
            case "de":
                PickerLanguage.SelectedItem = "Deutsch 🇩🇪";
                break;
            case "ar":
                PickerLanguage.SelectedItem = "Arabic 🇦🇪";
                break;
            case "zh":
                PickerLanguage.SelectedItem = "Chinese 🇨🇳";
                break;
            case "hi":
                PickerLanguage.SelectedItem = "Hindi 🇮🇳";
                break;
            case "ru":
                PickerLanguage.SelectedItem = "Russian 🇷🇺";
                break;
            default:
                PickerLanguage.SelectedItem = "English 🇬🇧 🇺🇸";
                break;
        }
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
        string cultureInfo = "it-IT";
        switch (PickerLanguage.SelectedItem.ToString())
        {
            case "Italian 🇮🇹":
                cultureInfo = "it-IT";
                break;
            case "English 🇬🇧 🇺🇸":
                cultureInfo = "en-Us";
                break;
            case "French 🇫🇷":
                cultureInfo = "fr-FR";
                break;
            case "Spanish 🇪🇸":
                cultureInfo = "es-ES";
                break;
            case "Portuguese 🇵🇹":
                cultureInfo = "pt-PT";
                break;
            case "Deutsch 🇩🇪":
                cultureInfo = "de-DE";
                break;
            case "Arabic 🇦🇪":
                cultureInfo = "ar";
                break;
            case "Chinese 🇨🇳":
                cultureInfo = "zh-CN";
                break;
            case "Hindi 🇮🇳":
                cultureInfo = "hi";
                break;
            case "Russian 🇷🇺":
                cultureInfo = "ru-RU";
                break;
            default:
                cultureInfo = "en-Us";
                break;
        }
 
        var switchToCulture = new CultureInfo(cultureInfo);
        LocalizationResourceManager.Instance.SetCulture(switchToCulture);
        Preferences.Default.Set("language", switchToCulture.TwoLetterISOLanguageName);

        //var switchToCulture = Language.Culture.TwoLetterISOLanguageName
        //    .Equals("it", StringComparison.InvariantCultureIgnoreCase) ?
        //    new CultureInfo("en-US") : new CultureInfo("it");
        //CambiaLinguaButton.Text = LocalizationResourceManager.Instance["TextButton"].ToString();
        //SemanticScreenReader.Announce(CambiaLinguaButton.Text);
    }
}