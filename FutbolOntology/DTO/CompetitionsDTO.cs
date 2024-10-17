
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class CompetitionsDTO
{
    public int CompetitionId { get; set; }
    public string CompetitionName { get; set; }
    public string Country { get; set; }
    public string Type { get; set; }
}

// CSV Configuration for Competitions
public sealed class CompetitionsDTOMap : ClassMap<CompetitionsDTO>
{
    public CompetitionsDTOMap()
    {
        Map(m => m.CompetitionId).Name("competition_id");
        Map(m => m.CompetitionName).Name("competition_name");
        Map(m => m.Country).Name("country");
        Map(m => m.Type).Name("type");
    }
}


