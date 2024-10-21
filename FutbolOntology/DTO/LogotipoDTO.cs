
using CsvHelper.Configuration;

public class LogotipoDTO
{
    public string ClubNombre { get; set; }
    public string UrlLogotipo { get; set; }
}

// Mapa para LogotipoDTO
public sealed class LogotipoDTOMap : ClassMap<LogotipoDTO>
{
    public LogotipoDTOMap()
    {
        Map(m => m.ClubNombre).Name("club_nombre");
        Map(m => m.UrlLogotipo).Name("url_logotipo");
    }
}
