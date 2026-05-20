// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Views/FaultManagement/ArizaListesiView.xaml.cs
// Açıklama: Arıza listesi sayfası code-behind.
// ============================================================

using ArizaTakipSistemi.MAUI.Services;

namespace ArizaTakipSistemi.MAUI.Views.FaultManagement;

public partial class ArizaListesiView : ContentPage
{
    private readonly IArizaApiService _apiService;

    public ArizaListesiView(IArizaApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var ad = Preferences.Get("KullaniciAd", "Kullanıcı");
        KullaniciBilgi.Text = $"👤 {ad}";
        await ArizalariYukle();
    }

    private async Task ArizalariYukle()
    {
        var arizalar = await _apiService.TumArizalariGetirAsync();
        ArizaCollectionView.ItemsSource = arizalar;
    }

    private async void FiltreTumu_Clicked(object sender, EventArgs e)
    {
        await ArizalariYukle();
    }

    private async void FiltreBeklemede_Clicked(object sender, EventArgs e)
    {
        var arizalar = await _apiService.DurumaGoreFiltrelemeAsync(0);
        ArizaCollectionView.ItemsSource = arizalar;
    }

    private async void FiltreDevamEdiyor_Clicked(object sender, EventArgs e)
    {
        var arizalar = await _apiService.DurumaGoreFiltrelemeAsync(1);
        ArizaCollectionView.ItemsSource = arizalar;
    }

    private async void FiltreTamamlandi_Clicked(object sender, EventArgs e)
    {
        var arizalar = await _apiService.DurumaGoreFiltrelemeAsync(2);
        ArizaCollectionView.ItemsSource = arizalar;
    }

    private async void YeniAriza_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ArizaEkleView));
    }

    private async void ArizaDetay_Tapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is int arizaId)
        {
            await Shell.Current.GoToAsync($"{nameof(ArizaGuncelleView)}?ArizaId={arizaId}");
        }
    }
}
