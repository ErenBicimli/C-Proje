// ============================================================
// Sorumlu Geliþtirici: SAÝD (Said_CrudUI)
// Dosya: Views/FaultManagement/CihazTuruSecimView.xaml.cs
// Açýklama: Kartlardan birine týklayýnca seçilen türü ArizaListesi'ne
//           parametre olarak gönderir. Tümü için boþ string.
// ============================================================

namespace ArizaTakipSistemi.MAUI.Views.FaultManagement;

public partial class CihazTuruSecimView : ContentPage
{
    public CihazTuruSecimView()
    {
        InitializeComponent();
    }

    private async Task Aca(string tur)
    {
        // QueryProperty olarak ArizaListesiView'a geçiyoruz.
        await Shell.Current.GoToAsync($"ArizaListesi?CihazTuru={Uri.EscapeDataString(tur)}");
    }

    private async void Telefon_Tapped(object sender, TappedEventArgs e) => await Aca("Telefon");
    private async void Bilgisayar_Tapped(object sender, TappedEventArgs e) => await Aca("Bilgisayar");
    private async void Tablet_Tapped(object sender, TappedEventArgs e) => await Aca("Tablet");
    private async void Yazici_Tapped(object sender, TappedEventArgs e) => await Aca("Yazýcý");
    private async void Monitor_Tapped(object sender, TappedEventArgs e) => await Aca("Monitor");
    private async void Diger_Tapped(object sender, TappedEventArgs e) => await Aca("Diðer");
    private async void Tumu_Tapped(object sender, TappedEventArgs e) => await Aca("");
}
