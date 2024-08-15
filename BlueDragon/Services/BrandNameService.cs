using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class BrandNameService
    {
        private readonly HccContext _context;

        public BrandNameService(HccContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<LuBrandName>> GetBrandNames()
        {
            return await _context.LuBrandNames.OrderBy(c => c.Name).ToListAsync(); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LuBrandName?> GetBrandName(LuBrandName model)
        {
            return await _context.LuBrandNames.FirstOrDefaultAsync(c => c.Id == model.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(LuBrandName model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Delete(LuBrandName model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
