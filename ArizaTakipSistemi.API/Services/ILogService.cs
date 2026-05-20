// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend) / SAİD (Said_CrudUI - Audit Log Kullanımı)
// Dosya: Services/ILogService.cs
// Açıklama: Audit Log servis arayüzü (Dependency Injection için).
// ============================================================

using ArizaTakipSistemi.API.Models;

namespace ArizaTakipSistemi.API.Services
{
    /// <summary>
    /// Audit Log işlemleri için servis arayüzü.
    /// </summary>
    public interface ILogService
    {
        Task<List<Log>> TumLoglariGetirAsync();
        Task<List<Log>> TabloAdınaGoreGetirAsync(string tabloAdi);
        Task<List<Log>> KullaniciyaGoreGetirAsync(int kullaniciId);
        Task<List<Log>> TarihAraliginaGoreGetirAsync(DateTime baslangic, DateTime bitis);
        Task LogEkleAsync(IslemTuru islemTuru, string tabloAdi, int kayitId, string yapilanIslem, int kullaniciId, string? eskiDegerler = null, string? yeniDegerler = null);
    }
}
