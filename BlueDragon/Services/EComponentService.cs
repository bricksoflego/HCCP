using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services
{
    public class EComponentService
    {
        private readonly HccContext _context;

        public EComponentService(HccContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Ecomponent>> GetComponents()
        {
            return await _context.Ecomponents.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Ecomponent?> GetComponent(Ecomponent model)
        {
            return await _context.Ecomponents.FirstOrDefaultAsync(c => c.Ecid == model.Ecid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(Ecomponent model)
        {
            _context.Update(model);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Delete(Ecomponent model)
        {
            _context.Remove(model);
            await _context.SaveChangesAsync();
        }
    }
}
