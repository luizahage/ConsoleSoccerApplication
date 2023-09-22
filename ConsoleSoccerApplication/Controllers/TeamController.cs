using ConsoleSoccerApplication.Data.Interfaces;
using ConsoleSoccerApplication.Exceptions;
using ConsoleSoccerApplication.Mapper;
using ConsoleSoccerApplication.Models.Dtos;
using ConsoleSoccerApplication.Models.Entities;
using ConsoleSoccerApplication.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleSoccerApplication.Controllers
{
    public class TeamController
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ConfigurationService _configurationService;

        public TeamController(ITeamRepository teamRepository, ConfigurationService configurationService)
        {
            _teamRepository = teamRepository;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Busca informações de um time específico
        /// <paramref name="teamId"/>
        ///</summary>
        public TeamInfoDTO GetTeamInformation(int teamId)
        {
            var uriApi = $"http://api.football-data.org/v4/teams/{teamId}";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    //Define URI base para esta requisição
                    httpClient.BaseAddress = new Uri(uriApi);
                    //Limpa todos os cabeçalhos "Accept" padrão definidos para o objeto HttpClient
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    //Adiciona um cabeçalho "Accept" personalizado, como estando disposto a aceitar respostas no formato JSON
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Acessa o token
                    string authToken = _configurationService.GetAuthToken();
                    //Adiciona um cabeçalho personalizado à solicitação HTTP
                    httpClient.DefaultRequestHeaders.Add("X-Auth-Token", authToken);
                    //Realiza uma solicitação HTTP GET assíncrona para a URI
                    Task<string> response = httpClient.GetStringAsync(uriApi);
                    //Espera a conclusão de uma tarefa assíncrona antes de prosseguir com a execução do código
                    response.Wait();

                    //Desserializa uma string JSON em um objeto Team
                    Team teamResponse = JsonConvert.DeserializeObject<Team>(response.Result);

                    //Transforma um objeto do tipo Team em um objeto do tipo TeamInfoDTO
                    TeamInfoDTO teamInfo = Helper.GetTeamInfo(teamResponse);

                    //Retorna os dados necessários através da coleção de DTO
                    return teamInfo;
                }
            }
            catch (Exception e)
            {
                throw new AppException("Não foi possível obter as informações desse time! " + e.Message);
            }
        }

        /// <summary>
        /// Busca informações dos jogos de um time específico
        /// <paramref name="teamId"/>
        ///</summary>
        public async Task<TeamMatchesDTO> GetTeamMatches(int teamId)
        {
            var uriApi = $"http://api.football-data.org/v4/teams/{teamId}/matches/";
            try
            {
                using (var httpClient = new HttpClient())
                {
                    //Define URI base para esta requisição
                    httpClient.BaseAddress = new Uri(uriApi);
                    //Limpa todos os cabeçalhos "Accept" padrão definidos para o objeto HttpClient
                    httpClient.DefaultRequestHeaders.Accept.Clear();
                    //Adiciona um cabeçalho "Accept" personalizado, como estando disposto a aceitar respostas no formato JSON
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //Acessa o token
                    string authToken = _configurationService.GetAuthToken();
                    //Adiciona um cabeçalho personalizado à solicitação HTTP
                    httpClient.DefaultRequestHeaders.Add("X-Auth-Token", authToken);
                    //Realiza uma solicitação HTTP GET assíncrona para a URI
                    //Task<string> response = httpClient.GetStringAsync(uriApi);
                    string response = await httpClient.GetStringAsync(uriApi);
                    //Esperando a conclusão de uma tarefa assíncrona antes de prosseguir com a execução do código
                    //response.Wait();

                    //Desserializa uma string JSON em um objeto TeamMatches
                    //TeamMatches teamMatchesResponse = JsonConvert.DeserializeObject<TeamMatches>(response.Result);
                    TeamMatches teamMatchesResponse = JsonConvert.DeserializeObject<TeamMatches>(response);

                    //Transforma um objeto do tipo TeamMatches em um objeto do tipo TeamMatchesDTO
                    TeamMatchesDTO teamMatches = Helper.GetTeamMatches(teamMatchesResponse);

                    //Retorna os dados necessários através da coleção de DTO
                    return teamMatches;
                }
            }
            catch (Exception e)
            {
                throw new AppException("Não foi possível obter as informações dos jogos desse time! " + e.Message);
            }
        }

        /// <summary>
        /// Salva os times de uma competição específica no banco de dados
        /// <paramref name="teams"/>
        /// <paramref name="competition"/>
        ///</summary>
        public async Task<string> SaveTeams(IEnumerable<TeamDTO> teams, CompetitionDTO competition)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Insere os times de uma competição específica na tabela de times
            bool insertResponse = await _teamRepository.InsertTeamsToDatabase(connectionString, teams, competition.CompetitionId);

            //Informa se a inserção foi feita com sucesso
            if (insertResponse)
            {
                return $"Times da competição {competition.CompetitionName} salvos com sucesso no banco de dados.";
            }
            return $"Não foi possiível salvar os times da competição {competition.CompetitionName} no banco de dados.";
        }

        /// <summary>
        /// Obtém o time favorito com determinado nome no banco de dados
        /// <paramref name="teamName"/>
        /// </summary>
        public async Task<int?> GetFavoriteTeam(string teamName)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Obtém uma time específico da tabela de times favoritos a partir do parâmetro nome 
            TeamDTO team = await _teamRepository.GetFavoriteTeamFromDatabase(connectionString, teamName);

            //Caso o time exista no banco (seja diferente de null) obtém o id do time
            int? teamId = team != null ? team.TeamId : 0;

            return teamId;
        }

        /// <summary>
        /// Deleta o time dos favorito no banco de dados
        /// <paramref name="teamId"/>
        /// </summary>
        public async Task<string> DeleteFavoriteTeam(int teamId)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Deleta um time com determinado id da tabela de times favoritos
            bool deleteResponse = await _teamRepository.DeleteFavoriteTeamToDatabase(connectionString, teamId);

            //Informa se a exclusão foi feita com sucesso
            if (deleteResponse)
            {
                return "Time deletado dos favoritos com sucesso.";
            }
            else
            {
                return "Não foi possível deletar esse time dos favoritos.";
            }
        }

        /// <summary>
        /// Obtém o time com determinado nome no banco de dados
        /// <paramref name="teamName"/>
        /// </summary>
        public async Task<int?> GetTeam(string teamName, string competitionCode)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Obtém time com determinado nome na tabela de times
            TeamDTO team = await _teamRepository.GetTeamFromDatabase(connectionString, teamName, competitionCode);

            //Caso o time exista no banco (seja diferente de null) obtém o id do time
            int? teamId = team?.TeamId;

            return teamId;
        }

        /// <summary>
        /// Salva o time nos favoritos do banco de dados
        /// <paramref name="teamId"/>
        ///</summary>
        public async Task<string> SaveFavoriteTeam(int teamId)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Insere um time na tabela de times favoritos
            (bool, bool) insertResponse = await _teamRepository.InsertFavoriteTeamToDatabase(connectionString, teamId);

            //Informa se a inserção foi feita com sucesso
            if (insertResponse.Item1)
            {
                return "Time salvo com sucesso nos favoritos.";
            }
            else
            {
                return $"Não foi possível salvar esse time nos favoritos. {(insertResponse.Item2 ? "Esse time já existe nos favoritos." : "")}";
            }
        }

        /// <summary>
        /// Obtêm todos os times favoritos no banco de dados
        /// </summary>
        public async Task<List<string>> GetAllFavoriteTeams()
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Obtém todos os times da tabela de times favoritos
            List<string> listTeams = await _teamRepository.GetFavoriteTeamsFromDatabase(connectionString);

            return listTeams;
        }

        /// <summary>
        /// Lista os times de uma competição específica disponíveis no banco de dados
        /// </summary>
        public async Task<List<string>> ListTeams(string competitionCode)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Obtém os times de uma competição específica da tabela de times
            List<TeamDTO> teams = await _teamRepository.GetAllTeamsByCompetitionCodeFromDatabase(connectionString, competitionCode);

            List<string> listStringTeams = new List<string>();

            //Percorre os times obtidos no banco de dados
            foreach (var team in teams)
            {
                //Adiciona a string de cada time a lista
                listStringTeams.Add($"{team.TeamShortName ?? team.TeamName} - {team.TeamName}");
            }

            return listStringTeams;
        }
    }
}
