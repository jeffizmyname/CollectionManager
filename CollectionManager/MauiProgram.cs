using CollectionManager.CustomControls;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;

namespace CollectionManager
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitCore()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Objectivity-Black.otf", "ObjectivityBlack");
                    fonts.AddFont("Objectivity-Bold.otf", "ObjectivityBold");
                    fonts.AddFont("Objectivity-ExtraBold.otf", "ObjectivityExtraBold");
                    fonts.AddFont("Objectivity-Light.otf", "ObjectivityLight");
                    fonts.AddFont("Objectivity-Medium.otf", "ObjectivityMedium");
                    fonts.AddFont("Objectivity-Regular.otf", "ObjectivityRegular");
                    fonts.AddFont("Objectivity-Super.otf", "ObjectivitySuper");
                    fonts.AddFont("Objectivity-Thin.otf", "ObjectivityThin");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            //Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("MyCustomization", (handler, view) =>
            //{
            //    if (view is Entry) {
            //        handler.PlatformView.Padd
            //    }
            //});

            return builder.Build();
        }
    }
}
