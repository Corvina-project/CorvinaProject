using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        InitializeComponent();

        var webView = new WebView();

        Content = webView;
        BackgroundColor = Colors.Transparent;

        WebView = webView;
    }
 }