// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Services/ArizaApiService.cs
// Açıklama: API ile HTTP üzerinden iletişim kuran servis implementasyonu.
// ============================================================

using System.Net.Http.Json;
using ArizaTakipSistemi.MAUI.Models;

namespace ArizaTakipSistemi.MAUI.Services
{
    public class ArizaApiService : IArizaApiService
    {
        private readonly HttpClient _httpClient;

        public ArizaApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ===================== Kullanıcı İşlemleri =====================

        public async Task<KullaniciDto?> GirisYapAsync(string email, string sifre)
        {
            try
            {
                var loginDto = new LoginDto { Email = email, Sifre = sifre };
                var response = await _httpClient.PostAsJsonAsync("api/kullanicilar/giris", loginDto);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<KullaniciDto>();
                return null;
            }
            catch { return null; }
        }

        public async Task<KullaniciDto?> KullaniciKayitAsync(KullaniciDto kullanici, string sifre)
        {
            try
            {
                var kayitDto = new
                {
                    ad = kullanici.Ad,
                    soyad = kullanici.Soyad,
                    email = kullanici.Email,
                    sifre = sifre,
                    telefon = kullanici.Telefon,
                    rol = kullanici.Rol
                };
                var response = await _httpClient.PostAsJsonAsync("api/kullanicilar", kayitDto);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<KullaniciDto>();
                return null;
            }
            catch { return null; }
        }

        public async Task<List<KullaniciDto>> TumKullanicilariGetirAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<KullaniciDto>>("api/kullanicilar") ?? new();
            }
            catch { return new(); }
        }

        public async Task<bool> SifreDegistirAsync(int kullaniciId, string eskiSifre, string yeniSifre)
        {
            try
            {
                var dto = new SifreDegistirDto { EskiSifre = eskiSifre, YeniSifre = yeniSifre };
                var response = await _httpClient.PostAsJsonAsync($"api/kullanicilar/{kullaniciId}/sifre-degistir", dto);
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ===================== Cihaz İşlemleri =====================

        public async Task<List<CihazDto>> TumCihazlariGetirAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<CihazDto>>("api/cihazlar") ?? new();
            }
            catch { return new(); }
        }

        public async Task<CihazDto?> CihazGetirAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CihazDto>($"api/cihazlar/{id}");
            }
            catch { return null; }
        }

        public async Task<CihazDto?> CihazEkleAsync(CihazDto cihaz)
        {
            try
            {
                var payload = new
                {
                    marka = cihaz.Marka,
                    model = cihaz.Model,
                    seriNumarasi = cihaz.SeriNumarasi,
                    cihazTuru = cihaz.CihazTuru,
                    musteriAdi = cihaz.MusteriAdi,
                    musteriTelefon = cihaz.MusteriTelefon,
                    musteriEmail = cihaz.MusteriEmail,
                    musteriAdres = cihaz.MusteriAdres,
                    // Kabul tarihi de gönderilir. Backend'de alan eklenince otomatik kaydedilir,
                    // alan yoksa API bu fazladan veriyi sessizce yok sayar (hata vermez).
                    kabulTarihi = cihaz.KabulTarihi
                };

                var response = await _httpClient.PostAsJsonAsync("api/cihazlar", payload);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<CihazDto>();
                return null;
            }
            catch { return null; }
        }

        public async Task<CihazDto?> CihazGuncelleAsync(int id, CihazDto cihaz)
        {
            try
            {
                var payload = new
                {
                    marka = cihaz.Marka,
                    model = cihaz.Model,
                    seriNumarasi = cihaz.SeriNumarasi,
                    cihazTuru = cihaz.CihazTuru,
                    musteriAdi = cihaz.MusteriAdi,
                    musteriTelefon = cihaz.MusteriTelefon,
                    musteriEmail = cihaz.MusteriEmail,
                    musteriAdres = cihaz.MusteriAdres
                };

                var response = await _httpClient.PutAsJsonAsync($"api/cihazlar/{id}", payload);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<CihazDto>();
                return null;
            }
            catch { return null; }
        }

        public async Task<bool> CihazSilAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/cihazlar/{id}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        // ===================== Arıza İşlemleri =====================

        public async Task<List<ArizaDto>> TumArizalariGetirAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ArizaDto>>("api/arizalar") ?? new();
            }
            catch { return new(); }
        }

        public async Task<ArizaDto?> ArizaGetirAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ArizaDto>($"api/arizalar/{id}");
            }
            catch { return null; }
        }

        public async Task<ArizaDto?> ArizaEkleAsync(ArizaDto ariza)
        {
            try
            {
                var payload = new
                {
                    cihazId = ariza.CihazId,
                    kullaniciId = ariza.KullaniciId,
                    arizaTanimi = ariza.ArizaTanimi,
                    kategori = ariza.Kategori,
                    durum = ariza.Durum,
                    oncelikDurumu = ariza.OncelikDurumu,
                    tahminiMaliyet = ariza.TahminiMaliyet
                };

                var response = await _httpClient.PostAsJsonAsync("api/arizalar", payload);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("ARIZA EKLE RESPONSE: " + json);
                    return System.Text.Json.JsonSerializer.Deserialize<ArizaDto>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ARIZA EKLE HATA: " + ex.Message);
                return null;
            }
        }

        public async Task<ArizaDto?> ArizaGuncelleAsync(int id, ArizaDto ariza)
        {
            try
            {
                var payload = new
                {
                    cihazId = ariza.CihazId,
                    kullaniciId = ariza.KullaniciId,
                    arizaTanimi = ariza.ArizaTanimi,
                    kategori = ariza.Kategori,
                    durum = ariza.Durum,
                    oncelikDurumu = ariza.OncelikDurumu,
                    yapilanIslem = ariza.YapilanIslem,
                    tahminiMaliyet = ariza.TahminiMaliyet
                };

                var response = await _httpClient.PutAsJsonAsync($"api/arizalar/{id}", payload);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<ArizaDto>();
                return null;
            }
            catch { return null; }
        }

        public async Task<bool> ArizaSilAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/arizalar/{id}");
                return response.IsSuccessStatusCode;
            }
            catch { return false; }
        }

        public async Task<List<ArizaDto>> DurumaGoreFiltrelemeAsync(int durum)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ArizaDto>>($"api/arizalar/filtre/durum/{durum}") ?? new();
            }
            catch { return new(); }
        }

        public async Task<List<ArizaDto>> TeknisyeneGoreFiltrelemeAsync(int kullaniciId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<ArizaDto>>($"api/arizalar/filtre/teknisyen/{kullaniciId}") ?? new();
            }
            catch { return new(); }
        }
    }
}
