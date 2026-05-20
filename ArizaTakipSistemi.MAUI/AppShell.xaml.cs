// ============================================================
// Sorumlu Geliştirici: EREN (Eren_AuthUI)
// Dosya: AppShell.xaml.cs
// Açıklama: AppShell code-behind. Route kayıtları ve çıkış işlemi.
// ============================================================

using ArizaTakipSistemi.MAUI.Views.Auth;
using ArizaTakipSistemi.MAUI.Views.FaultManagement;

namespace ArizaTakipSistemi.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Sayfa route kayıtları (Shell navigasyonu için)
        Routing.RegisterRoute(nameof(ArizaEkleView), typeof(ArizaEkleView));
        Routing.RegisterRoute(nameof(ArizaGuncelleView), typeof(ArizaGuncelleView));
        Routing.RegisterRoute(nameof(PasswordChangeView), typeof(PasswordChangeView));
    }

    private void CikisYap_Clicked(object sender, EventArgs e)
    {
        // Oturum bilgilerini temizle
        Preferences.Remove("KullaniciId");
        Preferences.Remove("KullaniciAd");
        Preferences.Remove("KullaniciRol");
        Preferences.Set("GirisYapildi", false);

        // Login sayfasına dön
        Application.Current!.MainPage = new NavigationPage(
            App.Current!.Handler!.MauiContext!.Services.GetRequiredService<LoginView>());
    }
}
