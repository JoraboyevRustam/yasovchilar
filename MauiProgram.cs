using Microsoft.Extensions.Logging;
using SozTopar.ViewModels;
using SozTopar.Views;
using SozTopar.Services;

namespace SozTopar;

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
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddLogging(logging =>
		{
			logging.AddDebug();
		});
#endif

		// Services
		builder.Services.AddSingleton<IGameService, GameService>();
		builder.Services.AddSingleton<IEmojiCodeService, EmojiCodeService>();

		// ViewModels
		builder.Services.AddTransient<MainPageViewModel>();
		builder.Services.AddTransient<GamePageViewModel>();

		// Views
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<GamePage>();

		return builder.Build();
	}
}