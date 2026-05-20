// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Views/FaultManagement/CihazEkleView.xaml.cs
// Açıklama: Yeni cihaz ekleme sayfası code-behind.
// ============================================================

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

    private async void KaydetButton_Clicked(object sender, EventArgs e)
    {
        var marka = MarkaEntry.Text?.Trim();
        var model = ModelEntry.Text?.Trim();
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

        var cihaz = new CihazDto
        {
            Marka = marka,
            Model = model,
            SeriNumarasi = seriNo,
            CihazTuru = CihazTuruPicker.SelectedItem?.ToString() ?? "Diğer",
            MusteriAdi = musteriAdi,
            MusteriTelefon = musteriTelefon,
            MusteriEmail = MusteriEmailEntry.Text?.Trim(),
            MusteriAdres = MusteriAdresEditor.Text?.Trim()
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
