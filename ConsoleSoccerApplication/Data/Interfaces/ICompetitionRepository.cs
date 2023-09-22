using ConsoleSoccerApplication.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleSoccerApplication.Data.Interfaces
{
    public interface ICompetitionRepository
    {
        Task<bool> InsertCompetitionsToDatabase(string connectionString, IEnumerable<CompetitionDTO> competitions);
        Task<List<CompetitionDTO>> GetAllCompetitionsFromDatabase(string connectionString);
        Task<CompetitionDTO> GetCompetitionFromDatabase(string connectionString, string codeCompetitionName);
        Task<CompetitionDTO> GetCompetitionByCodeFromDatabase(string connectionString, string externalCompetitionCode);
    }
}
