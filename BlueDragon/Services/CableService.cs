using BlueDragon.Data;
using BlueDragon.Models;

namespace BlueDragon.Services
{
    public class CableService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Cable>> GetCables()
        {
            List<Cable> cables = new();
            using (var context = new HccContext())
            {
                cables = context.Cables.OrderBy(c => c.CableType).ToList();
                await Task.CompletedTask;
            }
            return cables;
        }

        public Task<Cable?> GetCable(Cable model)
        {
            using (var context = new HccContext())
            {
                return Task.FromResult(context.Cables?.FirstOrDefault(c => c.Cid == model.Cid));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(Cable model)
        {
            using (var context = new HccContext())
            {
                context.Update(model);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Delete(Cable model)
        {
            using (var context = new HccContext())
            {
                context.Remove(model);
                await context.SaveChangesAsync();
            }
        }
    }
}
