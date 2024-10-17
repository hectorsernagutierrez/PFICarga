
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class GameLineupsDTO
{
    public string GameLineupsId { get; set; }
    public DateTime Date { get; set; }
    public string GameId { get; set; }
    public string PlayerId { get; set; }
    public string ClubId { get; set; }
    public string PlayerName { get; set; }
    public string Type { get; set; }
    public string Position { get; set; }
    public string? NumberString { get; set; }
    public int? Number { get; set; }
    public bool TeamCaptain { get; set; }
}


// CSV Configuration for GameLineups


public sealed class GameLineupsDTOMap : ClassMap<GameLineupsDTO>
{
    public GameLineupsDTOMap()
    {
        Map(m => m.GameLineupsId).Name("game_lineups_id");
        Map(m => m.Date).Name("date");
        Map(m => m.GameId).Name("game_id");
        Map(m => m.PlayerId).Name("player_id");
        Map(m => m.ClubId).Name("club_id");
        Map(m => m.PlayerName).Name("player_name");
        Map(m => m.Type).Name("type");
        Map(m => m.Position).Name("position");
        Map(m => m.NumberString).Name("number");        
        Map(m => m.TeamCaptain).Name("team_captain");
    }
}


