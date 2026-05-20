// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Program.cs
// Açıklama: Uygulamanın başlangıç noktası, DI yapılandırmaları.
// ============================================================

using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ArizaTakipSistemi.API.Data;
using ArizaTakipSistemi.API.Services;
using ArizaTakipSistemi.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// JSON yapılandırması (EF Core için döngüsel referans hatasını önler)
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// DbContext (SQLite)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Dependency Injection (Servisler)
builder.Services.AddScoped<IKullaniciService, KullaniciService>();
builder.Services.AddScoped<ICihazService, CihazService>();
builder.Services.AddScoped<IArizaService, ArizaService>();
builder.Services.AddScoped<ILogService, LogService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Arıza Takip Sistemi API", Version = "v1" });
});

// CORS: MAUI uygulamasının API'ye erişebilmesi için gerekli olabilir
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Endpoint'lerin kaydedilmesi
app.MapKullaniciEndpoints();
app.MapCihazEndpoints();
app.MapArizaEndpoints();

app.Run();
