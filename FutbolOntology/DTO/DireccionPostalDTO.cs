
using CsvHelper.Configuration;

public class DireccionPostalDTO
{
    public string ClubNombre { get; set; }
    public string Calle { get; set; }
    public string CodigoPostal { get; set; }
    public string Ciudad { get; set; }
    public string Pais { get; set; }
    public string Estadio { get; set; }
}

// Mapa para DireccionPostalDTO
public sealed class DireccionPostalDTOMap : ClassMap<DireccionPostalDTO>
{
    public DireccionPostalDTOMap()
    {
        Map(m => m.ClubNombre).Name("club_nombre");
        Map(m => m.Calle).Name("calle");
        Map(m => m.CodigoPostal).Name("codigo_postal");
        Map(m => m.Ciudad).Name("ciudad");
        Map(m => m.Pais).Name("pais");
        Map(m => m.Estadio).Name("estadio");
    }
}
