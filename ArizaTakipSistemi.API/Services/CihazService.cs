// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Services/CihazService.cs
// Açıklama: Cihaz servis implementasyonu.
// ============================================================

using Microsoft.EntityFrameworkCore;
using ArizaTakipSistemi.API.Data;
using ArizaTakipSistemi.API.Models;

namespace ArizaTakipSistemi.API.Services
{
    public class CihazService : ICihazService
    {
        private readonly AppDbContext _context;

        public CihazService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Cihaz>> TumCihazlariGetirAsync()
        {
            return await _context.Cihazlar.ToListAsync();
        }

        public async Task<Cihaz?> CihazGetirAsync(int id)
        {
            return await _context.Cihazlar
                .Include(c => c.Arizalar)
                .FirstOrDefaultAsync(c => c.CihazId == id);
        }

        public async Task<Cihaz?> SeriNumarasiIleCihazGetirAsync(string seriNumarasi)
        {
            return await _context.Cihazlar
                .FirstOrDefaultAsync(c => c.SeriNumarasi == seriNumarasi);
        }

        public async Task<Cihaz> CihazEkleAsync(Cihaz cihaz)
        {
            var seriNoVarMi = await _context.Cihazlar.AnyAsync(c => c.SeriNumarasi == cihaz.SeriNumarasi);
            if (seriNoVarMi)
                throw new InvalidOperationException($"Bu seri numarasına sahip başka bir cihaz zaten var: {cihaz.SeriNumarasi}");

            _context.Cihazlar.Add(cihaz);
            await _context.SaveChangesAsync();
            return cihaz;
        }

        public async Task<Cihaz?> CihazGuncelleAsync(int id, Cihaz cihaz)
        {
            var mevcut = await _context.Cihazlar.FindAsync(id);
            if (mevcut == null) return null;

            if (mevcut.SeriNumarasi != cihaz.SeriNumarasi)
            {
                var seriNoVarMi = await _context.Cihazlar.AnyAsync(c => c.SeriNumarasi == cihaz.SeriNumarasi);
                if (seriNoVarMi)
                    throw new InvalidOperationException($"Bu seri numarasına sahip başka bir cihaz zaten var: {cihaz.SeriNumarasi}");
            }

            mevcut.MusteriAdi = cihaz.MusteriAdi;
            mevcut.MusteriTelefon = cihaz.MusteriTelefon;
            mevcut.MusteriEmail = cihaz.MusteriEmail;
            mevcut.MusteriAdres = cihaz.MusteriAdres;
            mevcut.Marka = cihaz.Marka;
            mevcut.Model = cihaz.Model;
            mevcut.SeriNumarasi = cihaz.SeriNumarasi;
            mevcut.CihazTuru = cihaz.CihazTuru;

            await _context.SaveChangesAsync();
            return mevcut;
        }

        public async Task<bool> CihazSilAsync(int id)
        {
            var mevcut = await _context.Cihazlar.FindAsync(id);
            if (mevcut == null) return false;

            var arizasiVarMi = await _context.Arizalar.AnyAsync(a => a.CihazId == id);
            if (arizasiVarMi)
                throw new InvalidOperationException("Bu cihaza ait arıza kayıtları bulunmaktadır. Önce cihaza ait arıza kayıtlarını silmelisiniz.");

            _context.Cihazlar.Remove(mevcut);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
