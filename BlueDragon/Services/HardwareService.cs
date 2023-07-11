using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace BlueDragon.Services
{
    public class HardwareService
    {
        private readonly HccContext _context;

        public HardwareService(HccContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Hardware>> GetHardware()
        {
            return await _context.Hardwares.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Hardware?> GetSelectedHardware(Hardware model)
        {
            return await _context.Hardwares.FirstOrDefaultAsync(c => c.Hid == model.Hid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(Hardware model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Delete(Hardware model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
