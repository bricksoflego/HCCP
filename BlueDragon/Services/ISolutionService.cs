using BlueDragon.Models;

namespace BlueDragon.Services
{
    public interface ISolutionService
    {
        Task<List<SolutionSetting>> GetSolutionSetings();
        Task Upsert(string name, bool value);
    }
}
