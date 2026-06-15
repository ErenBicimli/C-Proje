// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Models/Cihaz.cs
// Açıklama: Cihaz entity sınıfı (Code-First).
//           Teknik servise getirilen cihazları temsil eder.
// ============================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArizaTakipSistemi.API.Models
{
    /// <summary>
    /// Teknik servise teslim edilen cihazları temsil eden entity sınıfı.
    /// </summary>
    [Table("Cihazlar")]
    public class Cihaz
    {
        [Key]
        public int CihazId { get; set; }

        [Required(ErrorMessage = "Marka alanı zorunludur.")]
        [StringLength(50, ErrorMessage = "Marka en fazla 50 karakter olabilir.")]
        public string Marka { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Model en fazla 100 karakter olabilir.")]
        public string Model { get; set; } = string.Empty;

        [Required(ErrorMessage = "Seri numarası zorunludur.")]
        [StringLength(100, ErrorMessage = "Seri numarası en fazla 100 karakter olabilir.")]
        public string SeriNumarasi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cihaz türü zorunludur.")]
        [StringLength(50, ErrorMessage = "Cihaz türü en fazla 50 karakter olabilir.")]
        public string CihazTuru { get; set; } = string.Empty;

        [Required(ErrorMessage = "Müşteri adı zorunludur.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Müşteri adı 2-100 karakter arasında olmalıdır.")]
        public string MusteriAdi { get; set; } = string.Empty;

        [Required(ErrorMessage = "Müşteri telefon numarası zorunludur.")]
        [StringLength(20)]
        public string MusteriTelefon { get; set; } = string.Empty;

        [StringLength(200)]
        public string? MusteriAdres { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? MusteriEmail { get; set; }

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;

        // Navigation Properties
        /// <summary>
        /// Bu cihaza ait arıza kayıtları.
        /// </summary>
        public virtual ICollection<Ariza> Arizalar { get; set; } = new List<Ariza>();
    }
}
