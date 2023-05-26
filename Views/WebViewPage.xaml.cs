namespace MauiAuth0App.Views;

public partial class WebViewPage : ContentPage
{
    public WebView WebView { get; set; }
    
    public WebViewPage(string url)
    {
        InitializeComponent();

        var webView = new WebView();
        webView.Source = url;

        Content = webView;
        BackgroundColor = Colors.Transparent;

        WebView = webView;
    }

    public WebViewPage()
    {
        NavigationPage.SetHasNavigationBar(this,false);
        NavigationPage.SetHasBackButton(this, false);
        InitializeComponent();

        var webView = new WebView();

        Content = webView;
        BackgroundColor = Colors.Transparent;
        WebView = webView;
    }

    protected override bool OnBackButtonPressed() {
        return false;
    }
}