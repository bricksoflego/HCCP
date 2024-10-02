using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class CableService(HccContext context) : ICableService
    {
        private readonly HccContext _context = context;

        public async Task<List<Cable>> GetCables()
        {
            return await _context.Cables.OrderBy(c => c.CableType).ToListAsync();
        }

        public async Task<Cable?> GetCable(Cable model)
        {
            return await _context.Cables.FirstOrDefaultAsync(c => c.Cid == model.Cid);
        }

        public async Task Upsert(Cable model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Cable model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
