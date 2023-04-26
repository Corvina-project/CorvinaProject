using MauiAuth0App.Auth0;

namespace MauiAuth0App.Views;

public partial class LoginPage : ContentPage {

    private readonly Auth0Client auth0Client;
    private readonly HttpClient client;

    public LoginPage(Auth0Client auth0Client, HttpClient client) {
        InitializeComponent();
        this.auth0Client = auth0Client;
        this.client = client;

        auth0Client.Browser = new WebViewBrowserAuthenticator(WebViewInstance);
    }

    private async void OnLoginClicked(object sender, EventArgs e) {
        try {
            LoginBtn.IsVisible = false;
            var loginResult = await auth0Client.LoginAsync();

            if (!loginResult.IsError) {
                TokenHolder.AccessToken = loginResult.AccessToken;
                TokenHolder.RefreshToken = loginResult.RefreshToken;

                await Navigation.PushAsync(new OrganizationsPage(client));
            } else {
                await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
            }
        } catch (Exception ex) {
            await DisplayAlert("Errore interno", ex.Source + ": " + ex.Message, "OK");
        } finally {
            LoginBtn.IsVisible = true;
        }
    }

}

