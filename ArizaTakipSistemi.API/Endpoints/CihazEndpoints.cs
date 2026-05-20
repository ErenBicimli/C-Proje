// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Endpoints/CihazEndpoints.cs
// ============================================================

using ArizaTakipSistemi.API.Models;
using ArizaTakipSistemi.API.Services;

namespace ArizaTakipSistemi.API.Endpoints
{
    public static class CihazEndpoints
    {
        public static void MapCihazEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/cihazlar").WithTags("Cihazlar");

            group.MapGet("/", async (ICihazService service) =>
            {
                return Results.Ok(await service.TumCihazlariGetirAsync());
            });

            group.MapGet("/{id}", async (int id, ICihazService service) =>
            {
                var cihaz = await service.CihazGetirAsync(id);
                return cihaz != null ? Results.Ok(cihaz) : Results.NotFound();
            });

            group.MapGet("/seri/{seriNumarasi}", async (string seriNumarasi, ICihazService service) =>
            {
                var cihaz = await service.SeriNumarasiIleCihazGetirAsync(seriNumarasi);
                return cihaz != null ? Results.Ok(cihaz) : Results.NotFound();
            });

            group.MapPost("/", async (Cihaz cihaz, ICihazService service) =>
            {
                var yeniCihaz = await service.CihazEkleAsync(cihaz);
                return Results.Created($"/api/cihazlar/{yeniCihaz.CihazId}", yeniCihaz);
            });

            group.MapPut("/{id}", async (int id, Cihaz cihaz, ICihazService service) =>
            {
                var guncelCihaz = await service.CihazGuncelleAsync(id, cihaz);
                return guncelCihaz != null ? Results.Ok(guncelCihaz) : Results.NotFound();
            });

            group.MapDelete("/{id}", async (int id, ICihazService service) =>
            {
                var sonuc = await service.CihazSilAsync(id);
                return sonuc ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}
