// ============================================================
// Sorumlu Geliþtirici: SAÝD (Said_CrudUI)
// Dosya: Views/FaultManagement/LogListesiView.xaml.cs
// Açýklama: Audit log ekranýnýn mantýk (code-behind) kýsmý.
//           API'den loglarý çekip CollectionView'a baðlar.
// ============================================================

using ArizaTakipSistemi.MAUI.Models;
using ArizaTakipSistemi.MAUI.Services;

namespace ArizaTakipSistemi.MAUI.Views.FaultManagement;

public partial class LogListesiView : ContentPage
{
    private readonly IArizaApiService _apiService;

    public LogListesiView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    // Sayfa her açýldýðýnda loglarý tazele.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoglariYukle();
    }

    private async Task LoglariYukle()
    {
        var loglar = await _apiService.TumLoglariGetirAsync();
        // En yeni log en üstte olsun (API zaten sýralý dönüyorsa da güvenli olsun diye).
        var siraliLoglar = loglar.OrderByDescending(l => l.IslemTarihi).ToList();
        LogCollectionView.ItemsSource = siraliLoglar;
        ToplamLabel.Text = $"{siraliLoglar.Count} kayýt";
    }

    private async void Yenile_Clicked(object sender, EventArgs e)
    {
        await LoglariYukle();
    }
}
