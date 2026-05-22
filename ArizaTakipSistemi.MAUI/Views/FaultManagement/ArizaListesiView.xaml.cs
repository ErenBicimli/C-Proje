// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Views/FaultManagement/ArizaListesiView.xaml.cs
// Açıklama: Arıza listesi sayfasının mantık (code-behind) kısmı.
// Güncelleme: Özet kartları + Aylık Kazanç Hedefi dairesel göstergesi +
//             arama/kategori/tarih LINQ filtreleri.
// ============================================================

using ArizaTakipSistemi.MAUI.Models;
using ArizaTakipSistemi.MAUI.Services;
using Microsoft.Maui.Graphics;

namespace ArizaTakipSistemi.MAUI.Views.FaultManagement;

public partial class ArizaListesiView : ContentPage
{
    private readonly IArizaApiService _apiService;

    // Tüm arızalar (filtrelenmez); kart sayıları ve kazanç bundan hesaplanır.
    private List<ArizaDto> _tumArizalar = new();

    // Ekranda temel alınan liste; arama/kategori/tarih bunun üzerinde LINQ ile çalışır.
    private List<ArizaDto> _kaynakListe = new();

    // Dairesel kazanç göstergesinin çizimini yapan nesne.
    private readonly KazancGaugeDrawable _gaugeDrawable = new();

    public ArizaListesiView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;

        // Dairesel göstergeye çizim nesnesini bağla.
        KazancGauge.Drawable = _gaugeDrawable;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Başlıkta giriş yapılan ROL (büyük) ve ismi göster.
        int rol = Preferences.Get("KullaniciRol", 0);          // 0=Teknisyen, 1=Yönetici
        RolLabel.Text = rol == 1 ? "YÖNETİCİ" : "TEKNİKER";
        AdLabel.Text = Preferences.Get("KullaniciAd", "Kullanıcı");

        BaslangicTarihi.Date = DateTime.Today.AddMonths(-1);
        BitisTarihi.Date = DateTime.Today;

        // Daha önce kaydedilmiş hedef varsa kutuya yaz.
        var kayitliHedef = Preferences.Get("AylikKazancHedefi", 0.0);
        if (kayitliHedef > 0)
            HedefEntry.Text = kayitliHedef.ToString("0");

