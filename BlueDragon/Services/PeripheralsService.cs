using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class PeripheralService
    {
        private readonly HccContext _context;

        public PeripheralService(HccContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Peripheral>> GetPeripherals()
        {
            return await _context.Peripherals.OrderBy(c => c.Name).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Peripheral?> GetPeripheral(Peripheral model)
        {
            return await _context.Peripherals.FirstOrDefaultAsync(c => c.Pcid == model.Pcid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(Peripheral model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Delete(Peripheral model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
