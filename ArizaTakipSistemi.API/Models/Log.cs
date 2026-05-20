// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend) / SAİD (Said_CrudUI - Kullanım)
// Dosya: Models/Log.cs
// Açıklama: Audit Log entity sınıfı (Code-First).
//           Sistemde yapılan tüm ekleme/güncelleme/silme işlemlerini loglar.
// ============================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArizaTakipSistemi.API.Models
{
    /// <summary>
    /// Audit Log: Sistemde yapılan tüm CRUD işlemlerinin kaydını tutan entity sınıfı.
    /// Kimin, ne zaman, hangi tabloda, ne yaptığını takip eder.
    /// </summary>
    [Table("Loglar")]
    public class Log
    {
        [Key]
        public int LogId { get; set; }

        [Required]
        public IslemTuru IslemTuru { get; set; }

        [Required]
        [StringLength(50)]
        public string TabloAdi { get; set; } = string.Empty;

        [Required]
        public int KayitId { get; set; }

        [Required]
        [StringLength(500)]
        public string YapilanIslem { get; set; } = string.Empty;

        /// <summary>
        /// Eski değerler (JSON formatında saklanır).
        /// </summary>
        [StringLength(2000)]
        public string? EskiDegerler { get; set; }

        /// <summary>
        /// Yeni değerler (JSON formatında saklanır).
        /// </summary>
        [StringLength(2000)]
        public string? YeniDegerler { get; set; }

        // Foreign Key: İşlemi yapan kullanıcı
        [Required]
        public int KullaniciId { get; set; }

        public DateTime IslemTarihi { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey("KullaniciId")]
        public virtual Kullanici? Kullanici { get; set; }
    }
}
