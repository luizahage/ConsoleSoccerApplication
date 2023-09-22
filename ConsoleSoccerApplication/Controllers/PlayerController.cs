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
    public class PlayerController
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ConfigurationService _configurationService;

        public PlayerController(IPlayerRepository playerRepository, ConfigurationService configurationService)
        {
            _playerRepository = playerRepository;
            _configurationService = configurationService;
        }

        /// <summary>
        /// Busca informações de um jogador específico
        /// <paramref name="playerId"/>
        ///</summary>
        public async Task<PlayerInfoDTO> GetPlayerInformation(int playerId)
        {
            string uriApi = $"http://api.football-data.org//v4/persons/{playerId}";

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

                    //Desserializa uma string JSON em um objeto Player
                    Player playerInfoResponse = JsonConvert.DeserializeObject<Player>(response);

                    //Transforma objeto do tipo Player em um objeto do tipo PlayerInfoDTO
                    PlayerInfoDTO playerInfo = Helper.GetPlayerInfo(playerInfoResponse);

                    //Retorna os dados necessários através do DTO
                    return playerInfo;
                }
            }
            catch (Exception e)
            {
                throw new AppException("Não foi possível obter as informações desse jogador! " + e.Message);
            }
        }

        /// <summary>
        /// Busca informações dos jogos de um jogador específico
        /// <paramref name="playerId"/>
        ///</summary>
        public async Task<PlayerMatchesDTO> GetPlayerMatches(int playerId)
        {
            string uriApi = $"http://api.football-data.org/v4/persons/{playerId}/matches";

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
                    //Espera a conclusão de uma tarefa assíncrona antes de prosseguir com a execução do código
                    //response.Wait();

                    //Desserializa uma string JSON em um objeto PlayerMatches
                    PlayerMatches playerMatchesResponse = JsonConvert.DeserializeObject<PlayerMatches>(response);

                    //Transforma objeto do tipo PlayerMatches em um objeto do tipo PlayerMatchesDTO
                    PlayerMatchesDTO playerMatches = Helper.GetPlayerMatches(playerMatchesResponse);

                    //Retorna os dados necessários através do DTO
                    return playerMatches;
                }
            }
            catch (Exception e)
            {
                throw new AppException("Não foi possível obter as informações dos jogos desse jogador! " + e.Message);
            }
        }

        /// <summary>
        /// Obtém o jogador favorito com determinado nome no banco de dados
        /// <paramref name="codePlayerName"/>
        /// </summary>
        public async Task<int?> GetFavoritePlayer(string playerName)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Obtém jogador com determinado nome da tabela de jogadores favoritos
            PlayerInfoDTO player = await _playerRepository.GetFavoritePlayerFromDatabase(connectionString, playerName);

            //Caso o jogador exista no banco (seja diferente de null) obtém o id do jogador
            int? playerId = player != null ? player.Id : 0;

            return playerId;
        }

        /// <summary>
        /// Deleta o jogador dos favorito no banco de dados
        /// <paramref name="playerId"/>
        /// </summary>
        public async Task<string> DeleteFavoritePlayer(int playerId)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Deleta um jogador com determinado id da tabela de jogadores favoritos
            bool deleteResponse = await _playerRepository.DeleteFavoritePlayerToDatabase(connectionString, playerId);

            //Informa se a exlusão foi feita com sucesso
            if (deleteResponse)
            {
                return "Jogador deletado dos favoritos com sucesso.";
            }
            else
            {
                return "Não foi possível deletar esse jogador dos favoritos.";
            }
        }

        /// <summary>
        /// Salva o jogador nos favoritos do banco de dados
        /// <paramref name="player"/>
        ///</summary>
        public async Task<string> SaveFavoritePlayer(PlayerDTO player)
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Insere um jogador na tabela de jogadores favoritos
            (bool, bool) insertResponse = await _playerRepository.InsertFavoritePlayerToDatabase(connectionString, player);

            //Informa se a inserção foi feita com sucesso
            if (insertResponse.Item1)
            {
                return "Jogador salvo com sucesso nos favoritos.";
            }
            else
            {
                return $"Não foi possível salvar esse jogador nos favoritos. {(insertResponse.Item2 ? "Esse jogador já existe nos favoritos." : "")}";
            }
        }

        /// <summary>
        /// Obtém todos os jogadores favoritos no banco de dados
        /// </summary>
        public async Task<List<string>> GetAllFavoritePlayers()
        {
            //Define string de conexão com o banco de dados
            string connectionString = _configurationService.GetConnectionString();

            //Obtém todos os jogadores da tabela de jogadores favoritos
            List<string> listPlayers = await _playerRepository.GetFavoritePlayersFromDatabase(connectionString);

            return listPlayers;
        }
    }
}
