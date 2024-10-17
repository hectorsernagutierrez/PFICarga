
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class ClubGamesDTO
{
    public int ClubId { get; set; }
    public int GameId { get; set; }
    public int GoalsScored { get; set; }
    public int GoalsConceded { get; set; }
}

// CSV Configuration for ClubGames
public sealed class ClubGamesDTOMap : ClassMap<ClubGamesDTO>
{
    public ClubGamesDTOMap()
    {
        Map(m => m.ClubId).Name("club_id");
        Map(m => m.GameId).Name("game_id");
        Map(m => m.GoalsScored).Name("goals_scored");
        Map(m => m.GoalsConceded).Name("goals_conceded");
    }

    
}
