using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class EComponentService(HccContext context) : IEComponentService
    {
        private readonly HccContext _context = context;

        public async Task<List<Ecomponent>> GetComponents()
        {
            return await _context.Ecomponents.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Ecomponent?> GetComponent(Ecomponent model)
        {
            return await _context.Ecomponents.FirstOrDefaultAsync(c => c.Ecid == model.Ecid);
        }

        public async Task Upsert(Ecomponent model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Ecomponent model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
