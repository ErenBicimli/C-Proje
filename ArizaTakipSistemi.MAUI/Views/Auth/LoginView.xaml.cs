// ============================================================
// Sorumlu Geliştirici: EREN (Eren_AuthUI)
// Dosya: Views/Auth/LoginView.xaml.cs
// Açıklama: Giriş sayfası code-behind.
// ============================================================

using ArizaTakipSistemi.MAUI.Services;

namespace ArizaTakipSistemi.MAUI.Views.Auth;

public partial class LoginView : ContentPage
{
    private const string GIRIS_YAPILDI_KEY = "GirisYapildi";
    private readonly IArizaApiService _apiService;

    public LoginView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    private async void GirisButton_Clicked(object sender, EventArgs e)
    {
        var email = EmailEntry.Text?.Trim();
        var sifre = SifreEntry.Text?.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sifre))
        {
            HataMesaji.Text = "E-posta ve şifre alanları boş bırakılamaz!";
            HataMesaji.IsVisible = true;
            return;
        }

        GirisButton.IsEnabled = false;
        GirisButton.Text = "Giriş yapılıyor...";

        try
        {
            var kullanici = await _apiService.GirisYapAsync(email, sifre);

            if (kullanici != null)
            {
                // Oturum bilgisini sakla
                Preferences.Set("KullaniciId", kullanici.KullaniciId);
                Preferences.Set("KullaniciAd", kullanici.TamAd);
                Preferences.Set("KullaniciRol", kullanici.Rol);
                Preferences.Set(GIRIS_YAPILDI_KEY, true);

                await DisplayAlert("Başarılı", "Giriş Başarılı", "Tamam");

                // Ana sayfaya yönlendir
                Application.Current!.MainPage = new AppShell();
            }
            else
            {
                HataMesaji.Text = "E-posta veya şifre hatalı!";
                HataMesaji.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            HataMesaji.Text = $"Bağlantı hatası: {ex.Message}";
            HataMesaji.IsVisible = true;
        }
        finally
        {
            GirisButton.IsEnabled = true;
            GirisButton.Text = "GİRİŞ YAP";
        }
    }

    private async void HesapOlustur_Clicked(object sender, EventArgs e)
    {
        var registerPage = Handler!.MauiContext!.Services.GetRequiredService<RegisterView>();
        await Navigation.PushAsync(registerPage);
    }
}
