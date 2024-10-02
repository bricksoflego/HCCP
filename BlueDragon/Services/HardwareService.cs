using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class HardwareService(HccContext context) : IHardwareService
    {
        private readonly HccContext _context = context;

        public async Task<List<Hardware>> GetHardware()
        {
            return await _context.Hardwares.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Hardware?> GetSelectedHardware(Hardware model)
        {
            return await _context.Hardwares.FirstOrDefaultAsync(c => c.Hid == model.Hid);
        }

        public async Task Upsert(Hardware model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Hardware model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
