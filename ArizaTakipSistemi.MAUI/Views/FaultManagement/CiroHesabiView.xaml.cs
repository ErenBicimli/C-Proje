// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Views/FaultManagement/CiroHesabiView.xaml.cs
// Açıklama: Gelir (tamamlanan arızaların maliyet toplamı) ve
//           Gider (Tamamlandı olarak işaretlerken sorduğumuz masraf
//           toplamı) dairesel grafikleri. Net Ciro = Gelir - Gider.
// ============================================================

using ArizaTakipSistemi.MAUI.Helpers;
using ArizaTakipSistemi.MAUI.Models;
using ArizaTakipSistemi.MAUI.Services;

namespace ArizaTakipSistemi.MAUI.Views.FaultManagement;

public partial class CiroHesabiView : ContentPage
{
    private readonly IArizaApiService _apiService;
    private readonly KazancGaugeDrawable _gelirDrawable = new() { TersRenk = false };
    private readonly KazancGaugeDrawable _giderDrawable = new() { TersRenk = true };

    public CiroHesabiView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
        GelirGauge.Drawable = _gelirDrawable;
        GiderGauge.Drawable = _giderDrawable;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var hedef = Preferences.Get("AylikKazancHedefi", 0.0);
        if (hedef > 0) HedefEntry.Text = hedef.ToString("0");
        await CiroHesapla();
    }

    private async Task CiroHesapla()
    {
        // GELİR: tamamlanan arızaların Tahmini Maliyet (Alınacak Tutar) toplamı
        var arizalar = await _apiService.TumArizalariGetirAsync();
        double gelir = arizalar
            .Where(a => a.Durum == 2)
            .Sum(a => (double)(a.TahminiMaliyet ?? 0));

        // GİDER: tamamlanan arızaların Harcanan Masraf toplamı
        double gider = arizalar
            .Where(a => a.Durum == 2)
            .Sum(a => (double)(a.HarcananMasraf ?? 0));
            
        double netCiro = gelir - gider;

        // Yüzde hesapları
        double hedef = Preferences.Get("AylikKazancHedefi", 0.0);
        double gelirYuzde = hedef > 0 ? (gelir / hedef) * 100.0 : 0;
        double giderYuzde = gelir > 0 ? (gider / gelir) * 100.0 : 0; // gelire oranla gider

        // Etiketleri güncelle
        GelirYuzdeLabel.Text = $"%{gelirYuzde:0}";
        GiderYuzdeLabel.Text = $"%{giderYuzde:0}";
        GelirTutarLabel.Text = $"{gelir:N0} ₺";
        GiderTutarLabel.Text = $"{gider:N0} ₺";
        NetCiroLabel.Text = $"{netCiro:N0} ₺";
        HedefLabel.Text = $"Hedef: {hedef:N0} ₺";

        // Net ciro pozitif ise yeşil, negatif ise kırmızı renkte göster
        NetCiroLabel.TextColor = netCiro >= 0
            ? Color.FromArgb("#27ae60")
            : Color.FromArgb("#e94560");

        // Gauge'leri yeniden çiz
        _gelirDrawable.Yuzde = gelirYuzde;
        _giderDrawable.Yuzde = giderYuzde;
        GelirGauge.Invalidate();
        GiderGauge.Invalidate();
    }

    private async void HedefGuncelle_Clicked(object sender, EventArgs e)
    {
        if (double.TryParse(HedefEntry.Text, out var hedef) && hedef > 0)
        {
            Preferences.Set("AylikKazancHedefi", hedef);
            await CiroHesapla();
        }
        else
        {
            await DisplayAlert("Uyarı", "Lütfen geçerli bir hedef tutar girin.", "Tamam");
        }
    }
}
