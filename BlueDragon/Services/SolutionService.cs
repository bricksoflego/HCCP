using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services;
public class SolutionService(HccContext context) : ISolutionService
{
    private readonly HccContext _context = context;

    /// <summary>
    /// Retrieves a list of solution settings from the database, ordered by their name.
    /// </summary>
    /// <returns>A task that returns a list of SolutionSetting objects sorted by name.</returns>
    public async Task<List<SolutionSetting>> GetSolutionSetings()
    {
        return await _context.SolutionSettings.OrderBy(s => s.Name).ToListAsync();
    }

    /// <summary>
    /// Updates the value of an existing solution setting identified by its name. If the setting is found, 
    /// its IsEnabled property is updated and changes are saved to the database.
    /// </summary>
    /// <param name="name">The name of the solution setting to update.</param>
    /// <param name="value">The new value for the IsEnabled property.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Upsert(string name, bool value)
    {
        var setting = await _context.SolutionSettings.FirstOrDefaultAsync(s => s.Name == name);
        if (setting != null)
        {
            setting.IsEnabled = value;
            _context.Update(setting);
            await _context.SaveChangesAsync();
        }
    }
}