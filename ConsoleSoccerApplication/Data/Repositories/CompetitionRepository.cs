using ConsoleSoccerApplication.Data.Interfaces;
using ConsoleSoccerApplication.Exceptions;
using ConsoleSoccerApplication.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ConsoleSoccerApplication.Data.Repositories
{
    public class CompetitionRepository : ICompetitionRepository
    {
        public async Task<bool> InsertCompetitionsToDatabase(string connectionString, IEnumerable<CompetitionDTO> competitions)
        {
            //Cria um novo objeto SqlConnection usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Insert das competições
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                foreach (var competition in competitions)
                {
                    //Define as variáveis que serão inseridas no banco de dados
                    int externalCompetitionId = competition.CompetitionId;
                    string competitionName = competition.CompetitionName;
                    string competitionCode = competition.CompetitionCode;
                    string country = competition.AreaName;
                    string creationDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.FFF");

                    //Define o objeto SqlCommand e a instrução SQL
                    SqlCommand command = new SqlCommand(
                        @"IF NOT EXISTS (
                            SELECT * FROM Competition
                            WHERE ExternalCompetitionID = @externalCompetitionId)
                        BEGIN
                            INSERT INTO Competition(ExternalCompetitionID, CompetitionName, Code, Country, CreationDate) 
                            VALUES(@externalCompetitionId, @competitionName, @competitionCode, @country, @creationDate) 
                        END", sqlConnection);

                    //Adiciona os parâmetros necessários na instrução SQL
                    command.Parameters.AddWithValue("@externalCompetitionId", externalCompetitionId);
                    command.Parameters.AddWithValue("@competitionName", competitionName);
                    command.Parameters.AddWithValue("@competitionCode", competitionCode);
                    command.Parameters.AddWithValue("@country", country);
                    command.Parameters.AddWithValue("@creationDate", creationDate);

                    //Executa o comando de inserção
                    int rowAffected = command.ExecuteNonQuery();

                    //Verifica se uma linha foi afetada, ou seja, se a informação (linha) foi inserida
                    if (rowAffected <= 0)
                    {
                        //Retorna falso, caso nenhuma linha tenha sido inserida (operação mal sucedida)
                        return false;
                    }
                }

                //Retorna verdadeiro, se todas as linhas foram inseridas (operação bem sucedida)
                return true;
            }
            catch (SqlException /*e*/)
            {
                //throw new AppException("SQL Erro: " + e.Message);

                //Retorna false caso tenha ocorrido algum erro ao tentar inserir os times (operação mal sucedida)
                return false;
            }
            finally
            {
                //Fecha a conexão
                sqlConnection.Close();
            }
        }


        public async Task<List<CompetitionDTO>> GetAllCompetitionsFromDatabase(string connectionString)
        {
            //Instancia a lista de DTO de retorno
            List<CompetitionDTO> competitions = new List<CompetitionDTO>();

            //Cria um novo objeto SqlConnection usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Select das competições no banco de dados
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"SELECT * FROM Competition", sqlConnection);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                if (dataReader.HasRows)
                {
                    //Percorre o SqlDataReader para obter os dados da consulta
                    while (await dataReader.ReadAsync())
                    {
                        CompetitionDTO competition = new CompetitionDTO
                        {
                            CompetitionId = dataReader.GetInt32(1),
                            CompetitionName = dataReader.GetString(2),
                            CompetitionCode = dataReader.GetString(3),
                            AreaName = dataReader.GetString(4)
                            
                        };

                        competitions.Add(competition);
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

            return competitions;
        }

        public async Task<CompetitionDTO> GetCompetitionFromDatabase(string connectionString, string codeCompetitionName)
        {
            //Instancia o DTO de retorno
            CompetitionDTO competition = null;

            //Cria um novo objeto SqlConnection usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Select da competição com determinado nome
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Define a variável que será usada como parâmetro para o Select
                string competitionName = codeCompetitionName;

                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"SELECT * FROM Competition
                    WHERE CompetitionName = @competitionName", sqlConnection);

                //Adiciona o parâmetro necessário na instrução SQL
                command.Parameters.AddWithValue("@competitionName", competitionName);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                if (dataReader.HasRows)
                {
                    //Percorre o SqlDataReader para obter os dados da consulta
                    while (await dataReader.ReadAsync())
                    {
                        competition = new CompetitionDTO
                        {
                            CompetitionId = dataReader.GetInt32(1),
                            CompetitionName = dataReader.GetString(2),
                            CompetitionCode = dataReader.GetString(3)
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

            return competition;
        }

        public async Task<CompetitionDTO> GetCompetitionByCodeFromDatabase(string connectionString, string externalCompetitionCode)
        {
            //Instancia o DTO de retorno
            CompetitionDTO competition = null;

            //Cria um novo objeto SqlConnection usando a string de conexão
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //Select da competição com determinado código
            try
            {
                //Abre a conexão
                await sqlConnection.OpenAsync();

                //Define a variável que será usada como parâmetro para o Select
                string competitionCode = externalCompetitionCode;

                //Define o objeto SqlCommand e a instrução SQL
                SqlCommand command = new SqlCommand(
                    @"SELECT * FROM Competition
                    WHERE Code = @competitionCode", sqlConnection);

                //Adiciona o parâmetro necessário na instrução SQL
                command.Parameters.AddWithValue("@competitionCode", competitionCode);

                //Define o objeto SqlDataReader e executa o método ExecuteReader()
                SqlDataReader dataReader = await command.ExecuteReaderAsync();

                //Verifica se objeto SqlDataReader possui uma linha, ou seja, se a informação (linha) foi encontrada
                if (dataReader.HasRows)
                {
                    //Percorre o SqlDataReader para obter os dados da consulta
                    while (await dataReader.ReadAsync())
                    {
                        competition = new CompetitionDTO
                        {
                            CompetitionId = dataReader.GetInt32(1),
                            CompetitionName = dataReader.GetString(2),
                            CompetitionCode = dataReader.GetString(3)
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

            return competition;
        }
    }
}
