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

// ===== Veritabanı otomatik kurulumu =====
// API her başladığında migration'ları uygular. Böylece arkadaşlar
// projeyi klonladıklarında veritabanı dosyası eksik/bozuk olsa bile
// otomatik oluşturulur ve seed verileri (Admin/Teknisyen) yüklenir.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// HTTPS yönlendirmesi yalnızca Production'da aktif olsun.
// Geliştirme ortamında MAUI uygulaması http://localhost:5020'e konuşur;
// HTTPS yönlendirmesi açıkken her bilgisayarda dev sertifikasının
// güvenilir olması gerekiyordu; bu da arkadaşların giriş yapamamasına yol açıyordu.
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

// Endpoint'lerin kaydedilmesi
app.MapKullaniciEndpoints();
app.MapCihazEndpoints();
app.MapArizaEndpoints();
app.MapLogEndpoints();

app.Run();
