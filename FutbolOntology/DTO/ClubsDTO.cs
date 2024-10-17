
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class ClubsDTO
{
    public int ClubId { get; set; }
    public string ClubName { get; set; }
    public string Country { get; set; }
    public int FoundedYear { get; set; }
}

// CSV Configuration for Clubs
public sealed class ClubsDTOMap : ClassMap<ClubsDTO>
{
    public ClubsDTOMap()
    {
        Map(m => m.ClubId).Name("club_id");
        Map(m => m.ClubName).Name("club_name");
        Map(m => m.Country).Name("country");
        Map(m => m.FoundedYear).Name("founded_year");
    }
}


