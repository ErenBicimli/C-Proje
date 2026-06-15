// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend) / SAİD (Said_CrudUI - Audit Log Bağlantısı)
// Dosya: Endpoints/ArizaEndpoints.cs
// Güncelleme: Ekle / Güncelle / Sil işlemlerine audit log bağlandı.
//             İşlemi yapan kullanıcının kimliği "X-Kullanici-Id" header'ından
//             okunur ve ILogService.LogEkleAsync ile Loglar tablosuna yazılır.
// ============================================================

using ArizaTakipSistemi.API.Models;
using ArizaTakipSistemi.API.Services;

namespace ArizaTakipSistemi.API.Endpoints
{
    public static class ArizaEndpoints
    {
        public static void MapArizaEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/arizalar").WithTags("Arızalar");

            // GET: tüm arızalar (loglanmaz - okuma işlemi)
            group.MapGet("/", async (IArizaService service) =>
            {
                return Results.Ok(await service.TumArizalariGetirAsync());
            });

            // GET: tek arıza (loglanmaz - okuma işlemi)
            group.MapGet("/{id}", async (int id, IArizaService service) =>
            {
                var ariza = await service.ArizaGetirAsync(id);
                return ariza != null ? Results.Ok(ariza) : Results.NotFound();
            });

            // POST: yeni arıza ekleme + AUDIT LOG
            group.MapPost("/", async (Ariza ariza, IArizaService service, ILogService logService, HttpRequest req) =>
            {
                try
                {
                    var yeniAriza = await service.ArizaEkleAsync(ariza);
                    int kid = KullaniciIdAl(req, yeniAriza.KullaniciId ?? 0);
                    await logService.LogEkleAsync(
                        IslemTuru.Ekleme,
                        "Arizalar",
                        yeniAriza.ArizaId,
                        $"Yeni arıza eklendi: {yeniAriza.ArizaTanimi}",
                        kid);
                    return Results.Created($"/api/arizalar/{yeniAriza.ArizaId}", yeniAriza);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { mesaj = "Arıza eklenirken bir hata oluştu: " + ex.Message });
                }
            });

            // PUT: arıza güncelleme + AUDIT LOG
            group.MapPut("/{id}", async (int id, Ariza ariza, IArizaService service, ILogService logService, HttpRequest req) =>
            {
                try
                {
                    var guncelAriza = await service.ArizaGuncelleAsync(id, ariza);
                    if (guncelAriza == null) return Results.NotFound();

                    int kid = KullaniciIdAl(req, guncelAriza.KullaniciId ?? 0);
                    await logService.LogEkleAsync(
                        IslemTuru.Guncelleme,
                        "Arizalar",
                        guncelAriza.ArizaId,
                        $"Arıza güncellendi: {guncelAriza.ArizaTanimi} (Durum: {guncelAriza.Durum})",
                        kid);
                    return Results.Ok(guncelAriza);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { mesaj = "Arıza güncellenirken bir hata oluştu: " + ex.Message });
                }
            });

            // DELETE: arıza silme + AUDIT LOG
            group.MapDelete("/{id}", async (int id, IArizaService service, ILogService logService, HttpRequest req) =>
            {
                // Silmeden önce kaydı al, log mesajına tanımı yazmak için.
                var silinecek = await service.ArizaGetirAsync(id);
                var sonuc = await service.ArizaSilAsync(id);
                if (!sonuc) return Results.NotFound();

                int kid = KullaniciIdAl(req, silinecek?.KullaniciId ?? 0);
                await logService.LogEkleAsync(
                    IslemTuru.Silme,
                    "Arizalar",
                    id,
                    $"Arıza silindi: {silinecek?.ArizaTanimi}",
                    kid);
                return Results.NoContent();
            });

            // Filtreleme endpoint'leri (okuma - loglanmaz)
            group.MapGet("/filtre/durum/{durum}", async (ArizaDurumu durum, IArizaService service) =>
            {
                return Results.Ok(await service.DurumaGoreFiltrelemeAsync(durum));
            });

            group.MapGet("/filtre/teknisyen/{kullaniciId}", async (int kullaniciId, IArizaService service) =>
            {
                return Results.Ok(await service.TeknisyeneGoreFiltrelemeAsync(kullaniciId));
            });
        }

        // İşlemi yapan kullanıcının kimliğini "X-Kullanici-Id" header'ından okur.
        // Header yoksa veya geçersizse, arızanın atanmış teknisyen ID'sini kullanır.
        // O da yoksa son çare olarak 1 (Admin) döner. Böylece log her durumda düşer.
        private static int KullaniciIdAl(HttpRequest req, int fallbackId = 0)
        {
            if (req.Headers.TryGetValue("X-Kullanici-Id", out var v) && int.TryParse(v, out var i) && i > 0)
                return i;
            if (fallbackId > 0)
                return fallbackId;
            return 1; // son çare: Admin (seed kullanıcı)
        }
    }
}
