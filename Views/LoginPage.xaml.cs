using MauiAuth0App.Auth0;
using System.Text.Json;

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
                TokenHolder.Timer = Application.Current.Dispatcher.CreateTimer();
                TokenHolder.Timer.Interval = TimeSpan.FromMilliseconds(1700*1000);
                TokenHolder.Timer.Tick += (s, e) => {
                    MainThread.InvokeOnMainThreadAsync(async () => {
                        await RefreshAuth();
                    });
                };
                TokenHolder.Timer.Start();

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

    private async Task RefreshAuth() {
        List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>> {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", TokenHolder.RefreshToken),
            new KeyValuePair<string, string>("client_id", "nextel-mobile-app")
        };
        var content = new FormUrlEncodedContent(postData);
        var response = await client.PostAsync("https://auth.corvina.io/auth/realms/exor/protocol/openid-connect/token", content);

        Token result = await JsonSerializer.DeserializeAsync<Token>(await response.Content.ReadAsStreamAsync());
        TokenHolder.AccessToken = result.AccessToken;
        TokenHolder.RefreshToken = result.RefreshToken;
        if (TokenHolder.ResourceId != null)
            await TokenHandler.GetPermissionToken(client);
    }

}

