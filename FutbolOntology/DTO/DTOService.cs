using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutbolOntology.DTO
{
    internal class DTOService
    {
        public List<AppearancesDTO> ReadAppearances(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<AppearancesDTOMap>();
                var records = csv.GetRecords<AppearancesDTO>();
                return new List<AppearancesDTO>(records);
            }
        }

        public List<ClubGamesDTO> ReadClubGames(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<ClubGamesDTOMap>();
                var records = csv.GetRecords<ClubGamesDTO>();
                return new List<ClubGamesDTO>(records);
            }
        }

        public List<CompetitionsDTO> ReadCompetitions(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CompetitionsDTOMap>();
                var records = csv.GetRecords<CompetitionsDTO>();
                return new List<CompetitionsDTO>(records);
            }
        }
        public List<ClubsDTO> ReadClubs(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<ClubsDTOMap>();
                var records = csv.GetRecords<ClubsDTO>();
                return new List<ClubsDTO>(records);
            }
        }

        public List<GameEventsDTO> ReadGameEvents(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<GameEventsDTOMap>();
                var records = csv.GetRecords<GameEventsDTO>();
                return new List<GameEventsDTO>(records);
            }
        }

        public List<GameLineupsDTO> ReadGameLineups(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<GameLineupsDTOMap>();
                var records = csv.GetRecords<GameLineupsDTO>();
                return new List<GameLineupsDTO>(records);
            }
        }

        public List<GamesDTO> ReadGames(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<GamesDTOMap>();
                var records = csv.GetRecords<GamesDTO>();
                return new List<GamesDTO>(records);
            }
        }

        // Method to Read and Parse CSV into DTOs
        public List<PlayerDTO> ReadPlayers(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture)) // Fully qualified CsvReader
            {
                csv.Context.RegisterClassMap<PlayerDTOMap>();
                var records = csv.GetRecords<PlayerDTO>();
                return new List<PlayerDTO>(records);
            }
        }

        // Method to Read and Parse CSV into DTOs
        public List<PlayerValuationDTO> ReadPlayerValuations(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture)) // Fully qualified CsvReader
            {
                csv.Context.RegisterClassMap<PlayerValuationDTOMap>();
                var records = csv.GetRecords<PlayerValuationDTO>();
                return new List<PlayerValuationDTO>(records);
            }
        }

        public List<TransfersDTO> ReadTransfers(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<TransfersDTOMap>();
                var records = csv.GetRecords<TransfersDTO>();
                return new List<TransfersDTO>(records);
            }
        }

    }
}
