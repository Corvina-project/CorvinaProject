using Microsoft.Extensions.Logging;
using MauiAuth0App.Auth0;
using MauiAuth0App.Views;

namespace MauiAuth0App;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("DESIGNER.otf", "corvina_font");
			});

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

        return builder.Build();
	}
}
