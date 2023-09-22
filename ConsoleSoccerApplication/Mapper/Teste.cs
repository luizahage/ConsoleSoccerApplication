using ConsoleSoccerApplication.Models.Entities;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using ConsoleSoccerApplication.Controllers;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using ConsoleSoccerApplication.Exceptions;
using ConsoleSoccerApplication.Models.Dtos;
using System.Linq;
using ConsoleSoccerApplication.Data.Repositories;
using ConsoleSoccerApplication.Data.Interfaces;

namespace ConsoleSoccerApplication.Mapper
{
    public class Teste
    {
        //var competitions = CompetitionController.GetAllAvailableCompetitions();

        //Console.WriteLine("Opções:");

        //foreach (var competition in competitions)
        //{
        //    Console.WriteLine($"{competition.CompetitionName} - {competition.CompetitionCode}");
        //}

        //Console.WriteLine();

        //Console.WriteLine("De qual competição você deseja saber informações na atual temporada? (Insira um dos códigos acima)");

        //string competitionCode = Console.ReadLine();

        //var teams = CompetitionController.GetAllCompetitionTeams(competitionCode);

        //Console.WriteLine("De qual time dessa competição você deseja saber informações na atual temporada?");

        //foreach (var team in teams)
        //{
        //    Console.WriteLine($"{team.TeamShortName} - {team.TeamTLA}");
        //}

        //int teamId = 1770;
        //int teamId = 1783;

        //var teamInfo = TeamController.GetTeamInformation(teamId);
        //var teamMatches = TeamController.GetTeamMatches(teamId).Matches.Where(m => m.Competition.CompetitionCode.Equals("BSA"));

        //int matchId = 433655;
        //var matchInfo = MatchController.GetMatchInformation(matchId);

        //int playerId = 1783;/* 16271;*/
        //var playerInfo = PlayerController.GetPlayerInformation(playerId);

        //playerId = 1783;
        //var playerMatches = PlayerController.GetPlayerMatches(playerId);

        //var competitionTable = CompetitionController.GetCompetitionTable(competitionCode);
        //var topScores = CompetitionController.GetTopScores(competitionCode);
        //Console.WriteLine("De qual jogador desse time você deseja saber mais informações da atual temporada?");

        //Console.WriteLine("Deseja salvar/excluir as informações desse jogador nos seus favoritos?");

        //Console.WriteLine("Deseja salvar ou excluir (Digite: S para Salvar / E para Excluir)?");



        //--------------------------------------------------------BANCO DE DADOS------------------------------------------------------------

        //Define uma string de conexão com o banco de dados
        //string connectionString = "Server = LUIZAHAGE\\SQLEXPRESS; Database = ConsoleSoccerApplication; User Id = sa; Password = 123456";

        ////Cria um novo objeto SqlConnection object usando a string de conexão
        //SqlConnection sqlConnection = new SqlConnection(connectionString);

        ////INSERT
        //try
        //{
        //    //Abre a conexão
        //    sqlConnection.Open();

        //    //Define o objeto SqlCommand e a instrução SQL
        //    //SqlCommand command = new SqlCommand("SELECT * FROM Competions ORDER BY  CompetionName", sqlConnection);
        //    int externalCompetitionId = 2001;
        //    SqlCommand command = new SqlCommand(
        //        @"IF NOT EXISTS (
        //            SELECT * FROM Competition
        //            WHERE ExternalCompetitionID = @externalCompetitionId)
        //        BEGIN
        //            INSERT INTO Competition(ExternalCompetitionID, CompetitionName, Code, CreationDate) 
        //            VALUES(2021, 'Premier League', 'PL', @creationDate) 
        //        END", sqlConnection);
        //    command.Parameters.AddWithValue("@creationDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF"));
        //    command.Parameters.AddWithValue("@externalCompetitionId", externalCompetitionId);

        //    //Define o objeto SqlDataReader e executa o método ExecuteReader()
        //    SqlDataReader dataReader = command.ExecuteReader();

        //    //Percorre o SqlDataReader para obter os dados
        //    //List<string> listDatas = new List<string>();

        //    while (dataReader.Read())
        //    {
        //        //listDatas.Add(dataReader[0] + " - " + dataReader["CompetitionName"]);
        //        ReadSingleRow((IDataRecord)dataReader);
        //    }
        //}
        //catch (SqlException e)
        //{
        //    throw new AppException("SQL Erro: " + e.Message);
        //}
        //finally
        //{
        //    //Fecha a conexão
        //    sqlConnection.Close();
        //}

        ////UPDATE
        //try
        //{
        //    using (SqlConnection sqlConnection1 = new SqlConnection(connectionString))
        //    {
        //        //Abre a conexão
        //        sqlConnection1.Open();

        //        // Define o objeto SqlCommand e a instrução SQL
        //        int externalCompetitionId = 2021;
        //        SqlCommand command = new SqlCommand(
        //            @"IF EXISTS (
        //                SELECT * FROM Competition
        //                WHERE ExternalCompetitionID = @externalCompetitionId)
        //            BEGIN
        //                UPDATE Competition
        //                SET Code = 'P'
        //                WHERE ExternalCompetitionID = @externalCompetitionId
        //            END", sqlConnection1);
        //        command.Parameters.AddWithValue("@externalCompetitionId", externalCompetitionId);

