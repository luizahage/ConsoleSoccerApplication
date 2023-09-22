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
    public class CompetitionController
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly TranslationController _translationController;
        private readonly ConfigurationService _configurationService;

        public CompetitionController(ICompetitionRepository competitionRepository, TranslationController translationController, ConfigurationService configurationService)
        {
            _competitionRepository = competitionRepository;
            _translationController = translationController; 
            _configurationService = configurationService;
        }

        /// <summary>
        /// Busca todas as competições disponíveis
        /// </summary>
        public async Task<IEnumerable<CompetitionDTO>> GetAllAvailableCompetitions()
        {
            string uriApi = $"http://api.football-data.org/v4/competitions/";

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
                    string response =  await httpClient.GetStringAsync(uriApi);
                    //Espera a conclusão de uma tarefa assíncrona antes de prosseguir com a execução do código
                    //response.Wait();

                    //Desserializa uma string JSON em um objeto CompetitionResponse
                    CompetitionsResponse competitionResponse = JsonConvert.DeserializeObject<CompetitionsResponse>(response);

                    //Transforma uma lista de objetos do tipo Competition em uma coleção de objetos do tipo CompetitionDTO
                    IEnumerable<CompetitionDTO> competitions = Helper.GetAvailableCompetitions(competitionResponse.Competitions);

                    //Retorna os dados necessários através da coleção de DTO
                    return competitions;
                }
            }
            catch (Exception e)
            {
                throw new AppException("Não foi possível obter as competições disponíveis! " + e.Message);
            }
        }

        /// <summary>
        /// Busca todos os time de uma competição específica
        /// <paramref name="competitionCode"/>
        /// </summary>
        public async Task<IEnumerable<TeamDTO>> GetAllCompetitionTeams(string competitionCode)
        {
            string uriApi = $"http://api.football-data.org/v4/competitions/{competitionCode}/teams";

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
                    string response = await httpClient.GetStringAsync(uriApi);
                    //Espera a conclusão de uma tarefa assíncrona antes de prosseguir com a execução do código
                    //response.Wait();

                    //Desserializa uma string JSON em um objeto TeamsResponse
                    TeamsResponse teamsResponse = JsonConvert.DeserializeObject<TeamsResponse>(response);

                    //Transforma uma lista de objetos do tipo Team em uma coleção de objetos do tipo TeamDTO
                    IEnumerable<TeamDTO> teams = Helper.GetTeams(teamsResponse.Teams);

                    //Retorna os dados necessários através da coleção de DTO
                    return teams;
                }
            }
            catch (Exception e)
            {
                throw new AppException("Não foi possível obter os times dessa competição! " + e.Message);
            }
        }

        /// <summary>
        /// Busca a classificação dos times de uma competição específica
        /// <paramref name="competitionCode"/>
        /// </summary>
        public async Task<CompetitionTableDTO> GetCompetitionTable(string competitionCode)
        {
            string uriApi = $"http://api.football-data.org/v4/competitions/{competitionCode}/standings";

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
                    string response = await httpClient.GetStringAsync(uriApi);
                    //Espera a conclusão de uma tarefa assíncrona antes de prosseguir com a execução do código
                    //response.Wait();

                    //Desserializa uma string JSON em um objeto CompetitionTable
                    CompetitionTable competitionTableResponse = JsonConvert.DeserializeObject<CompetitionTable>(response);

                    //Transforma objeto do tipo CompetitionTable em um objeto do tipo CompetitionTableDTO
                    CompetitionTableDTO competitionTable = Helper.GetCompetitionTable(competitionTableResponse);

                    //Retorna os dados necessários através do DTO
                    return competitionTable;
                }
            }
            catch (Exception e)
            {
                throw new AppException("Não foi possível obter as informações da tabela de pontuação dessa competição! " + e.Message);
            }
        }

        /// <summary>
        /// Busca os artilheiros de uma competição específica
        /// <paramref name="competitionCode"/>
        /// </summary>
        public async Task<TopScorersDTO> GetTopScores(string competitionCode)
        {
            string uriApi = $"http://api.football-data.org/v4/competitions/{competitionCode}/scorers";

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
                    string response = await httpClient.GetStringAsync(uriApi);
                    //Espera a conclusão de uma tarefa assíncrona antes de prosseguir com a execução do código
                    //response.Wait();

                    //Desserializa uma string JSON em um objeto TopScorers
                    TopScorers topScorersResponse = JsonConvert.DeserializeObject<TopScorers>(response);

                    //Transforma objeto do tipo TopScorers em um objeto do tipo TopScorersDTO
                    TopScorersDTO topScorers = Helper.GetTopScorers(topScorersResponse);

                    //Retorna os dados necessários através do DTO
                    return topScorers;
                }
            }
            catch (Exception e)
            {
                throw new AppException("Não foi possível obter as informações dos artilheiros dessa competição! " + e.Message);
            }
        }

        /// <summary>
        /// Salva as competições disponíveis no banco de dados
        /// <paramref name="competitions"/>
        /// </summary>
        public async Task<string> SaveCompetitions(IEnumerable<CompetitionDTO> competitions)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Insere as competições disponíveis na tabela de competições
            bool insertResponse = await _competitionRepository.InsertCompetitionsToDatabase(connectionString, competitions);

            //Informa se a inserção foi feita com sucesso
            if (insertResponse)
            {
                return "Competições salvas com sucesso no banco de dados.";
            }
            return "Não foi possível salvar as competições no banco de dados.";
        }

        /// <summary>
        /// Lista as competições disponíveis no banco de dados
        /// </summary>
        public async Task<List<string>> ListCompetitions()
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Obtém as competições da tabela de competições
            List<CompetitionDTO> competitions = await _competitionRepository.GetAllCompetitionsFromDatabase(connectionString);

            List<string> listStringCompetitions = new List<string>();

            //Percorre as competições obtidas no banco de dados 
            foreach (var competition in competitions)
            {
                //Adiciona a string de cada competição a lista
                listStringCompetitions.Add($"{competition.CompetitionName} (País: {_translationController.TranslateText(competition.AreaName, "pt").Result})");
            }

            return listStringCompetitions;
        }

        /// <summary>
        /// Obtém o competição com determinado nome no banco de dados
        /// <paramref name="competitionName"/>
        /// </summary>
        public async Task<string> GetCompetition(string competitionName)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Obtém uma competição específica da tabela de competições a partir do parâmetro nome 
            CompetitionDTO competition = await _competitionRepository.GetCompetitionFromDatabase(connectionString, competitionName);

            //Caso a competição exista no banco (seja diferente de null) obtém o código da competição
            string competitionCode = competition?.CompetitionCode;

            return competitionCode;
        }

        /// <summary>
        /// Lista as competições de um jogador devolvendo como uma string
        /// <paramref name="competitionsCodes"/>
        /// </summary>
        public async Task<string> ListPlayerCompetitions(List<string> competitionsCodes)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            List<string> competitionsNames = new List<string>();

            //Percorre os códigos das competições de um jogador específico
            foreach (var code in competitionsCodes)
            {
                //Obtém o nome da competição com determinado código
                CompetitionDTO competition = await _competitionRepository.GetCompetitionByCodeFromDatabase(connectionString, code);
                //Adiciona o nome da competição a lista de competições de um jogador específico
                competitionsNames.Add(competition.CompetitionName);
            }

            //Transforma a lista de nome das competições em uma string
            string stringCompetition = String.Join(", ", competitionsNames);

            return stringCompetition;
        }
    }
}
