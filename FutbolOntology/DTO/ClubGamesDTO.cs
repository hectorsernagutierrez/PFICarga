
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;


public class ClubGamesDTO
{
    public int GameId { get; set; }
    public int ClubId { get; set; }
    public int OwnGoals { get; set; }
    public string OwnPosition { get; set; }
    public string OwnManagerName { get; set; }
    public int OpponentId { get; set; }
    public int OpponentGoals { get; set; }
    public string OpponentPosition { get; set; }
    public string OpponentManagerName { get; set; }
    public string Hosting { get; set; }
    public bool IsWin { get; set; }
}


// CSV Configuration for ClubGames


public sealed class ClubGamesDTOMap : ClassMap<ClubGamesDTO>
{
    public ClubGamesDTOMap()
    {
        Map(m => m.GameId).Name("game_id");
        Map(m => m.ClubId).Name("club_id");
        Map(m => m.OwnGoals).Name("own_goals");
        Map(m => m.OwnPosition).Name("own_position");
        Map(m => m.OwnManagerName).Name("own_manager_name");
        Map(m => m.OpponentId).Name("opponent_id");
        Map(m => m.OpponentGoals).Name("opponent_goals");
        Map(m => m.OpponentPosition).Name("opponent_position");
        Map(m => m.OpponentManagerName).Name("opponent_manager_name");
        Map(m => m.Hosting).Name("hosting");
        Map(m => m.IsWin).Name("is_win");
    }
}

