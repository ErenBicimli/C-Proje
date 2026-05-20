// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend) / EREN (Eren_AuthUI)
// Dosya: MauiProgram.cs
// Açıklama: MAUI uygulaması yapılandırması.
//           DI kayıtları, HttpClient ve sayfa registrasyonları.
// ============================================================

using Microsoft.Extensions.Logging;
using ArizaTakipSistemi.MAUI.Services;
using ArizaTakipSistemi.MAUI.Views.Auth;
using ArizaTakipSistemi.MAUI.Views.FaultManagement;

namespace ArizaTakipSistemi.MAUI;

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

        // ===================== HttpClient (API bağlantısı) =====================
        builder.Services.AddHttpClient<IArizaApiService, ArizaApiService>(client =>
        {
            // API'nin çalıştığı adres (dotnet run ile başlatılan)
            client.BaseAddress = new Uri("http://localhost:5020/");
        });

        // ===================== Sayfa Kayıtları (DI) =====================
        // Eren_AuthUI Sayfaları
        builder.Services.AddTransient<LoginView>();
        builder.Services.AddTransient<RegisterView>();
        builder.Services.AddTransient<PasswordChangeView>();

        // Said_CrudUI Sayfaları
        builder.Services.AddTransient<ArizaListesiView>();
        builder.Services.AddTransient<ArizaEkleView>();
        builder.Services.AddTransient<ArizaGuncelleView>();
        builder.Services.AddTransient<CihazEkleView>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
