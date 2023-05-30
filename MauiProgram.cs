using MauiAuth0App.Auth0;
#if ANDROID
using MauiAuth0App.Extensions;
using MauiAuth0App.Platforms.Android;
using Microsoft.Extensions.Hosting;
#endif
using MauiAuth0App.Views;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using MauiAuth0App.Resources.Languages;
using System.Globalization;

namespace MauiAuth0App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseSkiaSharp()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("DESIGNER.otf", "corvina_font");
                fonts.AddFont("Poppins-Regular.ttf", "attributes_font");
			});
#if ANDROID
		builder.Services.AddTransient<IServices, BackGroundService>();
#endif

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<LoginPage>();

        builder.Services.AddSingleton(new Auth0Client(new() {
            Domain = "auth.corvina.io/auth/realms/exor",
            ClientId = "nextel-mobile-app",
            Scope = "openid profile",
            RedirectUri = "https://localhost/callback"
        }));

        builder.Services.AddSingleton<TokenHandler>();
        builder.Services.AddHttpClient("DemoAPI",
                client => client.BaseAddress = new Uri("https://app.corvina.io/svc/")
            ).AddHttpMessageHandler<TokenHandler>();
        builder.Services.AddTransient(
            sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("DemoAPI")
        );

        var culture = new CultureInfo(Preferences.Default.Get("language", "it"));
        LocalizationResourceManager.Instance.SetCulture(culture);

        return builder.Build();
	}
}
