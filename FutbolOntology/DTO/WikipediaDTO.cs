
using CsvHelper.Configuration;

public class WikipediaDTO
{
    public string ClubNombre { get; set; }
    public string UrlWikipedia { get; set; }
}

// Mapa para WikipediaDTO
public sealed class WikipediaDTOMap : ClassMap<WikipediaDTO>
{
    public WikipediaDTOMap()
    {
        Map(m => m.ClubNombre).Name("club_nombre");
        Map(m => m.UrlWikipedia).Name("url_wikipedia");
    }
}
