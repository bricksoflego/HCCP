using BlueDragon.Data;
using BlueDragon.Models;

namespace BlueDragon.Services
{
    public class EComponentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Ecomponent>> GetComponents()
        {
            List<Ecomponent> ecomponents = new();
            using (var context = new HccContext())
            {
                ecomponents = context.Ecomponents.OrderBy(c => c.Name).ToList();
                await Task.CompletedTask;
            }
            return ecomponents;
        }


        public Task<Ecomponent?> GetComponent(Ecomponent model)
        {
            using (var context = new HccContext())
            {
                return Task.FromResult(context.Ecomponents?.FirstOrDefault(c => c.Ecid == model.Ecid)); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(Ecomponent model)
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
        public async Task Delete(Ecomponent model)
        {
            using (var context = new HccContext())
            {
                context.Remove(model);
                await context.SaveChangesAsync();
            }
        }

    }
}
