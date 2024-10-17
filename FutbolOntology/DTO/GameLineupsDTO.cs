
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class GameLineupsDTO
{
    public int GameId { get; set; }
    public int PlayerId { get; set; }
    public string Position { get; set; }
    public bool IsStartingPlayer { get; set; }
}

// CSV Configuration for GameLineups
public sealed class GameLineupsDTOMap : ClassMap<GameLineupsDTO>
{
    public GameLineupsDTOMap()
    {
        Map(m => m.GameId).Name("game_id");
        Map(m => m.PlayerId).Name("player_id");
        Map(m => m.Position).Name("position");
        Map(m => m.IsStartingPlayer).Name("is_starting_player");
    }
}


