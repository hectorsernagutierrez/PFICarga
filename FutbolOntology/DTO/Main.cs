
using FutbolOntology.DTO;
using System;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        // Paths to CSV files (update paths if needed)
        string appearancesFilePath = "path/to/appearances.csv";
        string clubGamesFilePath = "path/to/club_games.csv";
        string clubsFilePath = "path/to/clubs.csv";
        string competitionsFilePath = "path/to/competitions.csv";
        string gameEventsFilePath = "path/to/game_events.csv";
        string gameLineupsFilePath = "path/to/game_lineups.csv";
        string gamesFilePath = "path/to/games.csv";
        string playerValuationsFilePath = "path/to/player_valuations.csv";
        string playersFilePath = "path/to/players.csv";
        string transfersFilePath = "path/to/transfers.csv";

        // Read and display Appearances
        var service = new DTOService();
        List<AppearancesDTO> appearances = service.ReadAppearances(appearancesFilePath);
        Console.WriteLine("=== Appearances ===");
        foreach (var appearance in appearances)
        {
            Console.WriteLine($"Player ID: {appearance.PlayerId}, Game ID: {appearance.GameId}, Minutes Played: {appearance.MinutesPlayed}, Was Substitute: {appearance.WasSubstitute}, Was Captain: {appearance.WasCaptain}");
        }

        // Read and display Club Games
        List<ClubGamesDTO> clubGames = service.ReadClubGames(clubGamesFilePath);
        Console.WriteLine("\n=== Club Games ===");
        foreach (var clubGame in clubGames)
        {
            Console.WriteLine($"Club ID: {clubGame.ClubId}, Game ID: {clubGame.GameId}, Goals Scored: {clubGame.GoalsScored}, Goals Conceded: {clubGame.GoalsConceded}");
        }

        // Read and display Clubs
        List<ClubsDTO> clubs = service.ReadClubs(clubsFilePath);
        Console.WriteLine("\n=== Clubs ===");
        foreach (var club in clubs)
        {
            Console.WriteLine($"Club ID: {club.ClubId}, Name: {club.ClubName}, Country: {club.Country}, Founded Year: {club.FoundedYear}");
        }

        // Read and display Competitions
        List<CompetitionsDTO> competitions = service.ReadCompetitions(competitionsFilePath);
        Console.WriteLine("\n=== Competitions ===");
        foreach (var competition in competitions)
        {
            Console.WriteLine($"Competition ID: {competition.CompetitionId}, Name: {competition.CompetitionName}, Country: {competition.Country}, Type: {competition.Type}");
        }

        // Read and display Game Events
        List<GameEventsDTO> gameEvents = service.ReadGameEvents(gameEventsFilePath);
        Console.WriteLine("\n=== Game Events ===");
        foreach (var gameEvent in gameEvents)
        {
            Console.WriteLine($"Event ID: {gameEvent.EventId}, Game ID: {gameEvent.GameId}, Player ID: {gameEvent.PlayerId}, Event Type: {gameEvent.EventType}, Minute: {gameEvent.Minute}");
        }

        // Read and display Game Lineups
        List<GameLineupsDTO> gameLineups = service.ReadGameLineups(gameLineupsFilePath);
        Console.WriteLine("\n=== Game Lineups ===");
        foreach (var gameLineup in gameLineups)
        {
            Console.WriteLine($"Game ID: {gameLineup.GameId}, Player ID: {gameLineup.PlayerId}, Position: {gameLineup.Position}, Is Starting Player: {gameLineup.IsStartingPlayer}");
        }

        // Read and display Games
        List<GamesDTO> games = service.ReadGames(gamesFilePath);
        Console.WriteLine("\n=== Games ===");
        foreach (var game in games)
        {
            Console.WriteLine($"Game ID: {game.GameId}, Date: {game.Date}, Home Team ID: {game.HomeTeamId}, Away Team ID: {game.AwayTeamId}, Home Goals: {game.HomeGoals}, Away Goals: {game.AwayGoals}");
        }

        // Read and display Player Valuations
        List<PlayerValuationDTO> playerValuations = service.ReadPlayerValuations(playerValuationsFilePath);
        Console.WriteLine("\n=== Player Valuations ===");
        foreach (var valuation in playerValuations)
        {
            Console.WriteLine($"Player ID: {valuation.PlayerId}, Date: {valuation.Date}, Market Value: {valuation.MarketValueInEur}, Current Club ID: {valuation.CurrentClubId}, Competition ID: {valuation.PlayerClubDomesticCompetitionId}");
        }

        // Read and display Players
        List<PlayerDTO> players = service.ReadPlayers(playersFilePath);
        Console.WriteLine("\n=== Players ===");
        foreach (var player in players)
        {
            Console.WriteLine($"Player ID: {player.PlayerId}, Name: {player.Name}, Country of Birth: {player.CountryOfBirth}, Market Value: {player.MarketValueInEur}");
        }

        // Read and display Transfers
        List<TransfersDTO> transfers = service.ReadTransfers(transfersFilePath);
        Console.WriteLine("\n=== Transfers ===");
        foreach (var transfer in transfers)
        {
            Console.WriteLine($"Transfer ID: {transfer.TransferId}, Player ID: {transfer.PlayerId}, From Club ID: {transfer.FromClubId}, To Club ID: {transfer.ToClubId}, Transfer Fee: {transfer.TransferFee}, Transfer Date: {transfer.TransferDate}");
        }
    }
}
