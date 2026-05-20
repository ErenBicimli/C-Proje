// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Models/CihazDto.cs
// Açıklama: Cihaz veri transfer nesnesi (DTO).
// ============================================================

namespace ArizaTakipSistemi.MAUI.Models
{
    public class CihazDto
    {
        public int CihazId { get; set; }
        public string Marka { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SeriNumarasi { get; set; } = string.Empty;
        public string CihazTuru { get; set; } = string.Empty;
        public string MusteriAdi { get; set; } = string.Empty;
        public string MusteriTelefon { get; set; } = string.Empty;
        public string? MusteriAdres { get; set; }
        public string? MusteriEmail { get; set; }
        public DateTime OlusturulmaTarihi { get; set; }

        public string CihazBilgisi => $"{Marka} {Model}";
    }
}
