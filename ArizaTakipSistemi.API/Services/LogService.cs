// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Services/LogService.cs
// Açıklama: Log servis implementasyonu.
// ============================================================

using Microsoft.EntityFrameworkCore;
using ArizaTakipSistemi.API.Data;
using ArizaTakipSistemi.API.Models;

namespace ArizaTakipSistemi.API.Services
{
    public class LogService : ILogService
    {
        private readonly AppDbContext _context;

        public LogService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Log>> TumLoglariGetirAsync()
        {
            return await _context.Loglar
                .Include(l => l.Kullanici)
                .OrderByDescending(l => l.IslemTarihi)
                .ToListAsync();
        }

        public async Task<List<Log>> TabloAdınaGoreGetirAsync(string tabloAdi)
        {
            return await _context.Loglar
                .Include(l => l.Kullanici)
                .Where(l => l.TabloAdi == tabloAdi)
                .OrderByDescending(l => l.IslemTarihi)
                .ToListAsync();
        }

        public async Task<List<Log>> KullaniciyaGoreGetirAsync(int kullaniciId)
        {
            return await _context.Loglar
                .Include(l => l.Kullanici)
                .Where(l => l.KullaniciId == kullaniciId)
                .OrderByDescending(l => l.IslemTarihi)
                .ToListAsync();
        }

        public async Task<List<Log>> TarihAraliginaGoreGetirAsync(DateTime baslangic, DateTime bitis)
        {
            return await _context.Loglar
                .Include(l => l.Kullanici)
                .Where(l => l.IslemTarihi >= baslangic && l.IslemTarihi <= bitis)
                .OrderByDescending(l => l.IslemTarihi)
                .ToListAsync();
        }

        public async Task LogEkleAsync(IslemTuru islemTuru, string tabloAdi, int kayitId, string yapilanIslem, int kullaniciId, string? eskiDegerler = null, string? yeniDegerler = null)
        {
            var log = new Log
            {
                IslemTuru = islemTuru,
                TabloAdi = tabloAdi,
                KayitId = kayitId,
                YapilanIslem = yapilanIslem,
                KullaniciId = kullaniciId,
                IslemTarihi = DateTime.Now,
                EskiDegerler = eskiDegerler,
                YeniDegerler = yeniDegerler
            };

            _context.Loglar.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
