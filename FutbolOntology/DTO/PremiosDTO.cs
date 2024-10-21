
using CsvHelper.Configuration;

public class PremiosDTO
{
    public string ClubNombre { get; set; }
    public List<string> Premios { get; set; }
}

// Mapa para PremiosDTO
public sealed class PremiosDTOMap : ClassMap<PremiosDTO>
{
    public PremiosDTOMap()
    {
        Map(m => m.ClubNombre).Name("club_nombre");
        Map(m => m.Premios).Name("premios");
    }
}
