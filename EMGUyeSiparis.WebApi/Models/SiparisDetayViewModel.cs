namespace EMGUyeSiparis.WebApi.Models
{
    public class SiparisDetayViewModel
    {
        public int SiparisDetayId { get; set; }
        public string Urun { get; set; } = string.Empty;
        public int Adet { get; set; }
        public string Beden { get; set; } = string.Empty;
        public string KanGrubu { get; set; } = string.Empty;
        public decimal BirimFiyat { get; set; }
        
        // Sadece bu ürüne ait toplam tutar
        public decimal ToplamTutar => BirimFiyat * Adet;
    }
}