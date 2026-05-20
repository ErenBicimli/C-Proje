// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Endpoints/KullaniciEndpoints.cs
// Açıklama: Kullanıcı minimal API endpoint'leri
// ============================================================

using ArizaTakipSistemi.API.Models;
using ArizaTakipSistemi.API.Services;

namespace ArizaTakipSistemi.API.Endpoints
{
    public static class KullaniciEndpoints
    {
        public static void MapKullaniciEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/kullanicilar").WithTags("Kullanıcılar");

            group.MapGet("/", async (IKullaniciService service) =>
            {
                var kullanicilar = await service.TumKullanicilariGetirAsync();
                return Results.Ok(kullanicilar);
            });

            group.MapGet("/{id}", async (int id, IKullaniciService service) =>
            {
                var kullanici = await service.KullaniciGetirAsync(id);
                return kullanici != null ? Results.Ok(kullanici) : Results.NotFound();
            });

            group.MapPost("/", async (Kullanici kullanici, IKullaniciService service) =>
            {
                var yeniKullanici = await service.KullaniciEkleAsync(kullanici);
                return Results.Created($"/api/kullanicilar/{yeniKullanici.KullaniciId}", yeniKullanici);
            });

            group.MapPut("/{id}", async (int id, Kullanici kullanici, IKullaniciService service) =>
            {
                var guncelKullanici = await service.KullaniciGuncelleAsync(id, kullanici);
                return guncelKullanici != null ? Results.Ok(guncelKullanici) : Results.NotFound();
            });

            group.MapDelete("/{id}", async (int id, IKullaniciService service) =>
            {
                var sonuc = await service.KullaniciSilAsync(id);
                return sonuc ? Results.NoContent() : Results.NotFound();
            });

            group.MapPost("/giris", async (LoginDto loginDto, IKullaniciService service) =>
            {
                var kullanici = await service.GirisYapAsync(loginDto.Email, loginDto.Sifre);
                return kullanici != null ? Results.Ok(kullanici) : Results.Unauthorized();
            });

            group.MapPost("/{id}/sifre-degistir", async (int id, SifreDegistirDto dto, IKullaniciService service) =>
            {
                var sonuc = await service.SifreDegistirAsync(id, dto.EskiSifre, dto.YeniSifre);
                return sonuc ? Results.Ok() : Results.BadRequest("Mevcut şifre hatalı!");
            });
        }
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
    }

    public class SifreDegistirDto
    {
        public string EskiSifre { get; set; } = string.Empty;
        public string YeniSifre { get; set; } = string.Empty;
    }
}
