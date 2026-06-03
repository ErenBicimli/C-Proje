// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Views/FaultManagement/ArizaEkleView.xaml.cs
// Açıklama: Yeni arıza ekleme sayfası code-behind.
// Güncelleme: Aynı formdan yeni cihaz oluşturma desteği eklendi.
//             Anahtara göre ya yeni cihaz açılır + arıza eklenir,
//             ya da var olan bir cihaz seçilip arıza eklenir.
// ============================================================

using ArizaTakipSistemi.MAUI.Helpers;
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

        // Var olan cihazları yükle (anahtar kapatılırsa kullanılacak).
        _cihazlar = await _apiService.TumCihazlariGetirAsync();
        CihazPicker.ItemsSource = _cihazlar.Select(c => c.CihazBilgisi).ToList();

        // Teknisyenleri yükle.
        _teknisyenler = await _apiService.TumKullanicilariGetirAsync();
        TeknisyenPicker.ItemsSource = _teknisyenler.Select(k => k.TamAd).ToList();

        OncelikPicker.SelectedIndex = 1; // Normal
        KabulTarihiPicker.Date = DateTime.Today;
        KabulSaatiPicker.Time = DateTime.Now.TimeOfDay;
    }

    // Mod anahtarı: hangi paneli göstereceğimizi belirler.
    private void YeniCihazSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        YeniCihazPanel.IsVisible = e.Value;
        VarOlanCihazPanel.IsVisible = !e.Value;
    }

    // Marka değişince Model Picker'ı o markaya ait modellerle doldur.
    private void MarkaPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var marka = MarkaPicker.SelectedItem?.ToString();
        ModelPicker.ItemsSource = MarkaModelData.ModelleriGetir(marka);
        ModelPicker.SelectedIndex = -1;
        ModelPicker.Title = "Model Seçin";
    }

    private async void KaydetButton_Clicked(object sender, EventArgs e)
    {
        // ===== ARIZA ALANLARI ORTAK VALIDASYONU =====
        var tanim = ArizaTanimiEditor.Text?.Trim();
        if (string.IsNullOrEmpty(tanim) || tanim.Length < 10)
        {
            HataGoster("Arıza tanımı en az 10 karakter olmalıdır!");
            return;
        }
        if (KategoriPicker.SelectedIndex < 0)
        {
            HataGoster("Lütfen bir kategori seçin!");
            return;
        }

        KaydetButton.IsEnabled = false;

        // ===== CİHAZ ID'Yİ BUL =====
        int cihazId;

        if (YeniCihazSwitch.IsToggled)
        {
            // YENİ CİHAZ MODU: önce cihaz oluştur, ID'sini al, sonra arıza için kullan.
            var yeniCihaz = await YeniCihazOlustur();
            if (yeniCihaz == null) { KaydetButton.IsEnabled = true; return; }
            cihazId = yeniCihaz.CihazId;
        }
        else
        {
            // VAR OLAN CİHAZ MODU: listeden seçilen cihazı kullan.
            if (CihazPicker.SelectedIndex < 0)
            {
                HataGoster("Lütfen var olan bir cihaz seçin (veya anahtarı açıp yeni cihaz oluşturun).");
                KaydetButton.IsEnabled = true;
                return;
            }
            cihazId = _cihazlar[CihazPicker.SelectedIndex].CihazId;
        }

        // ===== ARIZA KAYDINI OLUŞTUR =====
        decimal? maliyet = null;
        if (!string.IsNullOrEmpty(MaliyetEntry.Text) && decimal.TryParse(MaliyetEntry.Text, out var parsedMaliyet))
            maliyet = parsedMaliyet;

        var ariza = new ArizaDto
        {
            CihazId = cihazId,
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
            HataGoster("Arıza kaydı oluşturulamadı! Lütfen tekrar deneyin.");
        }

        KaydetButton.IsEnabled = true;
    }

    // Yeni cihaz oluşturma (yardımcı): form alanlarını doğrular,
    // seri no çakışmasını kontrol eder, API'ye gönderir.
    private async Task<CihazDto?> YeniCihazOlustur()
    {
        var marka = MarkaPicker.SelectedItem?.ToString();
        var model = ModelPicker.SelectedItem?.ToString();
        var seriNo = SeriNoEntry.Text?.Trim();
        var musteriAdi = MusteriAdiEntry.Text?.Trim();
        var musteriTel = MusteriTelefonEntry.Text?.Trim();

        if (string.IsNullOrEmpty(marka)) { HataGoster("Lütfen marka seçin!"); return null; }
        if (string.IsNullOrEmpty(model)) { HataGoster("Lütfen model seçin!"); return null; }
        if (string.IsNullOrEmpty(seriNo)) { HataGoster("Lütfen seri numarası girin!"); return null; }
        if (CihazTuruPicker.SelectedIndex < 0) { HataGoster("Lütfen cihaz türü seçin!"); return null; }
        if (string.IsNullOrEmpty(musteriAdi)) { HataGoster("Lütfen müşteri adı girin!"); return null; }
        if (string.IsNullOrEmpty(musteriTel)) { HataGoster("Lütfen müşteri telefon girin!"); return null; }

        // Seri numarası çakışma kontrolü (LINQ ile).
        bool seriNoVarMi = _cihazlar.Any(c =>
            string.Equals(c.SeriNumarasi?.Trim(), seriNo, StringComparison.OrdinalIgnoreCase));
        if (seriNoVarMi)
        {
            HataGoster("Bu seri numarası zaten kayıtlı! Farklı bir seri numarası girin.");
            return null;
        }

        var kabulTarihi = KabulTarihiPicker.Date.Date + KabulSaatiPicker.Time;

        var cihaz = new CihazDto
        {
            Marka = marka,
            Model = model,
            SeriNumarasi = seriNo,
            CihazTuru = CihazTuruPicker.SelectedItem?.ToString() ?? "Diğer",
            MusteriAdi = musteriAdi,
            MusteriTelefon = musteriTel,
            KabulTarihi = kabulTarihi
        };

        var olusturulan = await _apiService.CihazEkleAsync(cihaz);
        if (olusturulan == null)
        {
            HataGoster("Cihaz kaydı oluşturulamadı!");
            return null;
        }
        return olusturulan;
    }

    private void HataGoster(string mesaj)
    {
        SonucMesaji.Text = mesaj;
        SonucMesaji.TextColor = Colors.Orange;
        SonucMesaji.IsVisible = true;
    }
}
