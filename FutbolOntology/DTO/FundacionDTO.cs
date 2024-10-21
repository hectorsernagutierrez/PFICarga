
using CsvHelper.Configuration;

public class FundacionDTO
{
    public string ClubNombre { get; set; }
    public DateTime FechaFundacion { get; set; }
}

// Mapa para FundacionDTO
public sealed class FundacionDTOMap : ClassMap<FundacionDTO>
{
    public FundacionDTOMap()
    {
        Map(m => m.ClubNombre).Name("club_nombre");
        Map(m => m.FechaFundacion).Name("fecha_fundacion");
    }
}
