
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class AppearancesDTO
{
    public int PlayerId { get; set; }
    public int GameId { get; set; }
    public int MinutesPlayed { get; set; }
    public bool WasSubstitute { get; set; }
    public bool WasCaptain { get; set; }
}

// CSV Configuration for Appearances
public sealed class AppearancesDTOMap : ClassMap<AppearancesDTO>
{
    public AppearancesDTOMap()
    {
        Map(m => m.PlayerId).Name("player_id");
        Map(m => m.GameId).Name("game_id");
        Map(m => m.MinutesPlayed).Name("minutes_played");
        Map(m => m.WasSubstitute).Name("was_substitute");
        Map(m => m.WasCaptain).Name("was_captain");
    }

    
}
