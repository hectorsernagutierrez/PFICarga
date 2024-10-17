using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class PlayersDTO
{
    public string PlayerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Name { get; set; }
    public string LastSeason { get; set; }
    public int CurrentClubId { get; set; }
    public string PlayerCode { get; set; }
    public string CountryOfBirth { get; set; }
    public string CityOfBirth { get; set; }
    public string CountryOfCitizenship { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string SubPosition { get; set; }
    public string Position { get; set; }
    public string Foot { get; set; }
    public int? HeightInCm { get; set; }
    public DateTime? ContractExpirationDate { get; set; }
    public string AgentName { get; set; }
    public string ImageUrl { get; set; }
    public string Url { get; set; }
    public string CurrentClubDomesticCompetitionId { get; set; }
    public string CurrentClubName { get; set; }
    public long? MarketValueInEur { get; set; }
    public long? HighestMarketValueInEur { get; set; }
}


// CSV Configuration for Player


public sealed class PlayersDTOMap : ClassMap<PlayersDTO>
{
    public PlayersDTOMap()
    {
        Map(m => m.PlayerId).Name("player_id");
        Map(m => m.FirstName).Name("first_name");
        Map(m => m.LastName).Name("last_name");
        Map(m => m.Name).Name("name");
        Map(m => m.LastSeason).Name("last_season");
        Map(m => m.CurrentClubId).Name("current_club_id");
        Map(m => m.PlayerCode).Name("player_code");
        Map(m => m.CountryOfBirth).Name("country_of_birth");
        Map(m => m.CityOfBirth).Name("city_of_birth");
        Map(m => m.CountryOfCitizenship).Name("country_of_citizenship");
        Map(m => m.DateOfBirth).Name("date_of_birth");
        Map(m => m.SubPosition).Name("sub_position");
        Map(m => m.Position).Name("position");
        Map(m => m.Foot).Name("foot");
        Map(m => m.HeightInCm).Name("height_in_cm");
        Map(m => m.ContractExpirationDate).Name("contract_expiration_date");
        Map(m => m.AgentName).Name("agent_name");
        Map(m => m.ImageUrl).Name("image_url");
        Map(m => m.Url).Name("url");
        Map(m => m.CurrentClubDomesticCompetitionId).Name("current_club_domestic_competition_id");
        Map(m => m.CurrentClubName).Name("current_club_name");
        Map(m => m.MarketValueInEur).Name("market_value_in_eur");
        Map(m => m.HighestMarketValueInEur).Name("highest_market_value_in_eur");
    }
}

