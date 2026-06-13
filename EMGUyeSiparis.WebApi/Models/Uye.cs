namespace EMGUyeSiparis.WebApi.Models
{
    public class Uye
    {
        public int Id { get; set; }
        public string Isim { get; set; } = string.Empty;
        public string Unvan { get; set; } = string.Empty;

        public string? KanGrubu { get; set; }
    }
}
