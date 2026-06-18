using System.Collections.Generic;

namespace EMGUyeSiparis.WebApi.Models
{
    public class SiparisKayitViewModel
    {
        public int UyeId { get; set; }
        // Kullanıcının seçeceği sipariş tarihi alanı
        public DateOnly Tarih { get; set; } = DateOnly.FromDateTime(DateTime.Now); // Varsayılan olarak bugün gelir
        public List<SiparisDetayGirisModel> Urunler { get; set; } = new List<SiparisDetayGirisModel>();
    }

    public class SiparisDetayGirisModel
    {
        public string Urun { get; set; } = string.Empty;
        public int Adet { get; set; }
        public string Beden { get; set; } = string.Empty;
        public string? KanGrubu { get; set; }
    }
}
