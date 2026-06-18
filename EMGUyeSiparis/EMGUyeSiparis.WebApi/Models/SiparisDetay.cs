using System.ComponentModel.DataAnnotations;

public class SiparisDetay
{
    public int Id { get; set; }

    public int SiparisOzetId { get; set; }

    public required string Urun {get; set;}
    public int Adet {get; set;}
    public required string Beden {get; set;}

    public string? KanGrubu {get; set;}
}