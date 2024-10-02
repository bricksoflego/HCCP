using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class PeripheralService(HccContext context) : IPeripheralService
    {
        private readonly HccContext _context = context;

        public async Task<List<Peripheral>> GetPeripherals()
        {
            return await _context.Peripherals.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Peripheral?> GetPeripheral(Peripheral model)
        {
            return await _context.Peripherals.FirstOrDefaultAsync(c => c.Pcid == model.Pcid);
        }

        public async Task Upsert(Peripheral model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Peripheral model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
