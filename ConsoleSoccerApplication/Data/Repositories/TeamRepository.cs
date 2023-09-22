using ConsoleSoccerApplication.Data.Interfaces;
using ConsoleSoccerApplication.Exceptions;
using ConsoleSoccerApplication.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleSoccerApplication.Data.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        public async Task<bool> InsertTeamsToDatabase(string connectionString, IEnumerable<TeamDTO> teams, int competitionIdCode)
        {
            //Cria um novo objeto SqlConnection usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Insert dos times
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Select da competição no banco de dados
                //Define o objeto SqlCommand e a instrução SQL
                int externalCompetitionId = competitionIdCode;
                SqlCommand command = new SqlCommand(
                    @"SELECT * FROM Competition
                    WHERE ExternalCompetitionID = @externalCompetitionId", sqlConnection);

                //Adiciona o parâmetro necessário na instrução SQL
                command.Parameters.AddWithValue("@externalCompetitionId", externalCompetitionId);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                if (dataReader.HasRows)
                {
                    //Move para a primeira linha de dados da consulta
                    await dataReader.ReadAsync();

                    //Define a variável competitionId que será inserida no banco de dados
                    int competitionId = dataReader.GetInt32(0);

                    //Fecha o objeto SqlDataReader
                    dataReader.Close();

                    //Percorre os times da competição
                    foreach (var team in teams)
                    {
                        //Define as variáveis que serão inseridas no banco de dados
                        int externalTeamId = team.TeamId;
                        string teamName = team.TeamName;
                        string teamShortName = team.TeamShortName;
                        string teamTLA = team.TeamTLA ?? "";
                        string creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF");

                        //Define o objeto SqlCommand e a instrução SQL
                        SqlCommand command2 = new SqlCommand(
                            @"IF NOT EXISTS (
                            SELECT * FROM Team
                            WHERE ExternalTeamID = @externalTeamId AND CompetitionID = @competitionId)
                        BEGIN
                            INSERT INTO Team(ExternalTeamID, TeamName, ShortName, TLA, CompetitionID, CreationDate) 
                            VALUES(@externalTeamId, @teamName, @teamShortName, @teamTLA, @competitionId, @creationDate) 
                        END", sqlConnection);

                        //Adiciona os parâmetros necessários na instrução SQL
                        command2.Parameters.AddWithValue("@externalTeamId", externalTeamId);
                        command2.Parameters.AddWithValue("@teamName", teamName);
                        command2.Parameters.AddWithValue("@teamShortName", teamShortName ?? (object)DBNull.Value);
                        command2.Parameters.AddWithValue("@teamTLA", teamTLA);
                        command2.Parameters.AddWithValue("@competitionId", competitionId);
                        command2.Parameters.AddWithValue("@creationDate", creationDate);

                        //Executa o comando de inserção
                        int rowAffected = command2.ExecuteNonQuery();

                        //Verifica se uma linha foi afetada, ou seja, se a informação (linha) foi inserida
                        if (rowAffected <= 0)
                        {
                            //Retorna falso, caso nenhuma linha tenha sido inserido (operação mal sucedida)
                            return false;
                        }
                    }

                    //Retorna verdadeiro, se todas as linhas foram inseridas (operação bem sucedida)
                    return true;
                }
                else
                {
                    //Retorna falso caso não tenha sido encontrada a competição a qual esses times pertencem (operação mal sucedida)
                    return false;
                }
            }
            catch (SqlException /*e*/)
            {
                //throw new AppException("SQL Erro: " + e.Message);

                //Retorna falso caso tenha ocorrido algum erro ao tentar inserir os times (operação mal sucedida)
                return false;
            }
            finally
            {
                //Fecha a conexão
                sqlConnection.Close();
            }
        }

        public async Task<TeamDTO> GetFavoriteTeamFromDatabase(string connectionString, string codeTeamName)
        {
            //Instancia o DTO de retorno
            TeamDTO team = null;

            //Cria um novo objeto SqlConnection object usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Select do time favorito com determinado código
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Define a variável que será usada como parâmetro para o Select
                string teamName = codeTeamName;

                //Select do time com o nome determinado
                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"SELECT * FROM Team
                    WHERE TeamName = @teamName OR ShortName = @teamName", sqlConnection);

                //Adiciona o parâmetro necessário na instrução SQL
                command.Parameters.AddWithValue("@teamName", teamName);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                if (dataReader.HasRows)
                {
                    //Fecha o objeto SqlDataReader
                    dataReader.Close();

                    //Select do time favorito com o id associado ao nome determinado
                    //Define o objeto SqlCommand e a instrução SQL
                    SqlCommand command2 = new SqlCommand(
                        @"IF EXISTS (
                            SELECT * FROM FavoriteTeam
                            WHERE TeamID IN (SELECT ID FROM Team WHERE TeamName = @teamName OR ShortName = @teamName))
                        BEGIN
                            SELECT * FROM Team
                            WHERE ID IN (SELECT TeamID FROM FavoriteTeam
                            WHERE TeamID IN (SELECT ID FROM Team WHERE TeamName = @teamName OR ShortName = @teamName))
                        END", sqlConnection);

                    //Adiciona o parâmetro necessário na instrução SQL
                    command2.Parameters.AddWithValue("@teamName", teamName);

                    //Define o objeto SqlDataReader e executa o método ExecuteReader()
                    SqlDataReader dataReader2 = await command2.ExecuteReaderAsync();

                    //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                    if (dataReader2.HasRows)
                    {
                        while(await dataReader2.ReadAsync())
                        {
                            team = new TeamDTO
                            {
                                TeamId = dataReader2.GetInt32(1),
                                TeamName = dataReader2.GetString(2)
                            };
                        }
                    }

                    //Fecha o objeto SqlDataReader
                    dataReader2.Close();
                }              
            }
            catch (SqlException e)
            {
                throw new AppException("SQL Erro: " + e.Message);
            }
            finally
            {
                //Fecha a conexão
                sqlConnection.Close();
            }

            return team;
        }

        public async Task<bool> DeleteFavoriteTeamToDatabase(string connectionString, int teamIdCode)
        {
            //Delete de um jogador favorito do banco de dados
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    //Abre a conexão
                    await sqlConnection.OpenAsync();

                    //Instancia a lista de ids do time
                    List<int> listTeamIds = new List<int>();

                    //Select do time
                    //Define a variável que será usada como parâmetro para o Select
                    int externalTeamId = teamIdCode;

                    //Define o objeto SqlCommand e a instrução SQL
                    SqlCommand command = new SqlCommand(
                        @"SELECT * FROM Team
                    WHERE ExternalTeamID = @externalTeamId", sqlConnection);

                    //Adiciona o parâmetro necessário na instrução SQL
                    command.Parameters.AddWithValue("@externalTeamId", externalTeamId);

                    //Define o objeto SqlDataReader e executa o método ExecuteReader()
                    SqlDataReader dataReader = await command.ExecuteReaderAsync();

                    //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                    if (dataReader.HasRows)
                    {
                        //Percorre o SqlDataReader para obter os dados da consulta
                        while (await dataReader.ReadAsync())
                        {
                            //Define a variável que será usada como parâmetro para o Delete
                            listTeamIds.Add(dataReader.GetInt32(0));
                        }

                        //Fecha o objeto SqlDataReader
                        dataReader.Close();

                        //Percorre a lista de ids do time no banco de dados
                        foreach (int teamId in listTeamIds)
                        { 

                            // Define o objeto SqlCommand e a instrução SQL
                            SqlCommand command2 = new SqlCommand(
                                @"IF EXISTS (
                                    SELECT * FROM FavoriteTeam
                                    WHERE TeamID = @teamId)
                                BEGIN
                                    DELETE FROM FavoriteTeam WHERE TeamID = @teamId
                                END", sqlConnection);

                            //Executa o comando de exclusão
                            command2.Parameters.AddWithValue("@teamId", teamId);

                            //Verifica se uma linha foi afetada, ou seja, se a informação (linha) foi deletada
                            int rowAffected = command2.ExecuteNonQuery();

                            //Informa se o delete foi feito com sucesso
                            if (rowAffected <= 0)
                            {
                                //Retorna falso, caso nenhuma linha tenha sido deletada (operação mal sucedida)
                                return false;
                            }
                        }

                        //Retorna verdadeiro, se as linhas foram deletadas (operação bem sucedida)
                        return true;
                    }
                    else
                    {
                        //Retorna falso, caso não tenha sido encontrada nenhuma competição com o id informado
                        return false;
                    }

                }
            }
            catch (SqlException e)
            {
                throw new Exception("SQL Erro: " + e.Message);
            }
        }

        public async Task<TeamDTO> GetTeamFromDatabase(string connectionString, string codeTeamName, string competitionCode)
        {
            //Instancia o DTO de retorno
            TeamDTO team = null;

            //Cria um novo objeto SqlConnection usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Select do time com determinado nome
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Define a variável que será usada como parâmetro para o Select
                string teamName = codeTeamName;
                string code = competitionCode;

                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"SELECT * FROM Team
                WHERE TeamName = @teamName OR ShortName = @teamName AND CompetitionId = (SELECT ID FROM Competition WHERE Code = @code)", sqlConnection);

                //Adiciona o parâmetro necessário na instrução SQL
                command.Parameters.AddWithValue("@teamName", teamName);
                command.Parameters.AddWithValue("@code", code);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                if (dataReader.HasRows)
                {
                    //Percorre o SqlDataReader para obter os dados da consulta
                    while (await dataReader.ReadAsync())
                    {
                        team = new TeamDTO
                        {
                            TeamId = dataReader.GetInt32(1),
                            TeamName = dataReader.GetString(2)
                        };
                    }
                }

                //Fecha o objeto SqlDataReader
                dataReader.Close();
            }
            catch (SqlException e)
            {
                throw new AppException("SQL Erro: " + e.Message);
            }
            finally
            {
                //Fecha a conexão
                sqlConnection.Close();
            }

            return team;
        }

        public async Task<(bool, bool)> InsertFavoriteTeamToDatabase(string connectionString, int teamIdCode)
        {
            //Cria um novo objeto SqlConnection usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Define a lista de ids do time
                List<int> listTeamIds = new List<int>();

                //Select do time no banco de dados
                //Define a variável que será usada como parâmetro para o Select no Team
                int externalTeamId = teamIdCode;

                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"SELECT * FROM Team
                    WHERE ExternalTeamID = @externalTeamId", sqlConnection);

                //Adiciona o parâmetro necessário na instrução SQL
                command.Parameters.AddWithValue("@externalTeamId", externalTeamId);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                if (dataReader.HasRows)
                {
                    //Insert do time nos favoritos
                    //Define as variáveis que serão inseridas no banco de dados
                    while (await dataReader.ReadAsync())
                    {
                        //Define a variável que será usada como parâmetro para o Delete
                        listTeamIds.Add(dataReader.GetInt32(0));
                    }
                        
                    string creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF");

                    //Fecha o objeto SqlDataReader
                    dataReader.Close();

                    //Percorre a lista de ids do time
                    foreach (var teamId in listTeamIds)
                    {
                        //Define o objeto SqlCommand e a instrução SQL
                        SqlCommand command2 = new SqlCommand(
                            @"IF NOT EXISTS (
                            SELECT * FROM FavoriteTeam
                            WHERE TeamID = @teamId)
                        BEGIN
                            INSERT INTO FavoriteTeam(TeamID, CreationDate) 
                            VALUES(@teamId, @creationDate) 
                        END", sqlConnection);

                        //Adiciona os parâmetros necessários na instrução SQL
                        command2.Parameters.AddWithValue("@teamId", teamId);
                        command2.Parameters.AddWithValue("@creationDate", creationDate);

                        //Executa o comando de inserção
                        int rowAffected = command2.ExecuteNonQuery();

                        //Verifica se uma linha foi afetada, ou seja, se a informação (linha) foi inserida
                        if (rowAffected <= 0)
                        {
                            bool alreadyExists = false;

                            //Define o objeto SqlCommand e a instrução SQL
                            SqlCommand command3 = new SqlCommand(
                                @"SELECT * FROM FavoriteTeam
                            WHERE TeamID = @teamId", sqlConnection);

                            //Adiciona o parâmetro necessário na instrução SQL
                            command3.Parameters.AddWithValue("@teamId", teamId);

                            //Define o objeto SqlDataReader e executa o método ExecuteReader()
                            SqlDataReader dataReader3 = await command3.ExecuteReaderAsync();

                            //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                            if (dataReader3.HasRows)
                            {
                                //Percorre o SqlDataReader para obter os dados da consulta
                                while (await dataReader3.ReadAsync())
                                {
                                    if (teamId == dataReader3.GetInt32(1))
                                    {
                                        alreadyExists = true;
                                    }
                                }
                            }

                            //Fecha o objeto SqlDataReader
                            dataReader3.Close();

                            //Retorna false na primeira opção quando a informação não foi inserida no banco (operação mal sucedida)
                            //E retorna true na segunda opção quando a informação não foi inserida por já existir no banco de dados
                            return (false, alreadyExists);
                        }
                    }

                    //Retorna true na primeira opção quando a informação foi inserida no banco (operação bem sucedida)
                    //E retorna false na segunda opção quando a informação não existia no banco de dados
                    return (true, false);
                }
                else
                {
                    //Retorna false e false, caso não tenha sido encontrado nenhum time com o id informado
                    return (false, false);
                }
            }
            catch (SqlException e)
            {
                throw new AppException("SQL Erro: " + e.Message);

                //Retorna false caso tenha ocorrido algum erro ao tentar inserir os times nos favoritos (operação mal sucedida)
                //return (false, false);
            }
            finally
            {
                //Fecha a conexão
                sqlConnection.Close();
            }
        }

        public async Task<List<string>> GetFavoriteTeamsFromDatabase(string connectionString)
        {
            //Instancia a lista de ids
            List<int> listTeamsIds = new List<int>();

            //Instancia a lista de string de retorno
            List<string> listTeams = new List<string>();

            //Cria um novo objeto SqlConnection usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Select de todos os times favoritos disponíveis
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"SELECT TeamID FROM FavoriteTeam", sqlConnection);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                if (dataReader.HasRows)
                {
                    //Percorre o SqlDataReader para obter os dados da consulta
                    while (await dataReader.ReadAsync())
                    {
                        int teamId = dataReader.GetInt32(0);

                        listTeamsIds.Add(teamId);
                    }

                    //Fecha o objeto SqlDataReader
                    dataReader.Close();

                    //Percorre 
                    foreach (var teamId in listTeamsIds)
                    {
                        //Define o objeto SqlCommand e a instrução SQL
                        SqlCommand command2 = new SqlCommand(
                            @"SELECT TeamName, ShortName FROM Team
                         WHERE ID = @teamId", sqlConnection);
                        command2.Parameters.AddWithValue("@teamId", teamId);

                        //Define o objeto SqlDataReader e executa o método ExecuteReader()
                        SqlDataReader dataReader2 = await command2.ExecuteReaderAsync();

                        //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                        if (dataReader2.HasRows)
                        {
                            //Percorre o SqlDataReader para obter os dados da consulta
                            while (await dataReader2.ReadAsync())
                            {
                                string teamName = dataReader2.GetString(0);
                                string teamShortName = dataReader2.GetString(1);

                                listTeams.Add($"{teamName} - {teamShortName}");
                            }
                        }

                        //Fecha o objeto SqlDataReader
                        dataReader2.Close();
                    }
                }
            }
            catch (SqlException e)
            {
                throw new AppException("SQL Erro: " + e.Message);
            }
            finally
            {
                //Fecha a conexão
                sqlConnection.Close();
            }

            return listTeams.Distinct().ToList();
        }

        public async Task<List<TeamDTO>> GetAllTeamsByCompetitionCodeFromDatabase(string connectionString, string competitionCode)
        {
            List<TeamDTO> teams = new List<TeamDTO>();

            //Cria um novo objeto SqlConnection usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Select das competições no banco de dados
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"SELECT * FROM Competition WHERE Code = @competitionCode", sqlConnection);

                //Adiciona o parâmetro necessário na instrução SQL
                command.Parameters.AddWithValue("@competitionCode", competitionCode);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                if (dataReader.HasRows)
                {
                    int competitionId = 0;

                    //Percorre o SqlDataReader para obter os dados da consulta
                    while (await dataReader.ReadAsync())
                    {
                        competitionId = dataReader.GetInt32(0);
                    }

                    //Fecha o objeto SqlDataReader
                    dataReader.Close();

                    //Define o objeto SqlCommand e a instrução SQL
                    SqlCommand command2 = new SqlCommand(
                        @"SELECT * FROM Team WHERE CompetitionID = @competitionId", sqlConnection);

                    //Adiciona o parâmetro necessário na instrução SQL
                    command2.Parameters.AddWithValue("@competitionId", competitionId);

                    //Define o objeto SqlDataReader e executa o método ExecuteReader()
                    SqlDataReader dataReader2 = await command2.ExecuteReaderAsync();

                    //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                    if (dataReader2.HasRows)
                    {
                        //Percorre o SqlDataReader para obter os dados da consulta
                        while (await dataReader2.ReadAsync())
                        {
                            TeamDTO team = new TeamDTO
                            {
                                TeamId = dataReader2.GetInt32(1),
                                TeamName = dataReader2.GetString(2),
                                TeamShortName = !dataReader2.IsDBNull(3) ? dataReader2.GetString(3) : null 
                            };

                            teams.Add(team);
                        }
                    }

                    //Fecha o objeto SqlDataReader
                    dataReader2.Close();
                }
            }
            catch (SqlException e)
            {
                throw new AppException("SQL Erro: " + e.Message);
            }
            finally
            {
                //Fecha a conexão
                sqlConnection.Close();
            }

            return teams;
        }
    }
}