        //        // Executa a consulta e não retorna nenhuma coleção. Retorna o número de registros afetados.
        //        int rowAffected = command.ExecuteNonQuery();
        //    }
        //}
        //catch (SqlException e)
        //{
        //    throw new AppException("SQL Erro: " + e.Message);
        //}

        ////DELETE
        //try
        //{
        //    using (SqlConnection sqlConnection2 = new SqlConnection(connectionString))
        //    {
        //        // Abre a conexão
        //        sqlConnection2.Open();

        //        // Define o objeto SqlCommand e a instrução SQL
        //        int externalCompetitionId = 2021;
        //        SqlCommand command = new SqlCommand(
        //            @"IF EXISTS (
        //                SELECT * FROM Competition
        //                WHERE ExternalCompetitionID = @externalCompetitionId)
        //            BEGIN
        //                DELETE FROM Competition WHERE ExternalCompetitionID = @externalCompetitionId
        //            END", sqlConnection2);
        //        command.Parameters.AddWithValue("@externalCompetitionId", externalCompetitionId);

        //        // Executa a consulta e não retorna nenhuma coleção. Retorna o número de registros afetados.
        //        int rowAffected = command.ExecuteNonQuery();
        //    }
        //}
        //catch (SqlException e)
        //{
        //    throw new Exception("SQL Erro: " + e.Message);
        //}

        //CompetitionRepository.InsertCompetitionsToDatabase();
        //CompetitionRepository.GetCompetitions("Server = LUIZAHAGE\\SQLEXPRESS; Database = ConsoleSoccerApplication; User Id = sa; Password = 123456");

        //List<string> competitionsTeste = CompetitionService.ListCompetitions();


        // Criar uma instância do CompetitionRepository (ou usar injeção de dependência)
        //ICompetitionRepository competitionRepository = new CompetitionRepository();

        // Criar uma instância do CompetitionController, passando o CompetitionRepository como parâmetro
        //CompetitionController competitionController = new CompetitionController(competitionRepository);

        // Chamar o método do Repository
        //var competitionsTeste = competitionController.ListCompetitions();

        //// Fazer algo com o resultado
        //foreach (var competition in competitions)
        //{
        //    Console.WriteLine(competition);
        //}

        // Aguardar a tecla Enter ser pressionada antes de encerrar o programa
        //Console.ReadLine();




        //try
        //{
        //    SqlConnection sqlConn = new SqlConnection(connectionString);
        //    cb.DataSource = "your_server.database.windows.net";
        //    cb.UserID = "your_user";
        //    cb.Password = "your_password";
        //    cb.InitialCatalog = "your_database";

        //    using (var connection = new SqlConnection(cb.ConnectionString))
        //    {
        //        connection.Open();

        //        Submit_Tsql_NonQuery(connection, "2 - Create-Tables", Build_2_Tsql_CreateTables());

        //        Submit_Tsql_NonQuery(connection, "3 - Inserts", Build_3_Tsql_Inserts());

        //        Submit_Tsql_NonQuery(connection, "4 - Update-Join", Build_4_Tsql_UpdateJoin(),
        //            "@csharpParmDepartmentName", "Accounting");

        //        Submit_Tsql_NonQuery(connection, "5 - Delete-Join", Build_5_Tsql_DeleteJoin(),
        //            "@csharpParmDepartmentName", "Legal");

        //        Submit_6_Tsql_SelectEmployees(connection);
        //    }
        //}
        //catch (SqlException e)
        //{
        //    Console.WriteLine(e.ToString());
        //}

        //Console.WriteLine("View the report output here, then press any key to end the program...");
        //Console.ReadKey();


        //var builder = new MySqlConnectionStringBuilder
        //{
        //    Server = "YOUR-SERVER.mysql.database.azure.com",
        //    Database = "YOUR-DATABASE",
        //    UserID = "USER@YOUR-SERVER",
        //    Password = "PASSWORD",
        //    SslMode = MySqlSslMode.Required,
        //};

        //using (var conn = new MySqlConnection(builder.ConnectionString))
        //{
        //    Console.WriteLine("Opening connection");
        //    await conn.OpenAsync();

        //    using (var command = conn.CreateCommand())
        //    {
        //        command.CommandText = "DROP TABLE IF EXISTS inventory;";
        //        await command.ExecuteNonQueryAsync();
        //        Console.WriteLine("Finished dropping table (if existed)");

        //        command.CommandText = "CREATE TABLE inventory (id serial PRIMARY KEY, name VARCHAR(50), quantity INTEGER);";
        //        await command.ExecuteNonQueryAsync();
        //        Console.WriteLine("Finished creating table");

