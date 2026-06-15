// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Views/FaultManagement/ArizaGuncelleView.xaml.cs
// Açıklama: Arıza güncelleme/detay sayfası code-behind.
// ============================================================

using ArizaTakipSistemi.MAUI.Helpers;
using ArizaTakipSistemi.MAUI.Models;
using ArizaTakipSistemi.MAUI.Services;

namespace ArizaTakipSistemi.MAUI.Views.FaultManagement;

[QueryProperty(nameof(ArizaId), "ArizaId")]
public partial class ArizaGuncelleView : ContentPage
{
    private readonly IArizaApiService _apiService;
    private ArizaDto? _ariza;

    public int ArizaId { get; set; }

    public ArizaGuncelleView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await ArizaYukle();
    }

    private async Task ArizaYukle()
    {
        _ariza = await _apiService.ArizaGetirAsync(ArizaId);
        if (_ariza == null)
        {
            await DisplayAlert("Hata", "Arıza bulunamadı!", "Tamam");
            await Shell.Current.GoToAsync("..");
            return;
        }

        // Cihaz bilgisi
        CihazBilgiLabel.Text = _ariza.Cihaz != null
            ? $"{_ariza.Cihaz.Marka} {_ariza.Cihaz.Model} (SN: {_ariza.Cihaz.SeriNumarasi})"
            : "Bilinmiyor";
        MusteriBilgiLabel.Text = _ariza.Cihaz != null
            ? $"Müşteri: {_ariza.Cihaz.MusteriAdi} - {_ariza.Cihaz.MusteriTelefon}"
            : "";

        // Form alanları
        ArizaTanimiEditor.Text = _ariza.ArizaTanimi;
        DurumPicker.SelectedIndex = _ariza.Durum;
        OncelikPicker.SelectedIndex = _ariza.OncelikDurumu;
        YapilanIslemEditor.Text = _ariza.YapilanIslem;
        MaliyetEntry.Text = _ariza.TahminiMaliyet?.ToString("F2");
        HarcananMasrafEntry.Text = _ariza.HarcananMasraf?.ToString("F2");

        // Tarihler
        OlusturulmaTarihiLabel.Text = $"📅 Oluşturulma: {_ariza.OlusturulmaTarihi:dd.MM.yyyy HH:mm}";
        GuncellemeTarihiLabel.Text = _ariza.GuncellemeTarihi.HasValue
            ? $"🔄 Güncelleme: {_ariza.GuncellemeTarihi:dd.MM.yyyy HH:mm}" : "";
        TamamlanmaTarihiLabel.Text = _ariza.TamamlanmaTarihi.HasValue
            ? $"✅ Tamamlanma: {_ariza.TamamlanmaTarihi:dd.MM.yyyy HH:mm}" : "";
    }

    private async void GuncelleButton_Clicked(object sender, EventArgs e)
    {
        if (_ariza == null) return;

        GuncelleButton.IsEnabled = false;

        decimal? harcananMasraf = null;
        if (!string.IsNullOrEmpty(HarcananMasrafEntry.Text) && decimal.TryParse(HarcananMasrafEntry.Text, out var parsedMasraf))
            harcananMasraf = parsedMasraf;

        decimal? maliyet = null;
        if (!string.IsNullOrEmpty(MaliyetEntry.Text) && decimal.TryParse(MaliyetEntry.Text, out var parsedMaliyet))
            maliyet = parsedMaliyet;

        _ariza.ArizaTanimi = ArizaTanimiEditor.Text ?? "";
        int yeniDurum = DurumPicker.SelectedIndex;
        _ariza.Durum = yeniDurum;
        _ariza.OncelikDurumu = OncelikPicker.SelectedIndex;
        _ariza.YapilanIslem = YapilanIslemEditor.Text;
        _ariza.TahminiMaliyet = maliyet;
        _ariza.HarcananMasraf = harcananMasraf;

        var sonuc = await _apiService.ArizaGuncelleAsync(_ariza.ArizaId, _ariza);

        if (sonuc != null)
        {
            await DisplayAlert("Başarılı", "Arıza kaydı güncellendi!", "Tamam");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Hata", "Güncelleme başarısız!", "Tamam");
        }

        GuncelleButton.IsEnabled = true;
    }

    private async void SilButton_Clicked(object sender, EventArgs e)
    {
        if (_ariza == null) return;

        var cevap = await DisplayAlert("Onay", "Bu arıza kaydını silmek istediğinize emin misiniz?", "Evet, Sil", "İptal");
        if (!cevap) return;

        var sonuc = await _apiService.ArizaSilAsync(_ariza.ArizaId);
        if (sonuc)
        {
            await DisplayAlert("Başarılı", "Arıza kaydı silindi!", "Tamam");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            await DisplayAlert("Hata", "Silme işlemi başarısız!", "Tamam");
        }
    }
}
