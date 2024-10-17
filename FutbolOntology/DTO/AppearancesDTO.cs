
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using static Gnoss.ApiWrapper.ApiModel.SparqlObject;

public class AppearancesDTO
{
    public int PlayerId { get; set; }
    public int GameId { get; set; }
    public int MinutesPlayed { get; set; }
    //public bool WasSubstitute { get; set; }
    //public bool WasCaptain { get; set; }
    public string appearanceid { get; set; }
    public string playerClubId { get; set; }
    public string playerCurrentClubId { get; set; }
    public DateTime date { get; set; }
    public string playerName { get; set; }
    public string competitionId { get; set; }
    public string yellowCards { get; set; }
    public string appearance_id { get; set; }
    public string appearance_id { get; set; }
    public string appearance_id { get; set; }
    public string appearance_id { get; set; }
}

// CSV Configuration for Appearances
public sealed class AppearancesDTOMap : ClassMap<AppearancesDTO>
{
    public AppearancesDTOMap()
    {
        Map(m => m.PlayerId).Name("player_id");
        Map(m => m.GameId).Name("game_id");
        Map(m => m.MinutesPlayed).Name("minutes_played");
        Map(m => m.appearanceid).Name("appearance_id");
        Map(m => m.playerClubId).Name("player_club_id");
        Map(m => m.playerCurrentClubId).Name("player_current_club_id");
        Map(m => m.date).Name("date");
        Map(m => m.playerName).Name("player_name");
        Map(m => m.playerName).Name("competition_id");
        Map(m => m.playerName).Name("yellow_cards");
        Map(m => m.playerName).Name("red_cards");
        Map(m => m.playerName).Name("goals");
        Map(m => m.playerName).Name("assists");
        Map(m => m.playerName).Name("minutes_played");

        
    }

    
}
