// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Models/Ariza.cs
// Açıklama: Arıza entity sınıfı (Code-First).
//           Cihazlara ait arıza/servis kayıtlarını temsil eder.
// ============================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArizaTakipSistemi.API.Models
{
    /// <summary>
    /// Cihazlara ait arıza/servis kayıtlarını temsil eden entity sınıfı.
    /// </summary>
    [Table("Arizalar")]
    public class Ariza
    {
        [Key]
        public int ArizaId { get; set; }

        // Foreign Key: Cihaz
        [Required]
        public int CihazId { get; set; }

        // Foreign Key: Atanan Teknisyen (Kullanıcı)
        public int? KullaniciId { get; set; }

        [Required(ErrorMessage = "Arıza tanımı zorunludur.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Arıza tanımı 10-500 karakter arasında olmalıdır.")]
        public string ArizaTanimi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori alanı zorunludur.")]
        [StringLength(100)]
        public string Kategori { get; set; } = string.Empty;

        [Required]
        public ArizaDurumu Durum { get; set; } = ArizaDurumu.Beklemede;

        [Required]
        public OncelikDurumu OncelikDurumu { get; set; } = OncelikDurumu.Normal;

        [StringLength(1000)]
        public string? YapilanIslem { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TahminiMaliyet { get; set; }

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;

        public DateTime? GuncellemeTarihi { get; set; }

        public DateTime? TamamlanmaTarihi { get; set; }

        // Navigation Properties
        [ForeignKey("CihazId")]
        public virtual Cihaz? Cihaz { get; set; }

        [ForeignKey("KullaniciId")]
        public virtual Kullanici? AtananTeknisyen { get; set; }
    }
}
