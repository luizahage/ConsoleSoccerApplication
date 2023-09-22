using ConsoleSoccerApplication.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleSoccerApplication.Data.Interfaces
{
    public interface IPlayerRepository
    {
        Task<PlayerInfoDTO> GetFavoritePlayerFromDatabase(string connectionString, string codePlayerName);
        Task<bool> DeleteFavoritePlayerToDatabase(string connectionString, int playerIdCode);
        Task<(bool, bool)> InsertFavoritePlayerToDatabase(string connectionString, PlayerDTO player);
        Task<List<string>> GetFavoritePlayersFromDatabase(string connectionString);
    }
}
