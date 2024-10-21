
using CsvHelper.Configuration;

public class NombresAlternativosDTO
{
    public string ClubNombre { get; set; }
    public List<string> NombresAlternativos { get; set; }
}

// Mapa para NombresAlternativosDTO
public sealed class NombresAlternativosDTOMap : ClassMap<NombresAlternativosDTO>
{
    public NombresAlternativosDTOMap()
    {
        Map(m => m.ClubNombre).Name("club_nombre");
        Map(m => m.NombresAlternativos).Name("nombres_alternativos");
    }
}
