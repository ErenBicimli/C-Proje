// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Helpers/MasrafYonetici.cs
// Açıklama: Tamamlanan arızalardan biriken masrafları toplam olarak
//           Preferences'ta saklar. Ciro Hesabı sayfası buradan okur.
// ============================================================

namespace ArizaTakipSistemi.MAUI.Helpers
{
    public static class MasrafYonetici
    {
        private const string KEY = "ToplamGider";

        public static double ToplamGider => Preferences.Get(KEY, 0.0);

        public static void Ekle(double tutar)
        {
            if (tutar <= 0) return;
            Preferences.Set(KEY, ToplamGider + tutar);
        }

        public static void Sifirla()
        {
            Preferences.Set(KEY, 0.0);
        }
    }
}
