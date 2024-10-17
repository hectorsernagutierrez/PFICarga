﻿using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class PlayerValuationDTO
{
    public int PlayerId { get; set; }
    public string Date { get; set; }
    public decimal MarketValueInEur { get; set; }
    public int CurrentClubId { get; set; }
    public string PlayerClubDomesticCompetitionId { get; set; }
}

// CSV Configuration for PlayerValuation
public sealed class PlayerValuationDTOMap : ClassMap<PlayerValuationDTO>
{
    public PlayerValuationDTOMap()
    {
        Map(m => m.PlayerId).Name("player_id");
        Map(m => m.Date).Name("date");
        Map(m => m.MarketValueInEur).Name("market_value_in_eur");
        Map(m => m.CurrentClubId).Name("current_club_id");
        Map(m => m.PlayerClubDomesticCompetitionId).Name("player_club_domestic_competition_id");
    }
}


