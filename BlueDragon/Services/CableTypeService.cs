using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class CableTypeService
    {
        private readonly HccContext _context;

        public CableTypeService(HccContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<LuCableType>> GetCableTypes()
        {
            return await _context.LuCableTypes.OrderBy(c => c.Name).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LuCableType?> GetCableType(LuCableType model)
        {
            return await _context.LuCableTypes.FirstOrDefaultAsync(c => c.Id == model.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(LuCableType model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Delete(LuCableType model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
