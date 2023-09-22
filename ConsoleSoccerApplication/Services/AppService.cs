using ConsoleSoccerApplication.Controllers;
using ConsoleSoccerApplication.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ConsoleSoccerApplication.Services
{
    public class AppService
    {
        public string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        public string GetEnumDescription(FieldInfo fi)
        {
            if (fi != null)
            {
                DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

                if (attributes != null && attributes.Any())
                {
                    return attributes.First().Description;
                }

                return "-";
            }

            return "-";
        }

        public async Task DeleteDataFromFavorites(PlayerController playerController, TeamController teamController, int? favoriteDataId)
        {
            string deletedResponse = "";

            while (deletedResponse.ToLower() != "sim" && deletedResponse.ToLower() != "não"
                && deletedResponse.ToLower() != "nao" && !deletedResponse.ToLower().Equals("s")
                && !deletedResponse.ToLower().Equals("n"))
            {
                //Obtém o interesse em deletar o jogador dos favoritos
                Console.WriteLine("Desejar excluir essas informações do seus favoritos?(Sim/Não)");

                deletedResponse = Console.ReadLine();
                Console.WriteLine();

                if (deletedResponse.ToLower() != "sim" && deletedResponse.ToLower() != "não"
                && deletedResponse.ToLower() != "nao" && !deletedResponse.ToLower().Equals("s")
                && !deletedResponse.ToLower().Equals("n"))
                {
                    Console.WriteLine("Essa opção de resposta não existe. Por favor, escreva novamente.");
                    Console.WriteLine();
                }
            }

            if (deletedResponse.ToLower() == "sim" || deletedResponse.ToLower().Equals("s"))
            {
                string deleteResponse;

                if (playerController != null)
                {
                    //Deleta o jogador dos favoritos
                    deleteResponse = await playerController.DeleteFavoritePlayer(favoriteDataId.Value);
                }
                else
                {
                    //Deleta o time dos favoritos
                    deleteResponse = await teamController.DeleteFavoriteTeam(favoriteDataId.Value);
                }

                Console.WriteLine(deleteResponse);
            }
        }

        public async Task GetAndSaveCompetitions(CompetitionController competitionController, IEnumerable<CompetitionDTO> competitions)
        {
            //Salva as competições no banco de dados
            string insertCompetitionResponse = await competitionController.SaveCompetitions(competitions);

            Console.WriteLine(insertCompetitionResponse);
        }

        public async Task GetAndSaveTeams(CompetitionController competitionController, IEnumerable<CompetitionDTO> competitions, TeamController teamController)
        {
            //Número máximo de tentativas
            int maxAttempts = 3;
            //Atraso inicial em milissegundos
            int delayMs = 500;
            //Contador de tentativas
            int attempt = 1;

            //Salva os times no banco de dados
            foreach (var competition in competitions)
            {
                bool success = false;

                //Continua tentando inserir os times (no máximo 3 vezes) caso a inserção dos times não seja feita com sucesso
                while (!success && attempt <= maxAttempts)
                {
                    try
                    {
                        // Introduz um atraso de segundos entre as chamadas
                        await Task.Delay(delayMs);

                        //Busca todos os times de uma determinada competição
                        IEnumerable<TeamDTO> teams = await competitionController.GetAllCompetitionTeams(competition.CompetitionCode);

                        // Introduz um atraso de segundos entre as chamadas
                        await Task.Delay(delayMs);

                        //Salva os times no banco de dados
                        string insertTeamsResponse = await teamController.SaveTeams(teams, competition);

                        Console.WriteLine(insertTeamsResponse);

                        success = true;
                    }
                    catch (Exception /*e*/)
                    {
                        //if (e is HttpRequestException && e.Message.Contains("429"))
                        //{
                        //Acrescenta 5 segundos de espera a cada tentativa
                        delayMs += 5000;
                        //}
                        //else
                        //{
                        //    throw new AppException("Erro: " + e.Message);
                        //}
                    }
                }
            }
        }

        public string GetInterestInFavoritesOrNewInformationsOrScores(bool isFavorites, bool isScores = false)
        {
            string message = "Deseja olhar a informações do seus favoritos? (Sim/Não)";

            if (!isFavorites && !isScores)
            {
                message = "Deseja obter novas informações? (Sim/Não)";
            }

            if (isScores)
            {
                message = "Deseja saber o top de artilheiros dessa competição? (Sim/Não)";
            }

            string response = "";

            while ((response.ToLower() != "sim" && response.ToLower() != "não"
            && response.ToLower() != "nao" && !response.ToLower().Equals("s")
            && !response.ToLower().Equals("n"))/* || input == " "*/)
            {
                //Obtém a resposta de interesse nos favoritos
                Console.WriteLine(message);

                response = Console.ReadLine();

                Console.WriteLine();

                if (response.ToLower() != "sim" && response.ToLower() != "não"
                    && response.ToLower() != "nao" && !response.ToLower().Equals("s")
                    && !response.ToLower().Equals("n"))
                {
                    Console.WriteLine("Essa opção de resposta não existe. Por favor, escreva novamente.");
                    Console.WriteLine();
                }
            }

            return response;
        }

        public string GetInterestPlayerOrTeamFavorites()
        {
            string informationResponse = "";

            while (informationResponse.ToLower() != "jogador" && informationResponse.ToLower() != "time")
            {
                //Obtém o interesse dos favoritos, jogador ou time
                Console.WriteLine("Deseja obter informações de um time ou jogador? (Jogador ou Time)");

                informationResponse = Console.ReadLine();
                Console.WriteLine();

                if (informationResponse.ToLower() != "jogador" && informationResponse.ToLower() != "time")
                {
                    Console.WriteLine("Essa opção de resposta não existe. Por favor, escreva novamente.");
                    Console.WriteLine();
                }
            }

            return informationResponse;
        }

        public async Task SaveDataInFavorites(PlayerController playerController, TeamController teamController, bool isPlayer, PlayerDTO player, int? teamId)
        {
            string data = "time";

            if (isPlayer)
            {
                data = "jogador";
            }

            string saveResponse = "";

            while (saveResponse.ToLower() != "sim" && saveResponse.ToLower() != "não"
                && saveResponse.ToLower() != "nao" && !saveResponse.ToLower().Equals("n")
                && !saveResponse.ToLower().Equals("s"))
            {
                //Obtém o interesse de salvar o jogador nos favoritos
                Console.WriteLine($"Deseja guardar a informações desse {data} nos seus favoritos? (Sim/Não)");

                saveResponse = Console.ReadLine();
                Console.WriteLine();

                if (saveResponse.ToLower() == "sim" && saveResponse.ToLower() == "não"
                    && saveResponse.ToLower() == "nao" && saveResponse.ToLower().Equals("n")
                    && saveResponse.ToLower().Equals("s"))
                {
                    Console.WriteLine("Essa opção de resposta não existe. Por favor, escreva novamente.");
                    Console.WriteLine();
                }
            }

            if (saveResponse.ToLower() == "sim" || saveResponse.ToLower().Equals("s"))
            {
                string insertResponse;

                if (player != null)
                {
                    //Salva o jogador nos favoritos
                    insertResponse = await playerController.SaveFavoritePlayer(player);
                }
                else
                {
                    //Salva o time nos favoritos
                    insertResponse = await teamController.SaveFavoriteTeam(teamId.Value);
                }

                Console.WriteLine(insertResponse);
                Console.WriteLine();
            }
        }

        public async Task<object> GetTheInterestedData(CompetitionController competitionController, TeamController teamController, bool? isPlayer, bool? isTeam, bool? isCompetition, int? teamId, string competitionCode)
        {
            string message = ""; 
            string messageError= "";

            object data = null;

            if (isPlayer.Value)
            {
                message = "De qual jogador desse time deseja saber informações da atual temporada?";
                messageError = "Esse jogador não se encontra disponível nesse time. Por favor, escreva novamente.";
            }
            else if (isTeam.Value)
            {
                message = "De qual time dessa competição deseja saber informações da atual temporada?";
                messageError = "Esse time não se encontra disponível. Por favor, escreva novamente.";
            }
            else if (isCompetition.Value)
            {
                message = "De qual competição deseja saber informações da atual temporada?";
                messageError = "Essa competição não se encontra disponível. Por favor, escreva novamente.";
            }

            while (data == null)
            {
                //Obtém a informação de interesse
                Console.WriteLine(message);

                string required = Console.ReadLine();
                Console.WriteLine();

                if (isPlayer.Value)
                {
                    data = teamController.GetTeamInformation(teamId.Value).Squad.FirstOrDefault(p => p.Name == required);
                }
                else if (isTeam.Value)
                {
                    data = await teamController.GetTeam(required, competitionCode); 
                }
                else if (isCompetition.Value)
                {
                    data = await competitionController.GetCompetition(required);
                }

                if (data == null)
                {
                    Console.WriteLine(messageError);
                    Console.WriteLine();
                }
            }
            return data;
        }
    }
}
