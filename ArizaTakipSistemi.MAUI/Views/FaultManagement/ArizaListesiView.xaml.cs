// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Views/FaultManagement/ArizaListesiView.xaml.cs
// Açıklama: Arıza listesi sayfasının mantık kısmı.
//   - CihazTuru parametresi (CihazTuruSecimView'dan geliyor) ile başlangıçta filtrelenir.
//   - "Tümü" gelirse tüm cihaz türleri listelenir.
//   - Öncelik durumuna göre sıralanır (Acil önce, Düşük sona).
//   - Yeni "🔴 Arızalı" butonu Durum 0 (Beklemede) veya 1 (Devam Ediyor) olanları gösterir.
// ============================================================

using ArizaTakipSistemi.MAUI.Models;
using ArizaTakipSistemi.MAUI.Services;

namespace ArizaTakipSistemi.MAUI.Views.FaultManagement;

[QueryProperty(nameof(CihazTuru), "CihazTuru")]
public partial class ArizaListesiView : ContentPage
{
    private readonly IArizaApiService _apiService;
    private List<ArizaDto> _tumArizalar = new();
    private List<ArizaDto> _kaynakListe = new();

    // CihazTuruSecimView'dan gelen parametre. Boş ise filtre uygulanmaz.
    public string CihazTuru { get; set; } = string.Empty;

    public ArizaListesiView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        int rol = Preferences.Get("KullaniciRol", 0);
        RolLabel.Text = rol == 1 ? "YÖNETİCİ" : "TEKNİKER";
        AdLabel.Text = Preferences.Get("KullaniciAd", "Kullanıcı");

        // Cihaz türüne göre sayfa başlığı
        Title = string.IsNullOrEmpty(CihazTuru) ? "Tüm Arızalar" : $"{CihazTuru} Arızaları";

        BaslangicTarihi.Date = DateTime.Today.AddMonths(-1);
        BitisTarihi.Date = DateTime.Today;

        await ArizalariYukle();
    }

    private async Task ArizalariYukle()
    {
        var hepsi = await _apiService.TumArizalariGetirAsync();

        // Cihaz Türü filtresi (CihazTuruSecimView'dan gelen parametre)
        _tumArizalar = string.IsNullOrEmpty(CihazTuru)
            ? hepsi
            : hepsi.Where(a => a.Cihaz != null
                   && string.Equals(a.Cihaz.CihazTuru, CihazTuru, StringComparison.OrdinalIgnoreCase)).ToList();

        _kaynakListe = _tumArizalar;
        SayilariGuncelle();
        FiltreleriUygula();
    }

    private void SayilariGuncelle()
    {
        ArizaliSayiLabel.Text = _tumArizalar.Count(a => a.Durum == 0 || a.Durum == 1).ToString();
        TeslimSayiLabel.Text = _tumArizalar.Count(a => a.Durum == 2).ToString();
    }

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

    // Tüm filtreleri uygula + ÖNCELİK DURUMUNA GÖRE SIRALA.
    // OncelikDurumu: 0=Düşük, 1=Normal, 2=Yüksek, 3=Acil — büyükten küçüğe sırala.
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

        // Öncelik durumuna göre azalan sıralama; aynı önceliklilerde tarih azalan.
        var siralanmis = sonuc
            .OrderByDescending(a => a.OncelikDurumu)
            .ThenByDescending(a => a.OlusturulmaTarihi)
            .ToList();

        ArizaCollectionView.ItemsSource = siralanmis;
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

    // YENİ: Arızalı (Durum 0 veya 1) — yani henüz tamamlanmamış olanlar
    private void FiltreArizali_Clicked(object sender, EventArgs e)
    {
        _kaynakListe = _tumArizalar.Where(a => a.Durum == 0 || a.Durum == 1).ToList();
        FiltreleriUygula();
    }

    private void FiltreBeklemede_Clicked(object sender, EventArgs e)
    {
        _kaynakListe = _tumArizalar.Where(a => a.Durum == 0).ToList();
        FiltreleriUygula();
    }

    private void FiltreDevamEdiyor_Clicked(object sender, EventArgs e)
    {
        _kaynakListe = _tumArizalar.Where(a => a.Durum == 1).ToList();
        FiltreleriUygula();
    }

    private void FiltreTamamlandi_Clicked(object sender, EventArgs e)
    {
        _kaynakListe = _tumArizalar.Where(a => a.Durum == 2).ToList();
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
