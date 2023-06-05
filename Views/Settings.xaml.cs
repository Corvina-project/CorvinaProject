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
            case "Ukrainian 🇺🇦":
                cultureInfo = "uk-UA";
                break;
            default:
                cultureInfo = "en-Us";
                break;
        }
 
        var switchToCulture = new CultureInfo(cultureInfo);
        LocalizationResourceManager.Instance.SetCulture(switchToCulture);
        Preferences.Default.Set("language", switchToCulture.Name);

        //var switchToCulture = Language.Culture.TwoLetterISOLanguageName
        //    .Equals("it", StringComparison.InvariantCultureIgnoreCase) ?
        //    new CultureInfo("en-US") : new CultureInfo("it");
        //CambiaLinguaButton.Text = LocalizationResourceManager.Instance["TextButton"].ToString();
        //SemanticScreenReader.Announce(CambiaLinguaButton.Text);
    }

    protected override void OnAppearing()
    {
        string s = Preferences.Default.Get("language", "it-IT");
        switch (s)
        {
            case "it-IT":
                PickerLanguage.SelectedItem = "Italiano 🇮🇹";
                PickerLanguage.SelectedIndex = 0;
                break;
            case "en-Us":
                PickerLanguage.SelectedItem = "English 🇬🇧 🇺🇸";
                break;
            case "es-ES":
                PickerLanguage.SelectedItem = "Spanish 🇪🇸";
                break;
            case "fr-FR":
                PickerLanguage.SelectedItem = "French 🇫🇷";
                break;
            case "pt-PT":
                PickerLanguage.SelectedItem = "Portuguese 🇵🇹";
                break;
            case "de-DE":
                PickerLanguage.SelectedItem = "Deutsch 🇩🇪";
                break;
            case "ar":
                PickerLanguage.SelectedItem = "Arabic 🇦🇪";
                break;
            case "zh-CN":
                PickerLanguage.SelectedItem = "Chinese 🇨🇳";
                break;
            case "hi":
                PickerLanguage.SelectedItem = "Hindi 🇮🇳";
                break;
            case "ru-RU":
                PickerLanguage.SelectedItem = "Russian 🇷🇺";
                break;
            case "uk-UA":
                PickerLanguage.SelectedItem = "Ukrainian 🇺🇦";
                break;
            default:
                PickerLanguage.SelectedItem = "English 🇬🇧 🇺🇸";
                break;
        }
    }
}