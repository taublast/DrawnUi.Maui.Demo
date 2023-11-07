using AppoMobi.Maui.DrawnUi.Infrastructure.Extensions;
using Microsoft.Extensions.Logging;

namespace SomeMauiApp
{
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

            builder.UseDrawnUi<App>(); //gamechanger

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static string RTL = "مرحبًا بكم في عصر العناصر البصرية المصممة\r\nحيث الدقة تلتقي بالإبداع في كل تفصيل\r\nنقدم لكم تجربة مستخدم فريدة ومتطورة\r\nاستمتع بالسلاسة والأداء العالي في التصميمات\r\nنحن نبتكر لنجعل تجربتكم أكثر تميزًا وسهولة";

    }


}