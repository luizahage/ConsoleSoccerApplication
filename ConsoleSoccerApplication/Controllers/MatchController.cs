using ConsoleSoccerApplication.Exceptions;
using ConsoleSoccerApplication.Mapper;
using ConsoleSoccerApplication.Models.Dtos;
using ConsoleSoccerApplication.Models.Entities;
using ConsoleSoccerApplication.Services;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleSoccerApplication.Controllers
{
    public class MatchController
    {
        private readonly ConfigurationService _configurationService;

        public MatchController(ConfigurationService configurationService)
        {
            _configurationService = configurationService;
        }

        /// <summary>
        /// Busca informações de um jogo específico
        /// <paramref name="matchId"/>
        ///</summary>
        public async Task<MatchDTO> GetMatchInformation(int matchId)
        {
            string uriApi = $"http://api.football-data.org/v4/matches/{matchId}";
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

                    //Desserializa uma string JSON em um objeto Match
                    Match matchResponse = JsonConvert.DeserializeObject<Match>(response);

                    //Transforma objeto do tipo Match em um objeto do tipo MatchDTO
                    MatchDTO matchInfo = Helper.GetMatchInfo(matchResponse);

                    //Retorna os dados necessários através da coleção de DTO
                    return matchInfo;
                }
            }
            catch (Exception e)
            {
                throw new AppException("Não foi possível obter as informações desse jogo! " + e.Message);
            }
        }
    }
}
