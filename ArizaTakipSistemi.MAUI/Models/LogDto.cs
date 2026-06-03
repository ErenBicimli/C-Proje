// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Models/LogDto.cs
// Açıklama: API'den gelen audit log kayıtlarının MAUI tarafındaki karşılığı.
// ============================================================

using Microsoft.Maui.Graphics;

namespace ArizaTakipSistemi.MAUI.Models
{
    public class LogDto
    {
        public int LogId { get; set; }
        public int IslemTuru { get; set; }     // 0=Ekleme, 1=Güncelleme, 2=Silme
        public string TabloAdi { get; set; } = string.Empty;
        public int KayitId { get; set; }
        public string YapilanIslem { get; set; } = string.Empty;
        public int KullaniciId { get; set; }
        public DateTime IslemTarihi { get; set; }
        public KullaniciDto? Kullanici { get; set; }

        // UI yardımcı alanları (data binding için)
        public string IslemTuruAdi => IslemTuru switch
        {
            0 => "Ekleme",
            1 => "Güncelleme",
            2 => "Silme",
            _ => "Bilinmiyor"
        };

        public string IslemEmoji => IslemTuru switch
        {
            0 => "➕",
            1 => "✏️",
            2 => "🗑️",
            _ => "❓"
        };

        public Color IslemRengi => IslemTuru switch
        {
            0 => Color.FromArgb("#27ae60"), // yeşil - ekleme
            1 => Color.FromArgb("#3498db"), // mavi - güncelleme
            2 => Color.FromArgb("#e94560"), // kırmızı - silme
            _ => Color.FromArgb("#8899aa")
        };

        public string KullaniciAdi => Kullanici != null
            ? $"{Kullanici.Ad} {Kullanici.Soyad}".Trim()
            : "Bilinmiyor";
    }
}
