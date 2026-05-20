// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Endpoints/ArizaEndpoints.cs
// ============================================================

using ArizaTakipSistemi.API.Models;
using ArizaTakipSistemi.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ArizaTakipSistemi.API.Endpoints
{
    public static class ArizaEndpoints
    {
        public static void MapArizaEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/arizalar").WithTags("Arızalar");

            group.MapGet("/", async (IArizaService service) =>
            {
                return Results.Ok(await service.TumArizalariGetirAsync());
            });

            group.MapGet("/{id}", async (int id, IArizaService service) =>
            {
                var ariza = await service.ArizaGetirAsync(id);
                return ariza != null ? Results.Ok(ariza) : Results.NotFound();
            });

            group.MapPost("/", async (Ariza ariza, IArizaService service) =>
            {
                var yeniAriza = await service.ArizaEkleAsync(ariza);
                return Results.Created($"/api/arizalar/{yeniAriza.ArizaId}", yeniAriza);
            });

            group.MapPut("/{id}", async (int id, Ariza ariza, IArizaService service) =>
            {
                var guncelAriza = await service.ArizaGuncelleAsync(id, ariza);
                return guncelAriza != null ? Results.Ok(guncelAriza) : Results.NotFound();
            });

            group.MapDelete("/{id}", async (int id, IArizaService service) =>
            {
                var sonuc = await service.ArizaSilAsync(id);
                return sonuc ? Results.NoContent() : Results.NotFound();
            });

            // Filtreleme Endpoints
            group.MapGet("/filtre/durum/{durum}", async (ArizaDurumu durum, IArizaService service) =>
            {
                return Results.Ok(await service.DurumaGoreFiltrelemeAsync(durum));
            });

            group.MapGet("/filtre/teknisyen/{kullaniciId}", async (int kullaniciId, IArizaService service) =>
            {
                return Results.Ok(await service.TeknisyeneGoreFiltrelemeAsync(kullaniciId));
            });
        }
    }
}
