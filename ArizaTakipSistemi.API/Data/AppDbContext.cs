// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Data/AppDbContext.cs
// Açıklama: Entity Framework Core DbContext sınıfı.
//           MS SQL Server bağlantısı ve tüm entity konfigürasyonları burada yapılır.
// ============================================================

using Microsoft.EntityFrameworkCore;
using ArizaTakipSistemi.API.Models;

namespace ArizaTakipSistemi.API.Data
{
    /// <summary>
    /// Uygulamanın veritabanı bağlam sınıfı.
    /// Code-First yaklaşımıyla MS SQL Server üzerinde çalışır.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ===================== DbSet Tanımları =====================
        public DbSet<Kullanici> Kullanicilar { get; set; }
        public DbSet<Cihaz> Cihazlar { get; set; }
        public DbSet<Ariza> Arizalar { get; set; }
        public DbSet<Log> Loglar { get; set; }

        // ===================== Fluent API Konfigürasyonları =====================
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------- Kullanici Konfigürasyonu ----------
            modelBuilder.Entity<Kullanici>(entity =>
            {
                // Email benzersiz olmalı
                entity.HasIndex(k => k.Email).IsUnique();

                // Varsayılan değerler
                entity.Property(k => k.OlusturulmaTarihi)
                      .HasDefaultValueSql("datetime('now','localtime')");

                entity.Property(k => k.AktifMi)
                      .HasDefaultValue(true);

                entity.Property(k => k.Rol)
                      .HasConversion<int>();
            });

            // ---------- Cihaz Konfigürasyonu ----------
            modelBuilder.Entity<Cihaz>(entity =>
            {
                // Seri numarası benzersiz olmalı
                entity.HasIndex(c => c.SeriNumarasi).IsUnique();

                entity.Property(c => c.OlusturulmaTarihi)
                      .HasDefaultValueSql("datetime('now','localtime')");
            });

            // ---------- Ariza Konfigürasyonu ----------
            modelBuilder.Entity<Ariza>(entity =>
            {
                entity.Property(a => a.OlusturulmaTarihi)
                      .HasDefaultValueSql("datetime('now','localtime')");

                entity.Property(a => a.Durum)
                      .HasConversion<int>();

                entity.Property(a => a.OncelikDurumu)
                      .HasConversion<int>();

                // Cihaz - Ariza ilişkisi (1:N)
                entity.HasOne(a => a.Cihaz)
                      .WithMany(c => c.Arizalar)
                      .HasForeignKey(a => a.CihazId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Kullanici (Teknisyen) - Ariza ilişkisi (1:N)
                entity.HasOne(a => a.AtananTeknisyen)
                      .WithMany(k => k.AtananArizalar)
                      .HasForeignKey(a => a.KullaniciId)
                      .OnDelete(DeleteBehavior.SetNull)
                      .IsRequired(false);
            });

            // ---------- Log Konfigürasyonu ----------
            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(l => l.IslemTarihi)
                      .HasDefaultValueSql("datetime('now','localtime')");

                entity.Property(l => l.IslemTuru)
                      .HasConversion<int>();

                // Kullanici - Log ilişkisi (1:N)
                entity.HasOne(l => l.Kullanici)
                      .WithMany(k => k.Loglar)
                      .HasForeignKey(l => l.KullaniciId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Performans için index
                entity.HasIndex(l => l.IslemTarihi);
                entity.HasIndex(l => l.TabloAdi);
            });

            // ===================== Seed Data (Test Verileri) =====================
            modelBuilder.Entity<Kullanici>().HasData(
                new Kullanici
                {
                    KullaniciId = 1,
                    Ad = "Admin",
                    Soyad = "Yönetici",
                    Email = "admin@arizatakip.com",
                    Sifre = "admin123", // Gerçek projede hash'lenecek
                    Rol = KullaniciRolu.Yonetici,
                    Telefon = "05001234567",
                    AktifMi = true,
                    OlusturulmaTarihi = new DateTime(2026, 1, 1)
                },
                new Kullanici
                {
                    KullaniciId = 2,
                    Ad = "Ahmet",
                    Soyad = "Tekniker",
                    Email = "ahmet@arizatakip.com",
                    Sifre = "ahmet123", // Gerçek projede hash'lenecek
                    Rol = KullaniciRolu.Teknisyen,
                    Telefon = "05009876543",
                    AktifMi = true,
                    OlusturulmaTarihi = new DateTime(2026, 1, 1)
                }
            );
        }
    }
}
