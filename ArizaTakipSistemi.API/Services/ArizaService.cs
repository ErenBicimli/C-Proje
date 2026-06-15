// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Services/ArizaService.cs
// Açıklama: Arıza servis implementasyonu.
// ============================================================

using Microsoft.EntityFrameworkCore;
using ArizaTakipSistemi.API.Data;
using ArizaTakipSistemi.API.Models;

namespace ArizaTakipSistemi.API.Services
{
    public class ArizaService : IArizaService
    {
        private readonly AppDbContext _context;

        public ArizaService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ariza>> TumArizalariGetirAsync()
        {
            return await _context.Arizalar
                .Include(a => a.Cihaz)
                .Include(a => a.AtananTeknisyen)
                .ToListAsync();
        }

        public async Task<Ariza?> ArizaGetirAsync(int id)
        {
            return await _context.Arizalar
                .Include(a => a.Cihaz)
                .Include(a => a.AtananTeknisyen)
                .FirstOrDefaultAsync(a => a.ArizaId == id);
        }

        public async Task<Ariza> ArizaEkleAsync(Ariza ariza)
        {
            ariza.OlusturulmaTarihi = DateTime.Now;
            _context.Arizalar.Add(ariza);
            await _context.SaveChangesAsync();
            return ariza;
        }

        public async Task<Ariza?> ArizaGuncelleAsync(int id, Ariza ariza)
        {
            var mevcut = await _context.Arizalar.FindAsync(id);
            if (mevcut == null) return null;

            mevcut.Durum = ariza.Durum;
            mevcut.OncelikDurumu = ariza.OncelikDurumu;
            mevcut.Kategori = ariza.Kategori;
            mevcut.ArizaTanimi = ariza.ArizaTanimi;
            mevcut.YapilanIslem = ariza.YapilanIslem;
            mevcut.TahminiMaliyet = ariza.TahminiMaliyet;
            mevcut.HarcananMasraf = ariza.HarcananMasraf;
            mevcut.KullaniciId = ariza.KullaniciId;
            mevcut.GuncellemeTarihi = DateTime.Now;
            
            if (ariza.Durum == ArizaDurumu.Tamamlandi || ariza.Durum == ArizaDurumu.IptalEdildi)
            {
                if (mevcut.TamamlanmaTarihi == null)
                    mevcut.TamamlanmaTarihi = DateTime.Now;
            }
            else
            {
                mevcut.TamamlanmaTarihi = null;
            }

            await _context.SaveChangesAsync();
            return mevcut;
        }

        public async Task<bool> ArizaSilAsync(int id)
        {
            var mevcut = await _context.Arizalar.FindAsync(id);
            if (mevcut == null) return false;

            _context.Arizalar.Remove(mevcut);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Ariza>> DurumaGoreFiltrelemeAsync(ArizaDurumu durum)
        {
            return await _context.Arizalar
                .Include(a => a.Cihaz)
                .Include(a => a.AtananTeknisyen)
                .Where(a => a.Durum == durum)
                .ToListAsync();
        }

        public async Task<List<Ariza>> TarihAraliginaGoreFiltrelemeAsync(DateTime baslangic, DateTime bitis)
        {
            return await _context.Arizalar
                .Include(a => a.Cihaz)
                .Include(a => a.AtananTeknisyen)
                .Where(a => a.OlusturulmaTarihi >= baslangic && a.OlusturulmaTarihi <= bitis)
                .ToListAsync();
        }

        public async Task<List<Ariza>> KategoriyeGoreFiltrelemeAsync(string kategori)
        {
            return await _context.Arizalar
                .Include(a => a.Cihaz)
                .Include(a => a.AtananTeknisyen)
                .Where(a => a.Kategori.Contains(kategori))
                .ToListAsync();
        }

        public async Task<List<Ariza>> TeknisyeneGoreFiltrelemeAsync(int kullaniciId)
        {
            return await _context.Arizalar
                .Include(a => a.Cihaz)
                .Include(a => a.AtananTeknisyen)
                .Where(a => a.KullaniciId == kullaniciId)
                .ToListAsync();
        }

        public async Task<List<Ariza>> OnceligeGoreFiltrelemeAsync(OncelikDurumu oncelik)
        {
            return await _context.Arizalar
                .Include(a => a.Cihaz)
                .Include(a => a.AtananTeknisyen)
                .Where(a => a.OncelikDurumu == oncelik)
                .ToListAsync();
        }
    }
}
