using MauiAuth0App.Auth0;
using System.Text.Json;
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
        try {
            await Navigation.PushAsync(webViewPage);
            isBusy = false;
            var loginResult = await auth0Client.LoginAsync();//TODO: Secondo me qui c'è un grandissimo memory leak
            await Navigation.PopAsync();

            service.Start();

            if (!loginResult.IsError) {
                TokenHolder.AccessToken = loginResult.AccessToken;
                TokenHolder.RefreshToken = loginResult.RefreshToken;
                
                if (TokenHolder.Timer == null)
                {
                    service.StartTokenHandler(client);
                }

                // if (!TokenHolder.Timer.IsRunning)
                //     TokenHolder.Timer.Start();

                await Navigation.PushAsync(new OrganizationsPage(client));
                
            } else {
                await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
            }
        } catch (Exception ex) {
            await DisplayAlert("Errore interno", ex.Source + ": " + ex.Message, "OK");
        } finally {
            //LoginBtn.IsVisible = true;
        }
    }

}

