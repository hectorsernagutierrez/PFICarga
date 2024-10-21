
using CsvHelper.Configuration;

public class EntrenadorDTO
{
    public string ClubNombre { get; set; }
    public string EntrenadorNombre { get; set; }
    public DateTime? FechaComienzo { get; set; }
    public DateTime? FechaFin { get; set; }
}

// Mapa para EntrenadorDTO
public sealed class EntrenadorDTOMap : ClassMap<EntrenadorDTO>
{
    public EntrenadorDTOMap()
    {
        Map(m => m.ClubNombre).Name("club_nombre");
        Map(m => m.EntrenadorNombre).Name("entrenador_nombre");
        Map(m => m.FechaComienzo).Name("fecha_comienzo");
        Map(m => m.FechaFin).Name("fecha_fin");
    }
}
