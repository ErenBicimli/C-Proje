// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Models/ArizaDto.cs
// Açıklama: Arıza veri transfer nesnesi (DTO).
// ============================================================

namespace ArizaTakipSistemi.MAUI.Models
{
    public class ArizaDto
    {
        public int ArizaId { get; set; }
        public int CihazId { get; set; }
        public int? KullaniciId { get; set; }
        public string ArizaTanimi { get; set; } = string.Empty;
        public string Kategori { get; set; } = string.Empty;
        public int Durum { get; set; }
        public int OncelikDurumu { get; set; }
        public string? YapilanIslem { get; set; }
        public decimal? TahminiMaliyet { get; set; }
        public DateTime OlusturulmaTarihi { get; set; }
        public DateTime? GuncellemeTarihi { get; set; }
        public DateTime? TamamlanmaTarihi { get; set; }

        // Navigation (API'den gelen iç içe nesneler)
        public CihazDto? Cihaz { get; set; }
        public KullaniciDto? AtananTeknisyen { get; set; }

        // Görüntüleme yardımcıları
        public string DurumAdi => Durum switch
        {
            0 => "Beklemede",
            1 => "Devam Ediyor",
            2 => "Tamamlandı",
            3 => "İptal Edildi",
            _ => "Bilinmiyor"
        };

        public string OncelikAdi => OncelikDurumu switch
        {
            0 => "Düşük",
            1 => "Normal",
            2 => "Yüksek",
            3 => "Acil",
            _ => "Bilinmiyor"
        };

        public Color DurumRengi => Durum switch
        {
            0 => Colors.Orange,
            1 => Colors.Blue,
            2 => Colors.Green,
            3 => Colors.Red,
            _ => Colors.Gray
        };

        public Color OncelikRengi => OncelikDurumu switch
        {
            0 => Colors.LightGray,
            1 => Colors.DodgerBlue,
            2 => Colors.Orange,
            3 => Colors.Red,
            _ => Colors.Gray
        };
    }
}
