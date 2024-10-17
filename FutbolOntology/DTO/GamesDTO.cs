
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class GamesDTO
{
    public string GameId { get; set; }
    public string CompetitionId { get; set; }
    public string Season { get; set; }
    public string Round { get; set; }
    public DateTime Date { get; set; }
    public string HomeClubId { get; set; }
    public string AwayClubId { get; set; }
    public int HomeClubGoals { get; set; }
    public int AwayClubGoals { get; set; }
    public string HomeClubPosition { get; set; }
    public string AwayClubPosition { get; set; }
    public string HomeClubManagerName { get; set; }
    public string AwayClubManagerName { get; set; }
    public string Stadium { get; set; }
    public int? Attendance { get; set; }
    public string Referee { get; set; }
    public string Url { get; set; }
    public string HomeClubFormation { get; set; }
    public string AwayClubFormation { get; set; }
    public string HomeClubName { get; set; }
    public string AwayClubName { get; set; }
    public string Aggregate { get; set; }
    public string CompetitionType { get; set; }
}


// CSV Configuration for Games


public sealed class GamesDTOMap : ClassMap<GamesDTO>
{
    public GamesDTOMap()
    {
        Map(m => m.GameId).Name("game_id");
        Map(m => m.CompetitionId).Name("competition_id");
        Map(m => m.Season).Name("season");
        Map(m => m.Round).Name("round");
        Map(m => m.Date).Name("date");
        Map(m => m.HomeClubId).Name("home_club_id");
        Map(m => m.AwayClubId).Name("away_club_id");
        Map(m => m.HomeClubGoals).Name("home_club_goals");
        Map(m => m.AwayClubGoals).Name("away_club_goals");
        Map(m => m.HomeClubPosition).Name("home_club_position");
        Map(m => m.AwayClubPosition).Name("away_club_position");
        Map(m => m.HomeClubManagerName).Name("home_club_manager_name");
        Map(m => m.AwayClubManagerName).Name("away_club_manager_name");
        Map(m => m.Stadium).Name("stadium");
        Map(m => m.Attendance).Name("attendance");
        Map(m => m.Referee).Name("referee");
        Map(m => m.Url).Name("url");
        Map(m => m.HomeClubFormation).Name("home_club_formation");
        Map(m => m.AwayClubFormation).Name("away_club_formation");
        Map(m => m.HomeClubName).Name("home_club_name");
        Map(m => m.AwayClubName).Name("away_club_name");
        Map(m => m.Aggregate).Name("aggregate");
        Map(m => m.CompetitionType).Name("competition_type");
    }
}