        //        command.CommandText = @"INSERT INTO inventory (name, quantity) VALUES (@name1, @quantity1),
        //            (@name2, @quantity2), (@name3, @quantity3);";
        //        command.Parameters.AddWithValue("@name1", "banana");
        //        command.Parameters.AddWithValue("@quantity1", 150);
        //        command.Parameters.AddWithValue("@name2", "orange");
        //        command.Parameters.AddWithValue("@quantity2", 154);
        //        command.Parameters.AddWithValue("@name3", "apple");
        //        command.Parameters.AddWithValue("@quantity3", 100);

        //        int rowCount = await command.ExecuteNonQueryAsync();
        //        Console.WriteLine(String.Format("Number of rows inserted={0}", rowCount));
        //    }

        //    // connection will be closed by the 'using' block
        //    Console.WriteLine("Closing connection");
        //}

        //Console.WriteLine("Press RETURN to exit");
        //Console.ReadLine();



        }

        //private static void ReadSingleRow(IDataRecord dataRecord)
        //{
        //    Console.WriteLine(String.Format("{0}, {1}", dataRecord[0], dataRecord[1]));
        //}

        //[HttpGet]
        //public async Task<IActionResult> ListarFormaDePagamento(string clienteId)
        //{
        //    var ApiKey = IuguClient.Properties.ApiKey;

        //    try
        //    {
        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Get,
        //            RequestUri = new Uri("https://api.iugu.com/v1/customers/" + clienteId + "?api_token=" + ApiKey),
        //            Headers =
        //            {
        //                { "Accept", "application/json" },
        //            },
        //        };
        //        using (var response = await client.SendAsync(request))
        //        {
        //            response.EnsureSuccessStatusCode();
        //            var body = await response.Content.ReadAsStringAsync();
        //            Console.WriteLine(body);
        //            return Json(body);
        //        }
        //    }
        //    catch (System.Exception e)
        //    {

        //        return Json(ResponseObject.ErrorResponse(message: e.Message));
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> MatchExterno([FromBody] MatchExternoViewModel matchExternoBody)
        //{
        //    try
        //    {
        //        var baseUrl = Utils.Settings.MatchExternoSettings.BaseURL;
        //        var token = Utils.Settings.MatchExternoSettings.Token;

        //        var data = new StringContent(JsonConvert.SerializeObject(matchExternoBody), Encoding.UTF8, "application/json");

        //        HttpClient client = new HttpClient();

        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

        //        HttpResponseMessage response = await client.PostAsync($"{baseUrl}pre-match/", data);


        //        var body = await response.Content.ReadAsStringAsync();
        //        var json = JsonConvert.DeserializeObject<MatchExternoRetornoViewModel>(body);

        //        client.Dispose();
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return Json(ResponseObject.SuccessResponse(json.result, Mensagem.Sucesso));
        //        }
        //        else
        //        {
        //            return Json(ResponseObject.ErrorResponse(message: json.detail.ToString()));
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        return Json(ResponseObject.ErrorResponse(message: e.Message));
        //    }
        //}

        //[HttpGet]
        //public async Task<IActionResult> ListarFormaDePagamento(string clienteId)
        //{
        //    var ApiKey = IuguClient.Properties.ApiKey;

        //    try
        //    {
        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Get,
        //            RequestUri = new Uri("https://api.iugu.com/v1/customers/" + clienteId + "?api_token=" + ApiKey),
        //            Headers =
        //            {
        //                { "Accept", "application/json" },
        //            },
        //        };
        //        using (var response = await client.SendAsync(request))
        //        {
        //            response.EnsureSuccessStatusCode();
        //            var body = await response.Content.ReadAsStringAsync();
        //            Console.WriteLine(body);
        //            return Json(body);
        //        }
        //    }
        //    catch (System.Exception e)
        //    {

        //        return Json(ResponseObject.ErrorResponse(message: e.Message));
        //    }
        //}

        //[HttpPost]
        //public async Task<IActionResult> MatchExterno([FromBody] MatchExternoViewModel matchExternoBody)
        //{
        //    try
        //    {
        //        var baseUrl = Utils.Settings.MatchExternoSettings.BaseURL;
        //        var token = Utils.Settings.MatchExternoSettings.Token;

        //        var data = new StringContent(JsonConvert.SerializeObject(matchExternoBody), Encoding.UTF8, "application/json");

        //        HttpClient client = new HttpClient();

        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", token);

        //        HttpResponseMessage response = await client.PostAsync($"{baseUrl}pre-match/", data);


        //        var body = await response.Content.ReadAsStringAsync();
        //        var json = JsonConvert.DeserializeObject<MatchExternoRetornoViewModel>(body);

        //        client.Dispose();
        //        if (response.IsSuccessStatusCode)
        //        {
        //            return Json(ResponseObject.SuccessResponse(json.result, Mensagem.Sucesso));
        //        }
        //        else
        //        {
        //            return Json(ResponseObject.ErrorResponse(message: json.detail.ToString()));
        //        }
        //    }

        //    catch (Exception e)
        //    {
        //        return Json(ResponseObject.ErrorResponse(message: e.Message));
        //    }
        //}
}
