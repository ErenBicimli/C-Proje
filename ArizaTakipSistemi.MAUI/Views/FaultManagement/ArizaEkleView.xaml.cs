// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Views/FaultManagement/ArizaEkleView.xaml.cs
// Açıklama: Yeni arıza ekleme sayfası code-behind.
// ============================================================

using ArizaTakipSistemi.MAUI.Models;
using ArizaTakipSistemi.MAUI.Services;

namespace ArizaTakipSistemi.MAUI.Views.FaultManagement;

public partial class ArizaEkleView : ContentPage
{
    private readonly IArizaApiService _apiService;
    private List<CihazDto> _cihazlar = new();
    private List<KullaniciDto> _teknisyenler = new();

    public ArizaEkleView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Cihazları yükle
        _cihazlar = await _apiService.TumCihazlariGetirAsync();
        CihazPicker.ItemsSource = _cihazlar.Select(c => c.CihazBilgisi).ToList();

        // Teknisyenleri yükle
        _teknisyenler = await _apiService.TumKullanicilariGetirAsync();
        TeknisyenPicker.ItemsSource = _teknisyenler.Select(k => k.TamAd).ToList();

        OncelikPicker.SelectedIndex = 1; // Normal
    }

    private async void KaydetButton_Clicked(object sender, EventArgs e)
    {
        // Validasyon
        if (CihazPicker.SelectedIndex < 0)
        {
            SonucMesaji.Text = "Lütfen bir cihaz seçin!";
            SonucMesaji.TextColor = Colors.Red;
            SonucMesaji.IsVisible = true;
            return;
        }

        var tanim = ArizaTanimiEditor.Text?.Trim();
        if (string.IsNullOrEmpty(tanim) || tanim.Length < 10)
        {
            SonucMesaji.Text = "Arıza tanımı en az 10 karakter olmalıdır!";
            SonucMesaji.TextColor = Colors.Red;
            SonucMesaji.IsVisible = true;
            return;
        }

        if (KategoriPicker.SelectedIndex < 0)
        {
            SonucMesaji.Text = "Lütfen bir kategori seçin!";
            SonucMesaji.TextColor = Colors.Red;
            SonucMesaji.IsVisible = true;
            return;
        }

        KaydetButton.IsEnabled = false;

        decimal? maliyet = null;
        if (!string.IsNullOrEmpty(MaliyetEntry.Text))
            decimal.TryParse(MaliyetEntry.Text, out var m) ;

        if (!string.IsNullOrEmpty(MaliyetEntry.Text) && decimal.TryParse(MaliyetEntry.Text, out var parsedMaliyet))
            maliyet = parsedMaliyet;

        var ariza = new ArizaDto
        {
            CihazId = _cihazlar[CihazPicker.SelectedIndex].CihazId,
            ArizaTanimi = tanim,
            Kategori = KategoriPicker.SelectedItem?.ToString() ?? "Diğer",
            OncelikDurumu = OncelikPicker.SelectedIndex >= 0 ? OncelikPicker.SelectedIndex : 1,
            Durum = 0, // Beklemede
            TahminiMaliyet = maliyet,
            KullaniciId = TeknisyenPicker.SelectedIndex >= 0
                ? _teknisyenler[TeknisyenPicker.SelectedIndex].KullaniciId
                : null
        };

        var sonuc = await _apiService.ArizaEkleAsync(ariza);

        if (sonuc != null)
        {
            await DisplayAlert("Başarılı", "Arıza kaydı başarıyla oluşturuldu!", "Tamam");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            SonucMesaji.Text = "Kayıt oluşturulamadı! Lütfen tekrar deneyin.";
            SonucMesaji.TextColor = Colors.Red;
            SonucMesaji.IsVisible = true;
        }

        KaydetButton.IsEnabled = true;
    }
}
