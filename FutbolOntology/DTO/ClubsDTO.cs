
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class ClubsDTO
{
    public int ClubId { get; set; }
    public string ClubCode { get; set; }
    public string Name { get; set; }
    public string DomesticCompetitionId { get; set; }
    public long TotalMarketValue { get; set; }
    public int SquadSize { get; set; }
    public double AverageAge { get; set; }
    public int ForeignersNumber { get; set; }
    public double ForeignersPercentage { get; set; }
    public int NationalTeamPlayers { get; set; }
    public string StadiumName { get; set; }
    public int StadiumSeats { get; set; }
    public string NetTransferRecord { get; set; }
    public string CoachName { get; set; }
    public string LastSeason { get; set; }
    public string Filename { get; set; }
    public string Url { get; set; }
}


// CSV Configuration for Clubs


public sealed class ClubsDTOMap : ClassMap<ClubsDTO>
{
    public ClubsDTOMap()
    {
        Map(m => m.ClubId).Name("club_id");
        Map(m => m.ClubCode).Name("club_code");
        Map(m => m.Name).Name("name");
        Map(m => m.DomesticCompetitionId).Name("domestic_competition_id");
        Map(m => m.TotalMarketValue).Name("total_market_value");
        Map(m => m.SquadSize).Name("squad_size");
        Map(m => m.AverageAge).Name("average_age");
        Map(m => m.ForeignersNumber).Name("foreigners_number");
        Map(m => m.ForeignersPercentage).Name("foreigners_percentage");
        Map(m => m.NationalTeamPlayers).Name("national_team_players");
        Map(m => m.StadiumName).Name("stadium_name");
        Map(m => m.StadiumSeats).Name("stadium_seats");
        Map(m => m.NetTransferRecord).Name("net_transfer_record");
        Map(m => m.CoachName).Name("coach_name");
        Map(m => m.LastSeason).Name("last_season");
        Map(m => m.Filename).Name("filename");
        Map(m => m.Url).Name("url");
    }
}



