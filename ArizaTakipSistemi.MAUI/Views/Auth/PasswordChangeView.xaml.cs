// ============================================================
// Sorumlu Geliştirici: EREN (Eren_AuthUI)
// Dosya: Views/Auth/PasswordChangeView.xaml.cs
// Açıklama: Şifre değiştirme sayfası code-behind.
// ============================================================

using ArizaTakipSistemi.MAUI.Services;

namespace ArizaTakipSistemi.MAUI.Views.Auth;

public partial class PasswordChangeView : ContentPage
{
    private readonly IArizaApiService _apiService;

    public PasswordChangeView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void KaydetButton_Clicked(object sender, EventArgs e)
    {
        var eski = EskiSifreEntry.Text?.Trim();
        var yeni = YeniSifreEntry.Text?.Trim();
        var yeniTekrar = YeniSifreTekrarEntry.Text?.Trim();

        if (string.IsNullOrEmpty(eski) || string.IsNullOrEmpty(yeni) || string.IsNullOrEmpty(yeniTekrar))
        {
            SonucMesaji.Text = "Tüm alanları doldurun!";
            SonucMesaji.TextColor = Colors.Red;
            SonucMesaji.IsVisible = true;
            return;
        }

        if (yeni != yeniTekrar)
        {
            SonucMesaji.Text = "Yeni şifreler eşleşmiyor!";
            SonucMesaji.TextColor = Colors.Red;
            SonucMesaji.IsVisible = true;
            return;
        }

        if (yeni.Length < 6)
        {
            SonucMesaji.Text = "Yeni şifre en az 6 karakter olmalıdır!";
            SonucMesaji.TextColor = Colors.Red;
            SonucMesaji.IsVisible = true;
            return;
        }

        KaydetButton.IsEnabled = false;
        var kullaniciId = Preferences.Get("KullaniciId", 0);
        var sonuc = await _apiService.SifreDegistirAsync(kullaniciId, eski, yeni);

        if (sonuc)
        {
            SonucMesaji.Text = "Şifre başarıyla değiştirildi!";
            SonucMesaji.TextColor = Colors.LightGreen;
            EskiSifreEntry.Text = "";
            YeniSifreEntry.Text = "";
            YeniSifreTekrarEntry.Text = "";
        }
        else
        {
            SonucMesaji.Text = "Mevcut şifre hatalı veya işlem başarısız!";
            SonucMesaji.TextColor = Colors.Red;
        }

        SonucMesaji.IsVisible = true;
        KaydetButton.IsEnabled = true;
    }
}
