using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using ConsoleSoccerApplication.Enums;
using ConsoleSoccerApplication.Services;
using ConsoleSoccerApplication.Exceptions;
using ConsoleSoccerApplication.Controllers;
using ConsoleSoccerApplication.Models.Dtos;
using ConsoleSoccerApplication.Data.Interfaces;
using ConsoleSoccerApplication.Data.Repositories;

namespace ConsoleSoccerApplication
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Cria instâncias dos CompetitionRepository, TeamRespository e  PlayerRepository (usando injeção de dependência)
                ICompetitionRepository competitionRepository = new CompetitionRepository();
                ITeamRepository teamRespository = new TeamRepository();
                IPlayerRepository playerRepository = new PlayerRepository();

                //Cria instância do ConfigurationService
                ConfigurationService configurationService = new ConfigurationService();

                //Cria instância do TranslationController, passando ConfigurationService como dependência necessária
                TranslationController translationController = new TranslationController(configurationService);

                // Cria instâncias dos CompetitionController, TeamController, PlayerController e MatchController, passando os CompetitionRepository, TeamRespository, PlayerRepository e ConfigurationService como as dependências necessárias
                CompetitionController competitionController = new CompetitionController(competitionRepository, translationController, configurationService);
                TeamController teamController = new TeamController(teamRespository, configurationService);
                PlayerController playerController = new PlayerController(playerRepository, configurationService);
                MatchController matchController = new MatchController(configurationService);

                //Cria instância do AppService
                AppService appService = new AppService();

                ////Busca todas as competições que possuem informações disponíveis
                //IEnumerable<CompetitionDTO> competitions = await competitionController.GetAllAvailableCompetitions();

                ////Obtém e salva as competições com informações disponíveis
                //await appService.GetAndSaveCompetitions(competitionController, competitions);

                ////Obtém e salva os times das competições
                //await appService.GetAndSaveTeams(competitionController, competitions, teamController);

                bool finish = false;

                Console.WriteLine("---------------------------------------------------------- INFORMAÇÕES E ESTATÍSTICAS DE FUTEBOL -----------------------------------------------------------");

                Console.WriteLine();

                //Roda o programa enquanto a variável finish não recebe true (ou seja, informando que deseja finalizar)
                while (finish != true)
                {
                    Console.WriteLine("---------------------------------------------------------------- INFORMAÇÕES DOS FAVORITOS -----------------------------------------------------------------");

                    string favoritesResponse = "";
                    string input = "";

                    Console.WriteLine();

                    //Rodada a parte de favoritos enquanto resposta de interesse nos favoritos for 'sim'
                    while (favoritesResponse.ToLower() == "sim" || favoritesResponse.ToLower().Equals("s") || input == "")
                    {
                        //Obtém interesse nos favoritos
                        favoritesResponse = appService.GetInterestInFavoritesOrNewInformationsOrScores(true);

                        input = "-";

                        if (favoritesResponse.ToLower() == "sim" || favoritesResponse.ToLower().Equals("s"))
                        {
                            //Obtém o interesse em jogador ou time dos favoritos
                            string informationResponse = appService.GetInterestPlayerOrTeamFavorites();

                            //Obtém todos o jogadores guardados nos favoritos
                            List<string> listPlayers = await playerController.GetAllFavoritePlayers();

                            //Executa o trecho de jogadores, caso o interesse seja jogador e tenha pelo menos um jogador salvo nos favoritos
                            if (informationResponse.ToLower() == "jogador" && listPlayers.Count > 0)
                            {
                                Console.WriteLine("Jogadores favoritos:");
                                foreach (var player in listPlayers)
                                {
                                    Console.WriteLine(player);
                                }

                                Console.WriteLine();

                                Console.WriteLine("De qual jogador acima deseja saber informações?");

                                string favoritePlayerName = Console.ReadLine();
                                Console.WriteLine();

                                //Obtém o jogador nos favoritos especificado acima
                                int? favoritePlayerId = await playerController.GetFavoritePlayer(favoritePlayerName);

                                if (favoritePlayerId == null || favoritePlayerId == 0)
                                {
                                    Console.WriteLine("Não foram encontradas informações desse jogador nos seus favoritos.");
                                    Console.WriteLine();
                                }
                                else
                                {
                                    //Obtém as informações específicas do jogador e as informações dos jogos desse jogador
                                    PlayerInfoDTO favoritePlayerInformations = await playerController.GetPlayerInformation(favoritePlayerId.Value);
                                    PlayerMatchesDTO favoritePlayerMatchesInformations = await playerController.GetPlayerMatches(favoritePlayerId.Value);

                                    Console.WriteLine($"{favoritePlayerInformations.Name}");
                                    Console.WriteLine($"Primeiro nome: {favoritePlayerInformations.FirstName}");
                                    Console.WriteLine($"Sobrenome nome: {favoritePlayerInformations.LastName}");
                                    Console.WriteLine($"Data de nascimento: {favoritePlayerInformations.DateOfBirth:dd/MM/yyyy} ({favoritePlayerInformations.Age} anos)");
                                    Console.WriteLine($"Nacionalidade: {translationController.TranslateText(favoritePlayerInformations.Nationality, "pt").Result}");
                                    Console.WriteLine($"Posição: {(appService.GetEnumDescription(typeof(EnumPlayerPosition).GetField(favoritePlayerInformations.Position != null ? favoritePlayerInformations.Position.Replace("-", string.Empty).Replace(" ", string.Empty) : "-")))} - Seção: {(appService.GetEnumDescription(typeof(EnumPlayerPosition).GetField(favoritePlayerInformations.Section ?? "-")))}");
                                    Console.WriteLine($"Número: {favoritePlayerInformations.ShirtNumber}");
                                    Console.WriteLine($"Time atual: {favoritePlayerInformations.CurrentTeam.Name} ({favoritePlayerInformations.CurrentTeam.ShortName}) - País: {translationController.TranslateText(favoritePlayerInformations.CurrentTeam.Nationality, "pt").Result}");
                                    Console.WriteLine($"Contrato: {favoritePlayerInformations.Contract.Start.Value:dd/MM/yyyy} até {favoritePlayerInformations.Contract.Until.Value:dd/MM/yyyy}");

                                    //Transforma as competições que estão em um formato de bloco string em uma lista de strings
                                    List<string> listCompetitionsCodes = favoritePlayerMatchesInformations.Competitions?.Split(',').ToList();

                                    //Lista as competições do jogador
                                    string stringCompetitions = listCompetitionsCodes != null ? await competitionController.ListPlayerCompetitions(listCompetitionsCodes) : "-";

                                    Console.WriteLine($"Competições: {stringCompetitions}");

                                    Console.WriteLine();

                                    await appService.DeleteDataFromFavorites(playerController, null, favoritePlayerId.Value);
                                }
                            }
                            //Caso a escolha seja para saber de um jogador, mas não tenha nenhum salvo, é informado que ainda não possui nenhum
                            else if (informationResponse.ToLower() == "jogador" && listPlayers.Count <= 0)
                            {
                                Console.WriteLine("Ainda não possui nenhum jogador nos seus favoritos.");
                                Console.WriteLine();
                            }

                            //Obtém todos o times guardados nos favoritos
                            List<string> listTeams = await teamController.GetAllFavoriteTeams();

                            //Executa o trecho de times, caso o interesse seja time e tenha pelo menos um time salvo nos favoritos
                            if (informationResponse.ToLower() == "time" && listTeams.Count > 0)
                            {
                                Console.WriteLine("Times favoritos:");
                                foreach (var team in listTeams)
                                {
                                    Console.WriteLine(team);
                                }

                                Console.WriteLine();

                                Console.WriteLine("De qual time acima deseja saber informações?");

                                string favoriteTeamName = Console.ReadLine();
                                Console.WriteLine();

                                //Obtém o time nos favoritos especificado acima
                                int? favoriteTeamId = await teamController.GetFavoriteTeam(favoriteTeamName);

                                if (favoriteTeamId == null || favoriteTeamId == 0)
                                {
                                    Console.WriteLine("Não foram encontradas informações desse time nos seus favoritos.");
                                    Console.WriteLine();
                                }
                                else
                                {
                                    //Obtém as informações específicas do time e as informações dos jogos desse time
                                    TeamInfoDTO favoriteTeamInformations = teamController.GetTeamInformation(favoriteTeamId.Value);
                                    TeamMatchesDTO favoriteTeamMatchesInformations = await teamController.GetTeamMatches(favoriteTeamId.Value);

                                    Console.WriteLine($"Time: {favoriteTeamInformations.ShortName}");
                                    Console.WriteLine($"Nome completo: {favoriteTeamInformations.Name}");
                                    Console.WriteLine($"País: {translationController.TranslateText(favoriteTeamInformations.Nationality, "pt").Result}");
                                    Console.WriteLine($"Ano de fundação: {favoriteTeamInformations.Founded}");
                                    Console.WriteLine($"Cores: {translationController.TranslateText(favoriteTeamInformations.ClubColors, "pt").Result}");
                                    Console.WriteLine($"Estádio: {favoriteTeamInformations.Venue}");

                                    //Transforma as competições que estão em um formato de lista de strings em um bloco string
                                    Console.WriteLine($"Competições disputadas: {String.Join(", ",favoriteTeamInformations.NamesRunningCompetitions)}");

                                    Console.WriteLine($"Treinador: {favoriteTeamInformations.Coach.Name}");
                                    Console.WriteLine();
                                    Console.WriteLine($"Jogadores do {favoriteTeamInformations.ShortName}:");
                                    Console.WriteLine();

                                    foreach (var teamPlayer in favoriteTeamInformations.Squad)
                                    {
                                        Console.WriteLine($"Nome do jogador: {teamPlayer.Name}");
                                        Console.WriteLine($"Seção do campo: {(appService.GetEnumDescription(typeof(EnumPlayerPosition).GetField(teamPlayer.Position ?? "-")))}");
                                        Console.WriteLine();
                                    }

                                    Console.WriteLine($"Temporada do {favoriteTeamInformations.ShortName}:");
                                    Console.WriteLine($"Primeiro jogo da temporada: {favoriteTeamMatchesInformations.FirstMatch:dd/MM/yyyy}");
                                    Console.WriteLine($"Último jogo da temporada: {favoriteTeamMatchesInformations.LastMatch:dd/MM/yyyy}");
                                    Console.WriteLine($"Partidas jogadas: {favoriteTeamMatchesInformations.MatchesPlayed}");
                                    Console.WriteLine($"Vitórias: {favoriteTeamMatchesInformations.Wins}");
                                    Console.WriteLine($"Derrotas: {favoriteTeamMatchesInformations.Losses}");

                                    int empates = favoriteTeamMatchesInformations.MatchesPlayed - (favoriteTeamMatchesInformations.Wins + favoriteTeamMatchesInformations.Losses);

                                    Console.WriteLine($"Empates: {(favoriteTeamMatchesInformations.MatchesPlayed > 0 ? empates : 0)}");
                                    Console.WriteLine();

                                    //Agrupando o jogos desse time pelo nome da competição
                                    var groupByTeamMatches = favoriteTeamMatchesInformations.Matches
                                        .GroupBy(s => new { s.Competition.CompetitionName, s.AreaName, s.CurrentMatchday, s.Group, s.Stage }, 
                                        s => new { s.Competition.CompetitionName, s })
                                        .Select(g => new
                                        {
                                            CompetitionName = g.Key.CompetitionName,
                                            AreaName = g.Key.AreaName,
                                            CurrentMatchday = g.Key.CurrentMatchday,
                                            Group = g.Key.Group,
                                            Stage = g.Key.Stage,
                                            Matches = g.Select(x => x.s)
                                        });


                                    foreach (var item in groupByTeamMatches)
                                    {
                                        Console.WriteLine($"Jogos do {favoriteTeamInformations.ShortName}:");
                                        Console.WriteLine($"Competição: {item.CompetitionName}");
                                        Console.WriteLine($"Área: {translationController.TranslateText(item.AreaName, "pt").Result}");
                                        Console.WriteLine($"Rodada atual: {item.CurrentMatchday}");
                                        Console.WriteLine($"Estágio: {(appService.GetEnumDescription(typeof(EnumCompetitionStage).GetField(item.Stage ?? "-")))}");
                                        Console.WriteLine($"Grupo: {translationController.TranslateText(item.Group != null ? item.Group.Replace('_', ' ') : "-", "pt").Result}");
                                        Console.WriteLine();

                                        foreach (var favoriteTeamMatch in item.Matches.Select(t => t))
                                        {
                                            Console.WriteLine($"Rodada do jogo: {favoriteTeamMatch.Matchday}");
                                            Console.WriteLine($"Data do jogo: {favoriteTeamMatch.MatchDate:dd/MM/yyyy}");
                                            Console.WriteLine($"Status: {(appService.GetEnumDescription(typeof(EnumMatchStatus).GetField(favoriteTeamMatch.Status ?? "-")))}");
                                            Console.WriteLine($"Placar do primeiro tempo: {favoriteTeamMatch.HomeTeam.TeamName}: {favoriteTeamMatch.Score.HalfTime.Home ?? 0} x {favoriteTeamMatch.AwayTeam.TeamName}: {favoriteTeamMatch.Score.HalfTime.Away ?? 0}");
                                            Console.WriteLine($"Placar do segundo tempo: {favoriteTeamMatch.HomeTeam.TeamName}: {favoriteTeamMatch.Score.FullTime.Home ?? 0} x {favoriteTeamMatch.AwayTeam.TeamName}: {favoriteTeamMatch.Score.FullTime.Away ?? 0}");

                                            string vencedor = favoriteTeamMatch.Score.Winner == "HOME_TEAM" ? "- " + favoriteTeamMatch.HomeTeam.TeamName : favoriteTeamMatch.Score.Winner == "AWAY_TEAM" ? "- " + favoriteTeamMatch.AwayTeam.TeamName : "";

                                            Console.WriteLine($"Vencedor: {(appService.GetEnumDescription(typeof(EnumWinnerTeam).GetField(favoriteTeamMatch.Score.Winner ?? "-")))} {vencedor}");
                                            Console.WriteLine($"Duração do jogo: {(appService.GetEnumDescription(typeof(EnumScoreDuration).GetField(favoriteTeamMatch.Score.Duration ?? "-")))}");
                                            Console.WriteLine();
                                        }
                                    }

                                    await appService.DeleteDataFromFavorites(null, teamController, favoriteTeamId.Value);
                                    Console.WriteLine();
                                }
                            }
                            //Caso a escolha seja para saber de um time, mas não tenha nenhum salvo, é informado que ainda não possui nenhum
                            else if (informationResponse.ToLower() == "time" && listTeams.Count <= 0)
                            {
                                Console.WriteLine("Ainda não possui nenhum time nos seus favoritos.");
                                Console.WriteLine();
                            }
                        }
                    }
                    //Entra na parte de novas informações, caso a resposta de interesse nos favoritos seja 'não'
                    if (favoritesResponse.ToLower() == "não" || favoritesResponse.ToLower() == "nao" || favoritesResponse.ToLower().Equals("n"))
                    {
                        Console.WriteLine("-------------------------------------------------------------------- NOVAS INFORMAÇÕES --------------------------------------------------------------------");

                        string newInformationsResponse = " ";
                        string inputNewInformations = "";

                        Console.WriteLine();

                        //Rodada a parte de novas informações enquanto resposta de interesse nos nas novas informações for 'sim'
                        while (newInformationsResponse.ToLower() == "sim" || newInformationsResponse.ToLower().Equals("s") || inputNewInformations == "")
                        {
                            //Obtém interesse em novas informações
                            newInformationsResponse = appService.GetInterestInFavoritesOrNewInformationsOrScores(false);

                            inputNewInformations = "-";

                            if (newInformationsResponse.ToLower() == "sim" || newInformationsResponse.ToLower().Equals("s"))
                            {
                                //Lista as competições com informações disponíveis
                                List<string> availableCompetitions = await competitionController.ListCompetitions();

                                Console.WriteLine("Competições:");

                                foreach (string availableCompetition in availableCompetitions)
                                {
                                    Console.WriteLine(availableCompetition);
                                }

                                Console.WriteLine();

                                string competitionCode = (string)await appService.GetTheInterestedData(competitionController, null, false, false, true, null, null);

                                //Obtém interesse nos artilheiros do campeonato
                                string scoresResponse = appService.GetInterestInFavoritesOrNewInformationsOrScores(false, true);

                                //Obtém a tabela de classificação da competição especificada
                                CompetitionTableDTO competitionTable = await competitionController.GetCompetitionTable(competitionCode);

                                if (scoresResponse.ToLower() == "sim" || scoresResponse.ToLower().Equals("s"))
                                {
                                    //Obtém os artilheiros da competição especificada
                                    TopScorersDTO topScores = await competitionController.GetTopScores(competitionCode);

                                    Console.WriteLine($"{topScores.Competition.CompetitionName}");
                                    Console.WriteLine($"Tipo de competição: {(appService.GetEnumDescription(typeof(EnumTypeCompetition).GetField(topScores.TypeCompetition ?? "-")))}");
                                    Console.WriteLine($"Rodada atual: {topScores.CurrentMatchday}");
                                    Console.WriteLine();

                                    int i = 1;

                                    foreach (var scorer in topScores.Scorers.OrderByDescending(s => s.Goals))
                                    {
                                        Console.WriteLine($"{i}º");
                                        Console.WriteLine($"Jogador: {scorer.Player.Name}");
                                        Console.WriteLine($"Time: {scorer.Team.TeamName}");
                                        Console.WriteLine($"Partidas jogadas: {scorer.PlayedMatches}");
                                        Console.WriteLine($"Gols: {scorer.Goals ?? 0}");
                                        Console.WriteLine($"Assistências: {scorer.Assists ?? 0}");
                                        Console.WriteLine($"Pênaltis: {scorer.Penalties ?? 0}");
                                        Console.WriteLine();

                                        i++;
                                    }
                                }

                                Console.WriteLine($"Competição: {competitionTable.CompetitionName}");
                                Console.WriteLine($"País: {translationController.TranslateText(competitionTable.AreaName,"pt").Result}");
                                Console.WriteLine($"Primeiro jogo: {competitionTable.StartDate:dd/MM/yyyy}");
                                Console.WriteLine($"Último jogo: {competitionTable.EndDate:dd/MM/yyyy}");
                                Console.WriteLine($"Rodada atual: {competitionTable.CurrentMatchday}");

                                Console.WriteLine($"Campeão: {(competitionTable.Winner != null ? competitionTable.Winner.TeamName : "-")}");
                                Console.WriteLine();

                                //Agrupando a classificação dos times por grupo, estágio e tipo
                                var groupByTeams = competitionTable.Standings
                                    .GroupBy(s => new { s.Stage, s.Type, s.Group }, s => new { s.Stage, s.Type, s.Group, s.Table })
                                    .Select(g => new
                                    {
                                        Stage = g.Key.Stage,
                                        Type = g.Key.Type,
                                        Group = g.Key.Group,
                                        Table = g.Select( x => x.Table)
                                    });


                                foreach (var item in groupByTeams)
                                {
                                    Console.WriteLine($"Estágio: {(appService.GetEnumDescription(typeof(EnumCompetitionStage).GetField(item.Stage ?? "-")))}");
                                    Console.WriteLine($"Tipo: {(appService.GetEnumDescription(typeof(EnumTypeCompetition).GetField(item.Type ?? "-")))}");
                                    Console.WriteLine($"Grupo: {translationController.TranslateText(item.Group != null ? item.Group.Replace('_', ' ') : "-","pt").Result ?? "-"}");
                                    Console.WriteLine();

                                    foreach (var teams in item.Table.Select(t => t))
                                    {
                                        foreach (var team in teams)
                                        {
                                            Console.WriteLine($"Posição: {team.Position}");
                                            Console.WriteLine($"Nome: {team.Team.TeamName}");
                                            Console.WriteLine($"Pontos: {team.Points}");
                                            Console.WriteLine($"Partidas jogadas: {team.PlayedGames}");
                                            Console.WriteLine($"Vitórias: {team.Won}");
                                            Console.WriteLine($"Empates: {team.Draw}");
                                            Console.WriteLine($"Derrotas: {team.Lost}");
                                            Console.WriteLine($"Gols a favor: {team.GoalsFor}");
                                            Console.WriteLine($"Gols contra: {team.GoalsAgainst}");
                                            Console.WriteLine($"Saldo de gols: {team.GoalDifference}");
                                            Console.WriteLine();
                                        }
                                    }
                                }

                                //Lista os times da competição especificada com informações disponíveis
                                List<string> competitionTeams = await teamController.ListTeams(competitionCode);

                                Console.WriteLine("Times:");

                                foreach (string competitionTeam in competitionTeams)
                                {
                                    Console.WriteLine(competitionTeam);
                                }

                                Console.WriteLine();

                                int? teamId = (int?)await appService.GetTheInterestedData(null, teamController, false, true, false, null, competitionCode);

                                //Obtém as informações específicas do time e as informações dos jogos desse time
                                TeamInfoDTO teamInformation = teamController.GetTeamInformation(teamId.Value);
                                TeamMatchesDTO teamMatches = await teamController.GetTeamMatches(teamId.Value);

                                Console.WriteLine($"Time: {teamInformation.ShortName}");
                                Console.WriteLine($"Nome completo: {teamInformation.Name}");
                                Console.WriteLine($"País: {translationController.TranslateText(teamInformation.Nationality, "pt").Result}");
                                Console.WriteLine($"Ano de fundação: {teamInformation.Founded}");
                                Console.WriteLine($"Cores: {translationController.TranslateText(teamInformation.ClubColors, "pt").Result}");
                                Console.WriteLine($"Estádio: {teamInformation.Venue}");
                                Console.WriteLine($"Competições disputadas: {String.Join(", ", teamInformation.NamesRunningCompetitions)}");
                                Console.WriteLine($"Treinador: {teamInformation.Coach.Name}");
                                Console.WriteLine();
                                Console.WriteLine($"Jogadores do {teamInformation.ShortName}: ");
                                Console.WriteLine();

                                foreach (var teamPlayer in teamInformation.Squad)
                                {
                                    Console.WriteLine($"Nome do jogador: {teamPlayer.Name}");
                                    Console.WriteLine($"Seção do campo: {(appService.GetEnumDescription(typeof(EnumPlayerPosition).GetField(teamPlayer.Position ?? "-")))}");
                                    Console.WriteLine();
                                }

                                Console.WriteLine($"Primeiro jogo da temporada: {teamMatches.FirstMatch}");
                                Console.WriteLine($"Último jogo da temporada: {teamMatches.LastMatch}");
                                Console.WriteLine($"Partidas jogadas: {teamMatches.MatchesPlayed}");
                                Console.WriteLine($"Vitórias: {teamMatches.Wins}");
                                Console.WriteLine($"Derrotas: {teamMatches.Losses}");
                                Console.WriteLine($"Empates: {teamMatches.MatchesPlayed - (teamMatches.Wins + teamMatches.Losses)}");
                                Console.WriteLine();

                                //Agrupando o jogos desse time pelo nome da competição
                                var groupByTeamMatches = teamMatches.Matches
                                    .GroupBy(s => new { s.Competition.CompetitionName, s.AreaName, s.CurrentMatchday, s.Group, s.Stage },
                                    s => new { s.Competition.CompetitionName, s })
                                    .Select(g => new
                                    {
                                        CompetitionName = g.Key.CompetitionName,
                                        AreaName = g.Key.AreaName,
                                        CurrentMatchday = g.Key.CurrentMatchday,
                                        Group = g.Key.Group,
                                        Stage = g.Key.Stage,
                                        Matches = g.Select(x => x.s)
                                    }).OrderBy(t => t.CompetitionName);


                                foreach (var item in groupByTeamMatches)
                                {
                                    Console.WriteLine($"Jogos do {teamInformation.ShortName}:");
                                    Console.WriteLine($"Competição: {item.CompetitionName}");
                                    Console.WriteLine($"Área: {translationController.TranslateText(item.AreaName,"pt").Result}");
                                    Console.WriteLine($"Rodada atual: {item.CurrentMatchday}");
                                    Console.WriteLine($"Estágio: {(appService.GetEnumDescription(typeof(EnumCompetitionStage).GetField(item.Stage ?? "-")))}");
                                    Console.WriteLine($"Grupo: {translationController.TranslateText(item.Group != null ? item.Group.Replace('_', ' ') : "-", "pt").Result}");
                                    Console.WriteLine();

                                    foreach (var teamMatch in item.Matches.Select(t => t))
                                    {
                                        Console.WriteLine($"Rodada do jogo: {teamMatch.Matchday}");
                                        Console.WriteLine($"Data do jogo: {teamMatch.MatchDate:dd/MM/yyyy}");
                                        Console.WriteLine($"Status: {(appService.GetEnumDescription(typeof(EnumMatchStatus).GetField(teamMatch.Status ?? "-")))}");
                                        Console.WriteLine($"Placar do primeiro tempo: {teamMatch.HomeTeam.TeamName}: {teamMatch.Score.HalfTime.Home ?? 0} x {teamMatch.AwayTeam.TeamName}: {teamMatch.Score.HalfTime.Away ?? 0}");
                                        Console.WriteLine($"Placar do segundo tempo: {teamMatch.HomeTeam.TeamName}: {teamMatch.Score.FullTime.Home ?? 0} x {teamMatch.AwayTeam.TeamName}: {teamMatch.Score.FullTime.Away ?? 0}");

                                        string vencedor = teamMatch.Score.Winner == "HOME_TEAM" ? "- " + teamMatch.HomeTeam.TeamName : teamMatch.Score.Winner == "AWAY_TEAM" ? "- " + teamMatch.AwayTeam.TeamName : "";

                                        Console.WriteLine($"Vencedor: {(appService.GetEnumDescription(typeof(EnumWinnerTeam).GetField(teamMatch.Score.Winner ?? "-")))} {vencedor}");
                                        Console.WriteLine($"Duração do jogo: {(appService.GetEnumDescription(typeof(EnumScoreDuration).GetField(teamMatch.Score.Duration ?? "-")))}");
                                        Console.WriteLine();
                                    }
                                }

                                //Salva o time nos favoritos
                                await appService.SaveDataInFavorites(null, teamController, false, null, teamId.Value);

                                Console.WriteLine();
                                Console.WriteLine($"Jogadores do {teamInformation.ShortName}:");
                                Console.WriteLine();

                                foreach (var teamPlayer in teamInformation.Squad)
                                {
                                    Console.WriteLine($"{teamPlayer.Name} ({(appService.GetEnumDescription(typeof(EnumPlayerPosition).GetField(teamPlayer.Position ?? "-")))})");
                                }

                                Console.WriteLine();

                                //Obtém o jogador de interesse
                                PlayerDTO player = (PlayerDTO)await appService.GetTheInterestedData(null, teamController, true, false, false, teamId.Value, null);

                                //Obtém as informações específicas do jogador e as informações dos jogos desse jogador
                                PlayerInfoDTO playerInformation = await playerController.GetPlayerInformation(player.Id);

                                Console.WriteLine($"Jogador: {playerInformation.Name}");
                                Console.WriteLine($"Primeiro nome: {playerInformation.FirstName}");
                                Console.WriteLine($"Sobrenome nome: {playerInformation.LastName}");
                                Console.WriteLine($"Data de nascimento: {playerInformation.DateOfBirth:dd/MM/yyyy} ({playerInformation.Age} anos)");
                                Console.WriteLine($"Nacionalidade: {translationController.TranslateText(playerInformation.Nationality, "pt").Result}");
                                Console.WriteLine($"Posição: {(appService.GetEnumDescription(typeof(EnumPlayerPosition).GetField(playerInformation.Position != null ? playerInformation.Position.Replace("-", string.Empty).Replace(" ", string.Empty) : "-")))} - Seção: {(appService.GetEnumDescription(typeof(EnumPlayerPosition).GetField(playerInformation.Section ?? "-")))}");
                                Console.WriteLine($"Número: {playerInformation.ShirtNumber}");
                                Console.WriteLine($"Time atual: {playerInformation.CurrentTeam.Name} ({playerInformation.CurrentTeam.ShortName}) - País: {translationController.TranslateText(playerInformation.CurrentTeam.Nationality, "pt").Result}");
                                Console.WriteLine($"Contrato: {playerInformation.Contract.Start:dd/MM/yyyy} até {playerInformation.Contract.Until:dd/MM/yyyy}");

                                //Transforma os códigos das competições do jogador que estão em um formato de bloco string em uma lista de strings
                                List<string> listCompetitionsCodes = playerInformation.CurrentTeam.NamesRunningCompetitions;

                                //Obtém uma string com o nome das competições de um jogador
                                string stringCompetitions = String.Join(", ", listCompetitionsCodes);

                                Console.WriteLine($"Competições: {stringCompetitions}");
                                Console.WriteLine();

                                //Salva o jogador nos favoritos
                                await appService.SaveDataInFavorites(playerController, null, true, player, null);
                            }
                        }
                        if (newInformationsResponse.ToLower() == "não" || newInformationsResponse.ToLower() == "nao" || newInformationsResponse.ToLower().Equals("n"))
                        {
                            //Environment.Exit(0);

                            finish = true;
                        }
                    }
                }

                //Encerra o aplicativo de console
                Environment.Exit(0);

                //Aguarda a tecla Enter ser pressionada antes de encerrar o programa
                Console.ReadLine();
            }
            catch (AppException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }
    }
}
