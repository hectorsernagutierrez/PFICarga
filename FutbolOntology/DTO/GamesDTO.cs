
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class GamesDTO
{
    public int GameId { get; set; }
    public string Date { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public int HomeGoals { get; set; }
    public int AwayGoals { get; set; }
}

// CSV Configuration for Games
public sealed class GamesDTOMap : ClassMap<GamesDTO>
{
    public GamesDTOMap()
    {
        Map(m => m.GameId).Name("game_id");
        Map(m => m.Date).Name("date");
        Map(m => m.HomeTeamId).Name("home_team_id");
        Map(m => m.AwayTeamId).Name("away_team_id");
        Map(m => m.HomeGoals).Name("home_goals");
        Map(m => m.AwayGoals).Name("away_goals");
    }
}

