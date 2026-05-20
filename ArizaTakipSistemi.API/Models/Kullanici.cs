// ============================================================
// Sorumlu Geliştirici: BERK (Berk_Backend)
// Dosya: Models/Kullanici.cs
// Açıklama: Kullanıcı entity sınıfı (Code-First).
//           Teknisyen veya Yönetici rolüne sahip kullanıcıları temsil eder.
// ============================================================

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ArizaTakipSistemi.API.Models
{
    /// <summary>
    /// Sisteme giriş yapan kullanıcıları (Teknisyen/Yönetici) temsil eden entity sınıfı.
    /// </summary>
    [Table("Kullanicilar")]
    public class Kullanici
    {
        [Key]
        public int KullaniciId { get; set; }

        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Ad 2-50 karakter arasında olmalıdır.")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Soyad 2-50 karakter arasında olmalıdır.")]
        public string Soyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [StringLength(256, MinimumLength = 6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Sifre { get; set; } = string.Empty;

        [Required]
        public KullaniciRolu Rol { get; set; } = KullaniciRolu.Teknisyen;

        [StringLength(20)]
        public string? Telefon { get; set; }

        public bool AktifMi { get; set; } = true;

        public DateTime OlusturulmaTarihi { get; set; } = DateTime.Now;

        public DateTime? SonGirisTarihi { get; set; }

        // Navigation Properties
        /// <summary>
        /// Bu kullanıcıya (teknisyene) atanan arızalar.
        /// </summary>
        public virtual ICollection<Ariza> AtananArizalar { get; set; } = new List<Ariza>();

        /// <summary>
        /// Bu kullanıcının gerçekleştirdiği log kayıtları.
        /// </summary>
        public virtual ICollection<Log> Loglar { get; set; } = new List<Log>();
    }
}
