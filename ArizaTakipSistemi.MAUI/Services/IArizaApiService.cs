// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Services/IArizaApiService.cs
// Açıklama: API ile iletişim kuran servis arayüzü.
// ============================================================

using ArizaTakipSistemi.MAUI.Models;

namespace ArizaTakipSistemi.MAUI.Services
{
    public interface IArizaApiService
    {
        // Kullanıcı İşlemleri
        Task<KullaniciDto?> GirisYapAsync(string email, string sifre);
        Task<KullaniciDto?> KullaniciKayitAsync(KullaniciDto kullanici, string sifre);
        Task<List<KullaniciDto>> TumKullanicilariGetirAsync();
        Task<bool> SifreDegistirAsync(int kullaniciId, string eskiSifre, string yeniSifre);

        // Cihaz İşlemleri
        Task<List<CihazDto>> TumCihazlariGetirAsync();
        Task<CihazDto?> CihazGetirAsync(int id);
        Task<CihazDto?> CihazEkleAsync(CihazDto cihaz);
        Task<CihazDto?> CihazGuncelleAsync(int id, CihazDto cihaz);
        Task<bool> CihazSilAsync(int id);

        // Arıza İşlemleri
        Task<List<ArizaDto>> TumArizalariGetirAsync();
        Task<ArizaDto?> ArizaGetirAsync(int id);
        Task<ArizaDto?> ArizaEkleAsync(ArizaDto ariza);
        Task<ArizaDto?> ArizaGuncelleAsync(int id, ArizaDto ariza);
        Task<bool> ArizaSilAsync(int id);
        Task<List<ArizaDto>> DurumaGoreFiltrelemeAsync(int durum);
        Task<List<ArizaDto>> TeknisyeneGoreFiltrelemeAsync(int kullaniciId);

        // Audit Log İşlemleri (Said_CrudUI)
        Task<List<LogDto>> TumLoglariGetirAsync();
    }
}
