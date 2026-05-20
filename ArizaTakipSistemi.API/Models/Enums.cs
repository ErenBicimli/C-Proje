// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Models/Enums.cs
// Açıklama: Sistemde kullanılan tüm enum tanımları.
// ============================================================

namespace ArizaTakipSistemi.API.Models
{
    /// <summary>
    /// Kullanıcının sistemdeki rolünü belirler.
    /// </summary>
    public enum KullaniciRolu
    {
        Teknisyen = 0,
        Yonetici = 1
    }

    /// <summary>
    /// Arızanın mevcut durumunu belirler.
    /// </summary>
    public enum ArizaDurumu
    {
        Beklemede = 0,
        DevamEdiyor = 1,
        Tamamlandi = 2,
        IptalEdildi = 3
    }

    /// <summary>
    /// Arızanın öncelik seviyesini belirler.
    /// </summary>
    public enum OncelikDurumu
    {
        Dusuk = 0,
        Normal = 1,
        Yuksek = 2,
        Acil = 3
    }

    /// <summary>
    /// Audit Log'da yapılan işlem türünü belirler.
    /// </summary>
    public enum IslemTuru
    {
        Ekleme = 0,
        Guncelleme = 1,
        Silme = 2
    }
}
