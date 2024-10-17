
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;
public class GameEventsDTO
{
    public string GameEventId { get; set; }
    public DateTime Date { get; set; }
    public string GameId { get; set; }
    public int Minute { get; set; }
    public string Type { get; set; }
    public int ClubId { get; set; }
    public int PlayerId { get; set; }
    public string Description { get; set; }
    public int? PlayerInId { get; set; }
    public int? PlayerAssistId { get; set; }
}


// CSV Configuration for GameEvents


public sealed class GameEventsDTOMap : ClassMap<GameEventsDTO>
{
    public GameEventsDTOMap()
    {
        Map(m => m.GameEventId).Name("game_event_id");
        Map(m => m.Date).Name("date");
        Map(m => m.GameId).Name("game_id");
        Map(m => m.Minute).Name("minute");
        Map(m => m.Type).Name("type");
        Map(m => m.ClubId).Name("club_id");
        Map(m => m.PlayerId).Name("player_id");
        Map(m => m.Description).Name("description");
        Map(m => m.PlayerInId).Name("player_in_id");
        Map(m => m.PlayerAssistId).Name("player_assist_id");
    }
}
