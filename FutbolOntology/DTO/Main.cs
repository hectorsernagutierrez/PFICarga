
using FutbolOntology.DTO;
using System;
using System.Collections.Generic;


class Program
{
    static void Main(string[] args)
    {
        // Paths to CSV files (update paths if needed)
       
        
        string appearancesFile = @"Dataset/appearances.csv";
        string clubGamesFile = @"Dataset/club_games.csv";
        string clubsFile = @"Dataset/clubs.csv";
        string competitionsFile = @"Dataset/competitions.csv";
        string gameEventsFile = @"Dataset/game_events.csv";
        string gameLineupsFile = @"Dataset/game_lineups.csv";
        string gamesFile = @"Dataset/games.csv";
        string playerValuationsFile = @"Dataset/player_valuations.csv";
        string playersFile = @"Dataset/players.csv";
        string transfersFile = @"Dataset/transfers.csv";

        string appearancesFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, appearancesFile);
        string clubGamesFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, clubGamesFile);
        string clubsFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, clubsFile);
        string competitionsFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, competitionsFile);
        string gameEventsFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, gameEventsFile);
        string gameLineupsFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, gameLineupsFile);
        string gamesFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, gamesFile);
        string playerValuationsFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, playerValuationsFile);
        string playersFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, playersFile);
        string transfersFilePath = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, transfersFile);



        // Read and display Appearances
        var service = new DTOService();
        //List<AppearancesDTO> appearances = service.ReadAppearances(appearancesFilePath);
        //Console.WriteLine("=== Appearances ===");
        //foreach (var appearance in appearances)
        //{
        //    Console.WriteLine($"Player ID: {appearance.PlayerId}, Game ID: {appearance.GameId}, Minutes Played: {appearance.MinutesPlayed}");
        //}

        // Read and display Club Games
        List<ClubGamesDTO> clubGames = service.ReadClubGames(clubGamesFilePath);
        Console.WriteLine("\n=== Club Games ===");
        foreach (var clubGame in clubGames)
        {
            Console.WriteLine($"Club ID: {clubGame.ClubId}, Game ID: {clubGame.GameId}");
        }

        // Read and display Clubs
        List<ClubsDTO> clubs = service.ReadClubs(clubsFilePath);
        Console.WriteLine("\n=== Clubs ===");
        foreach (var club in clubs)
        {
            Console.WriteLine($"Club ID: {club.ClubId}");
        }

        // Read and display Competitions
        List<CompetitionsDTO> competitions = service.ReadCompetitions(competitionsFilePath);
        Console.WriteLine("\n=== Competitions ===");
        foreach (var competition in competitions)
        {
            Console.WriteLine($"Competition ID: {competition.CompetitionId}, Type: {competition.Type}");
        }

        // Read and display Game Events
        List<GameEventsDTO> gameEvents = service.ReadGameEvents(gameEventsFilePath);
        Console.WriteLine("\n=== Game Events ===");
        foreach (var gameEvent in gameEvents)
        {
            Console.WriteLine($"Event ID: {gameEvent.GameEventId}, Game ID: {gameEvent.GameId}, Player ID: {gameEvent.PlayerId}, Event Type: {gameEvent.Type}, Minute: {gameEvent.Minute}");
        }

        // Read and display Game Lineups
        List<GameLineupsDTO> gameLineups = service.ReadGameLineups(gameLineupsFilePath);
        Console.WriteLine("\n=== Game Lineups ===");
        foreach (var gameLineup in gameLineups)
        {
            Console.WriteLine($"Game ID: {gameLineup.GameId}, Player ID: {gameLineup.PlayerId}, Position: {gameLineup.Position}");
        }

        // Read and display Games
        List<GamesDTO> games = service.ReadGames(gamesFilePath);
        Console.WriteLine("\n=== Games ===");
        foreach (var game in games)
        {
            Console.WriteLine($"Game ID: {game.GameId}, Date: {game.Date}, Home Team ID: {game.HomeClubId}, Away Team ID: {game.AwayClubId}, Home Goals: {game.HomeClubGoals}, Away Goals: {game.AwayClubGoals}");
        }

        // Read and display Player Valuations
        List<PlayerValuationsDTO> playerValuations = service.ReadPlayerValuations(playerValuationsFilePath);
        Console.WriteLine("\n=== Player Valuations ===");
        foreach (var valuation in playerValuations)
        {
            Console.WriteLine($"Player ID: {valuation.PlayerId}, Date: {valuation.Date}, Market Value: {valuation.MarketValueInEur}, Current Club ID: {valuation.CurrentClubId}, Competition ID: {valuation.PlayerClubDomesticCompetitionId}");
        }

        // Read and display Players
        List<PlayersDTO> players = service.ReadPlayers(playersFilePath);
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
            Console.WriteLine($" Player ID: {transfer.PlayerId}, From Club ID: {transfer.FromClubId}, To Club ID: {transfer.ToClubId}, Transfer Fee: {transfer.TransferFee}, Transfer Date: {transfer.TransferDate}");
        }
    }
}
