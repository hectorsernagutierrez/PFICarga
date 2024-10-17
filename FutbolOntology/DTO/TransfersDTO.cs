
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class TransfersDTO
{
    public int TransferId { get; set; }
    public int PlayerId { get; set; }
    public int FromClubId { get; set; }
    public int ToClubId { get; set; }
    public decimal TransferFee { get; set; }
    public string TransferDate { get; set; }
}

// CSV Configuration for Transfers
public sealed class TransfersDTOMap : ClassMap<TransfersDTO>
{
    public TransfersDTOMap()
    {
        Map(m => m.TransferId).Name("transfer_id");
        Map(m => m.PlayerId).Name("player_id");
        Map(m => m.FromClubId).Name("from_club_id");
        Map(m => m.ToClubId).Name("to_club_id");
        Map(m => m.TransferFee).Name("transfer_fee");
        Map(m => m.TransferDate).Name("transfer_date");
    }
}


