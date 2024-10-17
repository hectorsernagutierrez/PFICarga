
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class TransfersDTO
{
    public int PlayerId { get; set; }
    public DateTime TransferDate { get; set; }
    public string TransferSeason { get; set; }
    public int FromClubId { get; set; }
    public int ToClubId { get; set; }
    public string FromClubName { get; set; }
    public string ToClubName { get; set; }
    public long TransferFee { get; set; }
    public long MarketValueInEur { get; set; }
    public string PlayerName { get; set; }
}


// CSV Configuration for Transfers


public sealed class TransfersDTOMap : ClassMap<TransfersDTO>
{
    public TransfersDTOMap()
    {
        Map(m => m.PlayerId).Name("player_id");
        Map(m => m.TransferDate).Name("transfer_date");
        Map(m => m.TransferSeason).Name("transfer_season");
        Map(m => m.FromClubId).Name("from_club_id");
        Map(m => m.ToClubId).Name("to_club_id");
        Map(m => m.FromClubName).Name("from_club_name");
        Map(m => m.ToClubName).Name("to_club_name");
        Map(m => m.TransferFee).Name("transfer_fee");
        Map(m => m.MarketValueInEur).Name("market_value_in_eur");
        Map(m => m.PlayerName).Name("player_name");
    }
}



