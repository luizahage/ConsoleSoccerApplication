using ConsoleSoccerApplication.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleSoccerApplication.Data.Interfaces
{
    public interface ITeamRepository
    {
        Task<bool> InsertTeamsToDatabase(string connectionString, IEnumerable<TeamDTO> teams, int codeCompetitionId);
        Task<TeamDTO> GetFavoriteTeamFromDatabase(string connectionString, string codeTeamName);
        Task<bool> DeleteFavoriteTeamToDatabase(string connectionString, int teamIdCode);
        Task<TeamDTO> GetTeamFromDatabase(string connectionString, string codeTeamName, string competitionCode);
        Task<(bool, bool)> InsertFavoriteTeamToDatabase(string connectionString, int teamIdCode);
        Task<List<string>> GetFavoriteTeamsFromDatabase(string connectionString);
        Task<List<TeamDTO>> GetAllTeamsByCompetitionCodeFromDatabase(string connectionString, string competitionCode);
    }
}
