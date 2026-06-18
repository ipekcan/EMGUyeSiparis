public static class Sabitler
{
    public enum BedenTablosu
    {
        XXS,
        XS,
        S,
        M,
        L,
        XL,
        XXL,
        XXXL,
        Standart
    }
    public static readonly string[] KanGrubu = ["0 Rh(+)","A Rh(+)", "B Rh(+)", "AB Rh(+)","0 Rh(-)","A Rh(-)", "B Rh(-)", "AB Rh(-)"];
    public static readonly string[] Urunler = ["Reflektörlü Yelek", "Polar","Patch", "Polo Yaka T-shirt", "0 Yaka T-shirt", "Buff"];
    public static readonly Dictionary<string, decimal> UrunFiyatlari = new()
{
    { "Reflektörlü Yelek", 1400.00m },
    { "Polar", 1000.00m },
    { "Patch", 400.00m },
    { "Polo Yaka T-shirt", 700.00m },
    { "0 Yaka T-shirt", 500.00m },
    { "Buff", 50.00m }
};

}


