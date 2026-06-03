// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Helpers/KazancGaugeDrawable.cs
// Açıklama: Dairesel yüzde göstergesi (gauge). Hem Gelir hem Gider
//           sayfalarında ortak kullanılır.
// ============================================================

using Microsoft.Maui.Graphics;

namespace ArizaTakipSistemi.MAUI.Helpers
{
    public class KazancGaugeDrawable : IDrawable
    {
        public double Yuzde { get; set; } = 0; // 0..100

        // Renk şeması: false = düşük kırmızı, yüksek yeşil (gelir)
        //             true = düşük yeşil, yüksek kırmızı (gider)
        public bool TersRenk { get; set; } = false;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            float kalinlik = 16f;
            float pad = kalinlik / 2f + 6f;
            float x = dirtyRect.Left + pad;
            float y = dirtyRect.Top + pad;
            float cap = Math.Min(dirtyRect.Width, dirtyRect.Height) - pad * 2f;

            // Arka halka (gri tam çember)
            canvas.StrokeSize = kalinlik;
            canvas.StrokeColor = Color.FromArgb("#2a2a4a");
            canvas.DrawEllipse(x, y, cap, cap);

            double pct = Math.Max(0, Math.Min(Yuzde, 100));
            if (pct <= 0) return;

            Color renk;
            if (!TersRenk)
            {
                // Gelir: az dolu kötü → kırmızı, dolu iyi → yeşil
                renk = pct < 40 ? Color.FromArgb("#e94560")
                     : pct < 85 ? Color.FromArgb("#3498db")
                                : Color.FromArgb("#27ae60");
            }
            else
            {
                // Gider: az dolu iyi → yeşil, dolu kötü → kırmızı
                renk = pct < 40 ? Color.FromArgb("#27ae60")
                     : pct < 85 ? Color.FromArgb("#e67e22")
                                : Color.FromArgb("#e94560");
            }

            canvas.StrokeColor = renk;
            canvas.StrokeLineCap = LineCap.Round;

            if (pct >= 100)
            {
                canvas.DrawEllipse(x, y, cap, cap);
            }
            else
            {
                float sweep = (float)(360.0 * pct / 100.0);
                canvas.DrawArc(x, y, cap, cap, 90, 90 - sweep, true, false);
            }
        }
    }
}