        await ArizalariYukle();
    }

    private async Task ArizalariYukle()
    {
        _tumArizalar = await _apiService.TumArizalariGetirAsync();
        _kaynakListe = _tumArizalar;
        SayilariGuncelle();
        KazancHesapla();   // tamamlanan arızalara göre kazanç göstergesini güncelle
        FiltreleriUygula();
    }

    // ===== Özet kart sayıları =====
    private void SayilariGuncelle()
    {
        ArizaliSayiLabel.Text = _tumArizalar.Count(a => a.Durum == 0 || a.Durum == 1).ToString();
        TeslimSayiLabel.Text = _tumArizalar.Count(a => a.Durum == 2).ToString();
    }

    // ===== AYLIK KAZANÇ HESABI =====
    // MevcutKazanc = Tamamlanan (Durum==2) arızaların Tahmini Maliyet toplamı.
    // Yüzde = (MevcutKazanc / HedefUcret) * 100
    private void KazancHesapla()
    {
        double hedef = Preferences.Get("AylikKazancHedefi", 0.0);
        double mevcut = _tumArizalar
            .Where(a => a.Durum == 2)
            .Sum(a => (double)(a.TahminiMaliyet ?? 0));

        double yuzde = hedef > 0 ? (mevcut / hedef) * 100.0 : 0;

        YuzdeLabel.Text = $"%{yuzde:0}";
        KazancDurumLabel.Text = $"{mevcut:N0} ₺ / {hedef:N0} ₺";

        // Göstergeyi güncelle ve yeniden çizdir.
        _gaugeDrawable.Yuzde = yuzde;
        KazancGauge.Invalidate();
    }

    // Hedef tutarı kaydet butonu.
    private void HedefKaydet_Clicked(object sender, EventArgs e)
    {
        if (double.TryParse(HedefEntry.Text, out double hedef) && hedef > 0)
        {
            Preferences.Set("AylikKazancHedefi", hedef);
            KazancHesapla();
        }
        else
        {
            DisplayAlert("Uyarı", "Lütfen geçerli bir hedef tutar girin.", "Tamam");
        }
    }

    // ===== Özet kart tıklamaları =====
    private void ArizaliKart_Tapped(object sender, TappedEventArgs e)
    {
        _kaynakListe = _tumArizalar.Where(a => a.Durum == 0 || a.Durum == 1).ToList();
        FiltreleriUygula();
    }

    private void TeslimKart_Tapped(object sender, TappedEventArgs e)
    {
        _kaynakListe = _tumArizalar.Where(a => a.Durum == 2).ToList();
        FiltreleriUygula();
    }

    // ===== Filtreler (kategori -> tarih -> arama), hepsi LINQ =====
    private void FiltreleriUygula()
    {
        IEnumerable<ArizaDto> sonuc = _kaynakListe;

        if (KategoriFiltrePicker.SelectedIndex > 0)
        {
            var secilenKategori = KategoriFiltrePicker.SelectedItem?.ToString();
            sonuc = sonuc.Where(a => a.Kategori == secilenKategori);
        }

        if (TarihFiltresiSwitch.IsToggled)
        {
            var bas = BaslangicTarihi.Date.Date;
            var bit = BitisTarihi.Date.Date.AddDays(1).AddTicks(-1);
            sonuc = sonuc.Where(a => a.OlusturulmaTarihi >= bas && a.OlusturulmaTarihi <= bit);
        }

        var arama = AramaCubugu?.Text?.Trim();
        if (!string.IsNullOrWhiteSpace(arama))
        {
            sonuc = sonuc.Where(a =>
                (a.ArizaTanimi?.Contains(arama, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (a.Kategori?.Contains(arama, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (a.Cihaz != null && a.Cihaz.CihazBilgisi != null &&
                 a.Cihaz.CihazBilgisi.Contains(arama, StringComparison.OrdinalIgnoreCase)));
        }

        ArizaCollectionView.ItemsSource = sonuc.ToList();
    }

    private void AramaCubugu_TextChanged(object sender, TextChangedEventArgs e) => FiltreleriUygula();
    private void KategoriFiltre_Changed(object sender, EventArgs e) => FiltreleriUygula();
    private void TarihFiltresi_Toggled(object sender, ToggledEventArgs e) => FiltreleriUygula();
    private void TarihSecildi(object sender, DateChangedEventArgs e) => FiltreleriUygula();

    private async void FiltreTemizle_Clicked(object sender, EventArgs e)
    {
        AramaCubugu.Text = string.Empty;
        KategoriFiltrePicker.SelectedIndex = 0;
        TarihFiltresiSwitch.IsToggled = false;
        await ArizalariYukle();
    }

    private async void FiltreTumu_Clicked(object sender, EventArgs e) => await ArizalariYukle();

    private async void FiltreBeklemede_Clicked(object sender, EventArgs e)
    {
        _kaynakListe = await _apiService.DurumaGoreFiltrelemeAsync(0);
        FiltreleriUygula();
    }

    private async void FiltreDevamEdiyor_Clicked(object sender, EventArgs e)
    {
        _kaynakListe = await _apiService.DurumaGoreFiltrelemeAsync(1);
        FiltreleriUygula();
    }

    private async void FiltreTamamlandi_Clicked(object sender, EventArgs e)
    {
        _kaynakListe = await _apiService.DurumaGoreFiltrelemeAsync(2);
        FiltreleriUygula();
    }

    private async void YeniAriza_Clicked(object sender, EventArgs e)
        => await Shell.Current.GoToAsync(nameof(ArizaEkleView));

    private async void ArizaDetay_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is int arizaId)
            await Shell.Current.GoToAsync($"{nameof(ArizaGuncelleView)}?ArizaId={arizaId}");
    }
}

// ============================================================
// DAİRESEL KAZANÇ GÖSTERGESİ ÇİZİMİ
// Arka planda gri bir halka, üstünde yüzde kadar dolan renkli bir yay çizer.
// Renk: %40 altı KIRMIZI, %40-80 MAVİ, %80 üzeri YEŞİL.
// ============================================================
public class KazancGaugeDrawable : IDrawable
{
    public double Yuzde { get; set; } = 0; // 0..100 (üzeri 100 kabul edilir)

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        float kalinlik = 16f;
        float pad = kalinlik / 2f + 6f;
        float x = dirtyRect.Left + pad;
        float y = dirtyRect.Top + pad;
        float cap = Math.Min(dirtyRect.Width, dirtyRect.Height) - pad * 2f;

        // Arka halka (gri tam çember)
        canvas.StrokeSize = kalinlik;
        canvas.StrokeColor = Color.FromArgb("#2a2a4a");
        canvas.DrawEllipse(x, y, cap, cap);

        // Dolan yay
        double pct = Math.Max(0, Math.Min(Yuzde, 100));
        if (pct > 0)
        {
            Color renk = pct < 40 ? Color.FromArgb("#e94560")   // kırmızı
                       : pct < 85 ? Color.FromArgb("#3498db")   // mavi
                                  : Color.FromArgb("#27ae60");  // yeşil (%85 ve üzeri)

            canvas.StrokeColor = renk;
            canvas.StrokeLineCap = LineCap.Round;

            if (pct >= 100)
            {
                // %100: tam yeşil halka çiz.
                canvas.DrawEllipse(x, y, cap, cap);
            }
            else
            {
                float sweep = (float)(360.0 * pct / 100.0);
                // Tepeden (90°) başla, saat yönünde ilerle.
                canvas.DrawArc(x, y, cap, cap, 90, 90 - sweep, true, false);
            }
        }
    }
}
