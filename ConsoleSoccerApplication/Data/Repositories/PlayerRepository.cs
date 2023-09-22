using ConsoleSoccerApplication.Data.Interfaces;
using ConsoleSoccerApplication.Exceptions;
using ConsoleSoccerApplication.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsoleSoccerApplication.Data.Repositories
{
    public class PlayerRepository : IPlayerRepository
    {
        public async Task<PlayerInfoDTO> GetFavoritePlayerFromDatabase(string connectionString, string codePlayerName)
        {
            //Instancia o DTO de retorno
            PlayerInfoDTO player = null;

            //Cria um novo objeto SqlConnection object usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Select do jogador favorito com determinado nome
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Define a variável que será usada como parâmetro para o Select
                string playerName = codePlayerName;

                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"SELECT * FROM FavoritePlayer
                    WHERE PlayerName = @playerName", sqlConnection);
                command.Parameters.AddWithValue("@playerName", playerName);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        player = new PlayerInfoDTO
                        {
                            Id = dataReader.GetInt32(1),
                            Name = dataReader.GetString(2)
                        };
                    }
                }
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

            return player;
        }

        public async Task<bool> DeleteFavoritePlayerToDatabase(string connectionString, int playerIdCode)
        {
            //Delete de um jogador favorito do banco de dados
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    // Abre a conexão
                    await sqlConnection.OpenAsync();

                    //Define a variável que será usada como parâmetro para o Delete
                    int externalPlayerId = playerIdCode;

                    // Define o objeto SqlCommand e a instrução SQL
                    SqlCommand command = new SqlCommand(
                        @"IF EXISTS (
                            SELECT * FROM FavoritePlayer
                            WHERE ExternalPlayerID = @externalPlayerId)
                        BEGIN
                            DELETE FROM FavoritePlayer WHERE ExternalPlayerID = @externalPlayerId
                        END", sqlConnection);
                    command.Parameters.AddWithValue("@externalPlayerId", externalPlayerId);

                    // Executa a consulta e não retorna nenhuma coleção. Retorna o número de registros afetados.
                    int rowAffected = command.ExecuteNonQuery();

                    //Informa se o delete foi feito com sucesso
                    if (rowAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (SqlException e)
            {
                throw new Exception("SQL Erro: " + e.Message);
            }
        }

        public async Task<(bool, bool)> InsertFavoritePlayerToDatabase(string connectionString, PlayerDTO player)
        {
            //Cria um novo objeto SqlConnection object usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Insert do jogador nos favoritos
                //Define a variável que será usada como parâmetro para o Select
                int externalPlayerId = player.Id;
                string playerName = player.Name;
                string creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF");

                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"IF NOT EXISTS (
                    SELECT * FROM FavoritePlayer
                    WHERE ExternalPlayerID = @externalPlayerId)
                BEGIN
                    INSERT INTO FavoritePlayer(ExternalPlayerID, PlayerName, CreationDate) 
                    VALUES(@externalPlayerId, @playerName, @creationDate) 
                END", sqlConnection);
                command.Parameters.AddWithValue("@externalPlayerId", externalPlayerId);
                command.Parameters.AddWithValue("@playerName", playerName);
                command.Parameters.AddWithValue("@creationDate", creationDate);

                //Executa o comando de inserção
                int rowAffected = command.ExecuteNonQuery();

                //Verifica se uma linha foi afetada, ou seja, se a informação (linha) foi inserida
                if (rowAffected <= 0)
                {
                    bool alreadyExists = false;

                    //Define o objeto SqlCommand e a instrução SQL
                    SqlCommand command2 = new SqlCommand(
                        @"SELECT * FROM FavoritePlayer
                    WHERE ExternalPlayerID = @externalPlayerId", sqlConnection);

                    //Adiciona o parâmetro necessário na instrução SQL
                    command2.Parameters.AddWithValue("@externalPlayerId", externalPlayerId);

                    //Define o objeto SqlDataReader e executa o método ExecuteReader()
                    SqlDataReader dataReader = await command2.ExecuteReaderAsync();

                    //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                    if (dataReader.HasRows)
                    {
                        //Percorre o SqlDataReader para obter os dados da consulta
                        while (await dataReader.ReadAsync())
                        {
                            if (playerName == dataReader.GetString(2))
                            {
                                alreadyExists = true;
                            }
                        }
                    }

                    //Fecha o objeto SqlDataReader
                    dataReader.Close();

                    //Retorna false na primeira opção quando a informação não foi inserida no banco (operação mal sucedida)
                    //E retorna true na segunda opção quando a informação não foi inserida por já existir no banco de dados
                    return (false, alreadyExists);
                }
                else
                {
                    //Retorna true na primeira opção quando a informação foi inserida no banco (operação bem sucedida)
                    //E retorna false na segunda opção quando a informação não existia no banco de dados
                    return (true, false);
                }
            }
            catch (SqlException e)
            {
                throw new AppException("SQL Erro: " + e.Message);

                //Retorna false caso tenha ocorrido algum erro ao tentar inserir os jogadores nos favoritos (operação mal sucedida)
                //return (false, false);
            }
            finally
            {
                //Fecha a conexão
                sqlConnection.Close();
            }
        }

        public async Task<List<string>> GetFavoritePlayersFromDatabase(string connectionString)
        {
            //Instancia a lista de string de retorno
            List<string> listPlayers = new List<string>();

            //Cria um novo objeto SqlConnection object usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Select de todos os jogadores favoritos disponíveis
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"SELECT PlayerName FROM FavoritePlayer", sqlConnection);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        string team = dataReader.GetString(0);
                        listPlayers.Add(team);
                    }
                }
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

            return listPlayers;
        }
    }
}
