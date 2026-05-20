// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Services/IKullaniciService.cs
// Açıklama: Kullanıcı servis arayüzü (Dependency Injection için).
// ============================================================

using ArizaTakipSistemi.API.Models;

namespace ArizaTakipSistemi.API.Services
{
    /// <summary>
    /// Kullanıcı CRUD işlemleri için servis arayüzü.
    /// </summary>
    public interface IKullaniciService
    {
        Task<List<Kullanici>> TumKullanicilariGetirAsync();
        Task<Kullanici?> KullaniciGetirAsync(int id);
        Task<Kullanici?> GirisYapAsync(string email, string sifre);
        Task<Kullanici> KullaniciEkleAsync(Kullanici kullanici);
        Task<Kullanici?> KullaniciGuncelleAsync(int id, Kullanici kullanici);
        Task<bool> KullaniciSilAsync(int id);
        Task<bool> SifreDegistirAsync(int id, string eskiSifre, string yeniSifre);
    }
}
