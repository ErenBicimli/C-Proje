// ============================================================
// Sorumlu Geliştirici: EREN (Eren_AuthUI)
// Dosya: Views/Auth/RegisterView.xaml.cs
// Açıklama: Hesap oluşturma sayfası code-behind.
// ============================================================

using ArizaTakipSistemi.MAUI.Models;
using ArizaTakipSistemi.MAUI.Services;

namespace ArizaTakipSistemi.MAUI.Views.Auth;

public partial class RegisterView : ContentPage
{
    private readonly IArizaApiService _apiService;

    public RegisterView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        RolPicker.SelectedIndex = 0; // Varsayılan: Teknisyen
    }

    private async void KayitButton_Clicked(object sender, EventArgs e)
    {
        var ad = AdEntry.Text?.Trim();
        var soyad = SoyadEntry.Text?.Trim();
        var email = EmailEntry.Text?.Trim();
        var telefon = TelefonEntry.Text?.Trim();
        var sifre = SifreEntry.Text?.Trim();
        var sifreTekrar = SifreTekrarEntry.Text?.Trim();

        // Validasyonlar
        if (string.IsNullOrEmpty(ad) || string.IsNullOrEmpty(soyad))
        {
            GosterHata("Ad ve Soyad alanları zorunludur!");
            return;
        }

        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
        {
            GosterHata("Geçerli bir e-posta adresi girin!");
            return;
        }

        if (string.IsNullOrEmpty(sifre) || sifre.Length < 6)
        {
            GosterHata("Şifre en az 6 karakter olmalıdır!");
            return;
        }

        if (sifre != sifreTekrar)
        {
            GosterHata("Şifreler eşleşmiyor!");
            return;
        }

        if (RolPicker.SelectedIndex < 0)
        {
            GosterHata("Lütfen bir rol seçin!");
            return;
        }

        KayitButton.IsEnabled = false;
        KayitButton.Text = "Kayıt yapılıyor...";

        try
        {
            var yeniKullanici = new KullaniciDto
            {
                Ad = ad,
                Soyad = soyad,
                Email = email,
                Telefon = telefon,
                Rol = RolPicker.SelectedIndex // 0=Teknisyen, 1=Yönetici
            };

            var sonuc = await _apiService.KullaniciKayitAsync(yeniKullanici, sifre);

            if (sonuc != null)
            {
                await DisplayAlert("Başarılı", "Hesabınız oluşturuldu! Şimdi giriş yapabilirsiniz.", "Tamam");
                await Navigation.PopAsync();
            }
            else
            {
                GosterHata("Kayıt başarısız! Lütfen bilgilerinizi kontrol edin.");
            }
        }
        catch (Exception ex)
        {
            GosterHata(ex.Message);
        }
        finally
        {
            KayitButton.IsEnabled = true;
            KayitButton.Text = "HESAP OLUŞTUR";
        }
    }

    private async void GiriseSayfasinaGit_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private void GosterHata(string mesaj)
    {
        HataMesaji.Text = mesaj;
        HataMesaji.IsVisible = true;
    }
}
