// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Models/KullaniciDto.cs
// Açıklama: Kullanıcı veri transfer nesnesi (DTO).
// ============================================================

namespace ArizaTakipSistemi.MAUI.Models
{
    public class KullaniciDto
    {
        public int KullaniciId { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefon { get; set; }
        public int Rol { get; set; }
        public bool AktifMi { get; set; }
        public DateTime OlusturulmaTarihi { get; set; }
        public DateTime? SonGirisTarihi { get; set; }

        public string TamAd => $"{Ad} {Soyad}";
        public string RolAdi => Rol == 1 ? "Yönetici" : "Teknisyen";
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Sifre { get; set; } = string.Empty;
    }

    public class SifreDegistirDto
    {
        public string EskiSifre { get; set; } = string.Empty;
        public string YeniSifre { get; set; } = string.Empty;
    }
}
