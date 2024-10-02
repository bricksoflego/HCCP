using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class BrandNameService(HccContext context) : IBrandNameService
    {
        private readonly HccContext _context = context;

        public async Task<List<LuBrandName>> GetBrandNames()
        {
            return await _context.LuBrandNames.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<LuBrandName?> GetBrandName(LuBrandName model)
        {
            return await _context.LuBrandNames.FirstOrDefaultAsync(c => c.Id == model.Id);
        }

        public async Task Upsert(LuBrandName model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(LuBrandName model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
