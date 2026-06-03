// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Endpoints/LogEndpoints.cs
// Açıklama: Audit log görüntüleme endpoint'leri.
//           Hocaya / arkadaşlara logların gerçekten yazıldığını
//           tarayıcıdan göstermek için kullanılır.
// ============================================================

using ArizaTakipSistemi.API.Services;

namespace ArizaTakipSistemi.API.Endpoints
{
    public static class LogEndpoints
    {
        public static void MapLogEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/loglar").WithTags("Loglar (Audit)");

            // GET /api/loglar -> tüm loglar (en yeniden eskiye)
            group.MapGet("/", async (ILogService service) =>
            {
                var loglar = await service.TumLoglariGetirAsync();
                return Results.Ok(loglar);
            });

            // GET /api/loglar/kullanici/{id} -> bir kullanıcının yaptığı işlemler
            group.MapGet("/kullanici/{kullaniciId}", async (int kullaniciId, ILogService service) =>
            {
                return Results.Ok(await service.KullaniciyaGoreGetirAsync(kullaniciId));
            });

            // GET /api/loglar/tablo/{tabloAdi} -> bir tabloya ait işlemler (örn. "Arizalar")
            group.MapGet("/tablo/{tabloAdi}", async (string tabloAdi, ILogService service) =>
            {
                return Results.Ok(await service.TabloAdınaGoreGetirAsync(tabloAdi));
            });
        }
    }
}
