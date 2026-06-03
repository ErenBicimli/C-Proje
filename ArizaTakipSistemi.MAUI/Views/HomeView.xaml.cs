// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Views/HomeView.xaml.cs
// Açıklama: Ana sayfa. Kartlar push navigasyon ile diğer sayfalara
//           yönlendirir. Push olduğu için diğer sayfalarda otomatik
//           geri butonu (←) çıkar.
// ============================================================

using ArizaTakipSistemi.MAUI.Views.Auth;

namespace ArizaTakipSistemi.MAUI.Views;

public partial class HomeView : ContentPage
{
    public HomeView()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        int rol = Preferences.Get("KullaniciRol", 0);
        RolLabel.Text = rol == 1 ? "YÖNETİCİ" : "TEKNİKER";
        AdLabel.Text = Preferences.Get("KullaniciAd", "Kullanıcı");
    }

    // Arıza Listesi kartına basınca önce Cihaz Türü seçim sayfası açılır.
    private async void ArizaListesi_Tapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("CihazTuruSecim");

    private async void IslemGecmisi_Tapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("LogListesi");

    private async void SifreDegistir_Tapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("SifreDegistir");

    private async void CiroHesabi_Tapped(object sender, TappedEventArgs e)
        => await Shell.Current.GoToAsync("CiroHesabi");

    // Çıkış: oturumu temizle ve Login'e dön (Shell'i değiştir).
    private async void CikisYap_Tapped(object sender, TappedEventArgs e)
    {
        bool onay = await DisplayAlert("Çıkış", "Çıkış yapmak istediğinize emin misiniz?", "Evet", "Hayır");
        if (!onay) return;

        Preferences.Remove("KullaniciId");
        Preferences.Remove("KullaniciAd");
        Preferences.Remove("KullaniciRol");
        Preferences.Set("GirisYapildi", false);

        Application.Current!.MainPage = new NavigationPage(
            App.Current!.Handler!.MauiContext!.Services.GetRequiredService<LoginView>());
    }
}
