// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Views/FaultManagement/CihazEkleView.xaml.cs
// Açıklama: Yeni cihaz ekleme sayfası code-behind.
// ============================================================

using ArizaTakipSistemi.MAUI.Helpers;
using ArizaTakipSistemi.MAUI.Models;
using ArizaTakipSistemi.MAUI.Services;

namespace ArizaTakipSistemi.MAUI.Views.FaultManagement;

public partial class CihazEkleView : ContentPage
{
    private readonly IArizaApiService _apiService;

    public CihazEkleView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    // Sayfa açılınca kabul tarihi/saatini "şu an" olarak hazırlar (kullanıcı isterse değiştirir).
    protected override void OnAppearing()
    {
        base.OnAppearing();
        KabulTarihiPicker.Date = DateTime.Today;
        KabulSaatiPicker.Time = DateTime.Now.TimeOfDay;
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
        var marka = MarkaPicker.SelectedItem?.ToString();
        var model = ModelPicker.SelectedItem?.ToString();
        var seriNo = SeriNoEntry.Text?.Trim();
        var musteriAdi = MusteriAdiEntry.Text?.Trim();
        var musteriTelefon = MusteriTelefonEntry.Text?.Trim();

        if (string.IsNullOrEmpty(marka) || string.IsNullOrEmpty(model) ||
            string.IsNullOrEmpty(seriNo) || string.IsNullOrEmpty(musteriAdi) ||
            string.IsNullOrEmpty(musteriTelefon) || CihazTuruPicker.SelectedIndex < 0)
        {
            SonucMesaji.Text = "* ile işaretli tüm alanları doldurun!";
            SonucMesaji.TextColor = Colors.Red;
            SonucMesaji.IsVisible = true;
            return;
        }

        KaydetButton.IsEnabled = false;

        // SERİ NUMARASI KONTROLÜ:
        // Kaydetmeden önce mevcut cihazları çekip aynı seri numarası var mı diye LINQ ile bakarız.
        // Varsa kullanıcıya nazik uyarı verir, API'ye hiç gitmez (programın çökmesini engeller).
        var mevcutCihazlar = await _apiService.TumCihazlariGetirAsync();
        bool seriNoVarMi = mevcutCihazlar.Any(c =>
            string.Equals(c.SeriNumarasi?.Trim(), seriNo, StringComparison.OrdinalIgnoreCase));

        if (seriNoVarMi)
        {
            SonucMesaji.Text = "Bu seri numarası zaten kayıtlı! Lütfen farklı bir seri numarası girin.";
            SonucMesaji.TextColor = Colors.Orange;
            SonucMesaji.IsVisible = true;
            KaydetButton.IsEnabled = true; // butonu tekrar aktif et
            return;
        }

        // DatePicker'dan gelen tarih ile TimePicker'dan gelen saati tek bir DateTime'da birleştir.
        var kabulTarihi = KabulTarihiPicker.Date.Date + KabulSaatiPicker.Time;

        var cihaz = new CihazDto
        {
            Marka = marka,
            Model = model,
            SeriNumarasi = seriNo,
            CihazTuru = CihazTuruPicker.SelectedItem?.ToString() ?? "Diğer",
            MusteriAdi = musteriAdi,
            MusteriTelefon = musteriTelefon,
            MusteriEmail = MusteriEmailEntry.Text?.Trim(),
            MusteriAdres = MusteriAdresEditor.Text?.Trim(),
            KabulTarihi = kabulTarihi
        };

        var sonuc = await _apiService.CihazEkleAsync(cihaz);

        if (sonuc != null)
        {
            await DisplayAlert("Başarılı", "Cihaz kaydı başarıyla oluşturuldu!", "Tamam");
            await Shell.Current.GoToAsync("..");
        }
        else
        {
            SonucMesaji.Text = "Kayıt oluşturulamadı! Seri numarası zaten var olabilir.";
            SonucMesaji.TextColor = Colors.Red;
            SonucMesaji.IsVisible = true;
        }

        KaydetButton.IsEnabled = true;
    }
}
