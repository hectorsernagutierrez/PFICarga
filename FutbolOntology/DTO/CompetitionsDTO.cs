
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

public class CompetitionsDTO
{
    public string CompetitionId { get; set; }
    public string CompetitionCode { get; set; }
    public string Name { get; set; }
    public string SubType { get; set; }
    public string Type { get; set; }
    public int CountryId { get; set; }
    public string CountryName { get; set; }
    public string DomesticLeagueCode { get; set; }
    public string Confederation { get; set; }
    public string Url { get; set; }
    public bool IsMajorNationalLeague { get; set; }
}


// CSV Configuration for Competitions


public sealed class CompetitionsDTOMap : ClassMap<CompetitionsDTO>
{
    public CompetitionsDTOMap()
    {
        Map(m => m.CompetitionId).Name("competition_id");
        Map(m => m.CompetitionCode).Name("competition_code");
        Map(m => m.Name).Name("name");
        Map(m => m.SubType).Name("sub_type");
        Map(m => m.Type).Name("type");
        Map(m => m.CountryId).Name("country_id");
        Map(m => m.CountryName).Name("country_name");
        Map(m => m.DomesticLeagueCode).Name("domestic_league_code");
        Map(m => m.Confederation).Name("confederation");
        Map(m => m.Url).Name("url");
        Map(m => m.IsMajorNationalLeague).Name("is_major_national_league");
    }
}

