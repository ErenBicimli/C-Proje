// ============================================================
// Sorumlu Geliştirici: SAİD (Said_CrudUI)
// Dosya: Helpers/MarkaModelData.cs
// Açıklama: Marka -> Model listesi. Cihaz/Arıza ekleme ekranlarında
//           Marka seçilince Model Picker'ı buradan dolduruyoruz.
// ============================================================

namespace ArizaTakipSistemi.MAUI.Helpers
{
    public static class MarkaModelData
    {
        public static readonly Dictionary<string, string[]> Modeller = new()
        {
            ["Apple"] = new[]
            {
                "iPhone 11", "iPhone 11 Pro", "iPhone 12", "iPhone 12 Pro",
                "iPhone 13", "iPhone 13 Pro", "iPhone 14", "iPhone 14 Pro",
                "iPhone 15", "iPhone 15 Pro", "iPhone 16", "iPhone 16 Pro",
                "iPad", "iPad Pro", "iPad Air",
                "MacBook Air", "MacBook Pro", "Diğer"
            },
            ["Samsung"] = new[]
            {
                "Galaxy S21", "Galaxy S22", "Galaxy S23", "Galaxy S24", "Galaxy S25",
                "Galaxy A52", "Galaxy A53", "Galaxy A54", "Galaxy A55",
                "Galaxy Note 20", "Galaxy Z Flip", "Galaxy Z Fold",
                "Galaxy Tab S8", "Galaxy Tab S9", "Diğer"
            },
            ["Xiaomi"] = new[]
            {
                "Redmi Note 11", "Redmi Note 12", "Redmi Note 13", "Redmi Note 14",
                "Mi 11", "Mi 12", "Mi 13",
                "Poco X5", "Poco X6", "Poco F5", "Poco F6", "Diğer"
            },
            ["Huawei"] = new[]
            {
                "P30", "P40", "P50", "P60",
                "Mate 40", "Mate 50", "Mate 60",
                "Y9", "Nova 9", "Nova 11", "Diğer"
            },
            ["Oppo"] = new[]
            {
                "Reno 8", "Reno 10", "Reno 11",
                "A78", "A98", "Find X5", "Find X6", "Diğer"
            },
            ["Vivo"] = new[]
            {
                "Y36", "Y22", "Y27",
                "V25", "V27", "V29",
                "X90", "X100", "Diğer"
            },
            ["HP"] = new[]
            {
                "Pavilion", "Probook", "Elitebook",
                "Omen 15", "Omen 16", "Envy", "Victus", "Diğer"
            },
            ["Dell"] = new[]
            {
                "XPS 13", "XPS 15", "XPS 17",
                "Inspiron 15", "Inspiron 16",
                "Latitude", "Vostro", "Alienware", "Diğer"
            },
            ["Lenovo"] = new[]
            {
                "ThinkPad X1", "ThinkPad T14", "ThinkPad E15",
                "IdeaPad 3", "IdeaPad 5",
                "Yoga", "Legion 5", "Legion 7", "Diğer"
            },
            ["Asus"] = new[]
            {
                "ZenBook", "VivoBook",
                "ROG Strix", "ROG Zephyrus",
                "TUF Gaming", "ExpertBook", "Diğer"
            },
            ["Acer"] = new[]
            {
                "Aspire 3", "Aspire 5", "Aspire 7",
                "Predator Helios", "Nitro 5", "Swift", "Diğer"
            },
            ["MSI"] = new[]
            {
                "GF63", "Katana", "Stealth", "Raider", "Modern", "Prestige", "Diğer"
            },
            ["Casper"] = new[]
            {
                "Excalibur G650", "Excalibur G770", "Excalibur G900",
                "Nirvana C600", "Nirvana C650", "Nirvana X500", "Diğer"
            },
            ["Monster"] = new[]
            {
                "Tulpar T5", "Tulpar T7", "Tulpar A7",
                "Abra A5", "Abra A7", "Diğer"
            },
            ["LG"] = new[]
            {
                "Gram 14", "Gram 16", "Gram 17",
                "Velvet", "V60", "Diğer"
            },
            ["Sony"] = new[]
            {
                "Xperia 1", "Xperia 5", "Xperia 10",
                "VAIO", "Diğer"
            },
            ["Microsoft"] = new[]
            {
                "Surface Pro", "Surface Laptop", "Surface Book", "Surface Go", "Diğer"
            },
            ["Diğer"] = new[] { "Diğer" }
        };

        // Bir markaya ait modelleri getirir. Marka tanımlı değilse sadece "Diğer" döner.
        public static string[] ModelleriGetir(string? marka)
        {
            if (!string.IsNullOrEmpty(marka) && Modeller.TryGetValue(marka, out var liste))
                return liste;
            return new[] { "Diğer" };
        }
    }
}
