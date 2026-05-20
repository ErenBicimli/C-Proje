// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Services/KullaniciService.cs
// Açıklama: Kullanıcı servis implementasyonu.
//           Kullanıcı CRUD, giriş ve şifre değiştirme işlemleri.
// ============================================================

using Microsoft.EntityFrameworkCore;
using ArizaTakipSistemi.API.Data;
using ArizaTakipSistemi.API.Models;

namespace ArizaTakipSistemi.API.Services
{
    /// <summary>
    /// IKullaniciService arayüzünün implementasyonu.
    /// </summary>
    public class KullaniciService : IKullaniciService
    {
        private readonly AppDbContext _context;

        public KullaniciService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Kullanici>> TumKullanicilariGetirAsync()
        {
            return await _context.Kullanicilar
                .Where(k => k.AktifMi)
                .OrderBy(k => k.Ad)
                .ToListAsync();
        }

        public async Task<Kullanici?> KullaniciGetirAsync(int id)
        {
            return await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.KullaniciId == id);
        }

        public async Task<Kullanici?> GirisYapAsync(string email, string sifre)
        {
            var kullanici = await _context.Kullanicilar
                .FirstOrDefaultAsync(k => k.Email == email && k.Sifre == sifre && k.AktifMi);

            if (kullanici != null)
            {
                kullanici.SonGirisTarihi = DateTime.Now;
                await _context.SaveChangesAsync();
            }

            return kullanici;
        }

        public async Task<Kullanici> KullaniciEkleAsync(Kullanici kullanici)
        {
            kullanici.OlusturulmaTarihi = DateTime.Now;
            _context.Kullanicilar.Add(kullanici);
            await _context.SaveChangesAsync();
            return kullanici;
        }

        public async Task<Kullanici?> KullaniciGuncelleAsync(int id, Kullanici kullanici)
        {
            var mevcutKullanici = await _context.Kullanicilar.FindAsync(id);
            if (mevcutKullanici == null) return null;

            mevcutKullanici.Ad = kullanici.Ad;
            mevcutKullanici.Soyad = kullanici.Soyad;
            mevcutKullanici.Email = kullanici.Email;
            mevcutKullanici.Telefon = kullanici.Telefon;
            mevcutKullanici.Rol = kullanici.Rol;

            await _context.SaveChangesAsync();
            return mevcutKullanici;
        }

        public async Task<bool> KullaniciSilAsync(int id)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null) return false;

            // Soft delete: Kullanıcıyı silmek yerine pasif yapıyoruz
            kullanici.AktifMi = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SifreDegistirAsync(int id, string eskiSifre, string yeniSifre)
        {
            var kullanici = await _context.Kullanicilar.FindAsync(id);
            if (kullanici == null || kullanici.Sifre != eskiSifre) return false;

            kullanici.Sifre = yeniSifre;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
