using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class CableTypeService(HccContext context) : ICableTypeService
    {
        private readonly HccContext _context = context;

        public async Task<List<LuCableType>> GetCableTypes()
        {
            return await _context.LuCableTypes.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<LuCableType?> GetCableType(LuCableType model)
        {
            return await _context.LuCableTypes.FirstOrDefaultAsync(c => c.Id == model.Id);
        }

        public async Task Upsert(LuCableType model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(LuCableType model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
