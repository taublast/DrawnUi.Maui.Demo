global using DrawnUi.Maui.Draw;
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
                })

                .UseDrawnUi(new()
                {
                    DesktopWindow = new() //optional
                    {
                        Width = 500,
                        Height = 800,
                    }
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        public static string RTL = "مرحبًا بكم في عصر العناصر البصرية المصممة\r\nحيث الدقة تلتقي بالإبداع في كل تفصيل\r\nنقدم لكم تجربة مستخدم فريدة ومتطورة\r\nاستمتع بالسلاسة والأداء العالي في التصميمات\r\nنحن نبتكر لنجعل تجربتكم أكثر تميزًا وسهولة";
        public static string Multiline = "This is a single label with a multiline text. The label that follows this one will have Spans defined.\r\nAnd a new line comes in. We can adjust space between paragraphs and characters. This text is aligned to Fill Words.\r\nLorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries.";

    }


}