using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class SolutionService(HccContext context)
    {
        private readonly HccContext _context = context;

        public async Task<List<SolutionSetting>> GetSolutionSetings()
        {
            return await _context.SolutionSettings.OrderBy(s => s.Name).ToListAsync();
        }

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
}

