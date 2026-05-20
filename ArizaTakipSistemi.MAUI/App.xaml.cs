// ============================================================
// Sorumlu Geliştirici: EREN (Eren_AuthUI)
// Dosya: App.xaml.cs
// Açıklama: Uygulama başlangıç noktası. Oturum kontrolü yapılır.
// ============================================================

using ArizaTakipSistemi.MAUI.Views.Auth;

namespace ArizaTakipSistemi.MAUI;

public partial class App : Application
{
    private readonly IServiceProvider _serviceProvider;

    public App(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        // Oturum kontrolü: Giriş yapılmışsa ana sayfaya, yoksa login'e yönlendir
        var girisYapildi = Preferences.Get("GirisYapildi", false);

        if (girisYapildi)
        {
            return new Window(new AppShell());
        }
        else
        {
            var loginPage = _serviceProvider.GetRequiredService<LoginView>();
            return new Window(new NavigationPage(loginPage));
        }
    }
}