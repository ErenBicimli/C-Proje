// ============================================================
// Sorumlu Geliştirici: EREN (Eren_AuthUI) — Said tarafından güncellendi
// Dosya: AppShell.xaml.cs
// Açıklama: Tüm sayfaları push navigasyon için route olarak kaydeder.
//           Push edilen sayfalarda otomatik geri butonu gözükür.
// ============================================================

using ArizaTakipSistemi.MAUI.Views.Auth;
using ArizaTakipSistemi.MAUI.Views.FaultManagement;

namespace ArizaTakipSistemi.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Tüm sayfalar push navigasyon için route'a kayıtlı.
        // HomeView varsayılan (Shell.xaml içinde), diğerleri buradan açılır.
        Routing.RegisterRoute("ArizaListesi", typeof(ArizaListesiView));
        Routing.RegisterRoute("CihazTuruSecim", typeof(CihazTuruSecimView));
        Routing.RegisterRoute("LogListesi", typeof(LogListesiView));
        Routing.RegisterRoute("CiroHesabi", typeof(CiroHesabiView));
        Routing.RegisterRoute("SifreDegistir", typeof(PasswordChangeView));
        Routing.RegisterRoute("ArizaEkle", typeof(ArizaEkleView));
        Routing.RegisterRoute(nameof(ArizaEkleView), typeof(ArizaEkleView));
        Routing.RegisterRoute(nameof(ArizaGuncelleView), typeof(ArizaGuncelleView));
        Routing.RegisterRoute(nameof(PasswordChangeView), typeof(PasswordChangeView));
    }
}
