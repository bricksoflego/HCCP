using BlueDragon.Data;
using BlueDragon.Models;

namespace BlueDragon.Services
{
    public class BrandNameService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<LuBrandName>> GetBrandNames()
        {
            List<LuBrandName> brands = new();
            using (var context = new HccContext())
            {
                brands = context.LuBrandNames.OrderBy(c => c.Name).ToList();
                await Task.CompletedTask;
            }
            return brands;
        }
        public Task<LuBrandName?> GetBrandName(LuBrandName model)
        {
            using (var context = new HccContext())
            {
                return Task.FromResult(context.LuBrandNames?.FirstOrDefault(c => c.Id == model.Id));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(LuBrandName model)
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
        public async Task Delete(LuBrandName model)
        {
            using (var context = new HccContext())
            {
                context.Remove(model);
                await context.SaveChangesAsync();
            }
        }
    }
}
