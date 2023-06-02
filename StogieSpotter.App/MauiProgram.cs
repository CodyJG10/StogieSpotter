using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using StogieSpotter.App.Services;
using StogieSpotter.App.ViewModels;
using StogieSpotter.App.Views;
using StogieSpotter.PlacesApi;
using Syncfusion.Maui.Core.Hosting;

namespace StogieSpotter.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
		builder.Logging.AddDebug();
#endif
            // Replace this with API key
            builder.Services.AddSingleton(new GooglePlacesService(""));
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<Home>();

            builder.Services.AddTransient<SearchForCityViewModel>();
            builder.Services.AddTransient<SearchForCity>();

            builder.Services.AddSingleton<IMauiInitializeService, MyServiceLocator>();


            return builder.Build();
        }
    }
}