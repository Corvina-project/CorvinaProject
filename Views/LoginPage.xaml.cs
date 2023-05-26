using MauiAuth0App.Auth0;
using System.Text.Json;
using IdentityModel.OidcClient;
using MauiAuth0App.Extensions;
using Microsoft.Extensions.Hosting;

namespace MauiAuth0App.Views;

public partial class LoginPage : ContentPage {

    private readonly Auth0Client auth0Client;
    private readonly HttpClient client;
    
    private WebViewPage webViewPage;
    private bool isBusy = false;

    private IServices service;

    public LoginPage(Auth0Client auth0Client, HttpClient client, IServices service) {
        InitializeComponent();
        this.auth0Client = auth0Client;
        this.client = client;
        //auth0Client.Browser = new WebViewBrowserAuthenticator(WebViewInstance);

        this.service = service;
        
        webViewPage = new WebViewPage();
        auth0Client.Browser = new WebViewBrowserAuthenticator(webViewPage.WebView);
    }

    private async void OnExitClicked(object sender, EventArgs e)
    {
        Application.Current.Quit();
    }

    private async void OnLoginClicked(object sender, EventArgs e) {
        if (isBusy) return;
        isBusy = true;
        try
        {
            LoginResult loginResult = new LoginResult();
            if (TokenHolder.AccessToken == null)
            {
                await Navigation.PushAsync(webViewPage);
                isBusy = false;
                loginResult = await auth0Client.LoginAsync();//TODO: Secondo me qui c'è un grandissimo memory leak
                await Navigation.PopAsync();

                if (loginResult.IsError)
                {
                    await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
                    return;
                }

                TokenHolder.AccessToken = loginResult.AccessToken;
                TokenHolder.RefreshToken = loginResult.RefreshToken;

                service.Start();
                if (TokenHolder.Timer == null) service.StartTokenHandler(client);

                    // if (!TokenHolder.Timer.IsRunning)
                    //TokenHolder.Timer.Start();
            }
            await Navigation.PushAsync(new OrganizationsPage(client, service));    
        } catch (Exception ex) {
            await DisplayAlert("Errore interno", ex.Source + ": " + ex.Message, "OK");
        } finally {
            //LoginBtn.IsVisible = true;
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

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Credits());
    }
}

