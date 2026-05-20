// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend) / SAİD (Said_CrudUI - Kullanım)
// Dosya: Services/IArizaService.cs
// Açıklama: Arıza servis arayüzü (Dependency Injection için).
//           CRUD işlemleri ve LINQ filtreleme metodlarını içerir.
// ============================================================

using ArizaTakipSistemi.API.Models;

namespace ArizaTakipSistemi.API.Services
{
    /// <summary>
    /// Arıza CRUD işlemleri ve filtreleme için servis arayüzü.
    /// </summary>
    public interface IArizaService
    {
        Task<List<Ariza>> TumArizalariGetirAsync();
        Task<Ariza?> ArizaGetirAsync(int id);
        Task<Ariza> ArizaEkleAsync(Ariza ariza);
        Task<Ariza?> ArizaGuncelleAsync(int id, Ariza ariza);
        Task<bool> ArizaSilAsync(int id);

        // LINQ Filtreleme Metodları (Said_CrudUI tarafından kullanılacak)
        Task<List<Ariza>> DurumaGoreFiltrelemeAsync(ArizaDurumu durum);
        Task<List<Ariza>> TarihAraliginaGoreFiltrelemeAsync(DateTime baslangic, DateTime bitis);
        Task<List<Ariza>> KategoriyeGoreFiltrelemeAsync(string kategori);
        Task<List<Ariza>> TeknisyeneGoreFiltrelemeAsync(int kullaniciId);
        Task<List<Ariza>> OnceligeGoreFiltrelemeAsync(OncelikDurumu oncelik);
    }
}
