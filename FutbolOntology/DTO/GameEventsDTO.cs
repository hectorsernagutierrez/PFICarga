
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class GameEventsDTO
{
    public int EventId { get; set; }
    public int GameId { get; set; }
    public int PlayerId { get; set; }
    public string EventType { get; set; }
    public int Minute { get; set; }
}

// CSV Configuration for GameEvents
public sealed class GameEventsDTOMap : ClassMap<GameEventsDTO>
{
    public GameEventsDTOMap()
    {
        Map(m => m.EventId).Name("event_id");
        Map(m => m.GameId).Name("game_id");
        Map(m => m.PlayerId).Name("player_id");
        Map(m => m.EventType).Name("event_type");
        Map(m => m.Minute).Name("minute");
    }
}
