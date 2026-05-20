// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Services/ICihazService.cs
// Açıklama: Cihaz servis arayüzü (Dependency Injection için).
// ============================================================

using ArizaTakipSistemi.API.Models;

namespace ArizaTakipSistemi.API.Services
{
    /// <summary>
    /// Cihaz CRUD işlemleri için servis arayüzü.
    /// </summary>
    public interface ICihazService
    {
        Task<List<Cihaz>> TumCihazlariGetirAsync();
        Task<Cihaz?> CihazGetirAsync(int id);
        Task<Cihaz?> SeriNumarasiIleCihazGetirAsync(string seriNumarasi);
        Task<Cihaz> CihazEkleAsync(Cihaz cihaz);
        Task<Cihaz?> CihazGuncelleAsync(int id, Cihaz cihaz);
        Task<bool> CihazSilAsync(int id);
    }
}
