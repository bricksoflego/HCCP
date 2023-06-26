using BlueDragon.Data;
using BlueDragon.Models;

namespace BlueDragon.Services
{
    public class CableTypeService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<LuCableType>> GetCableTypes()
        {
            List<LuCableType> cables = new();
            using (var context = new HccContext())
            {
                cables = context.LuCableTypes.OrderBy(c => c.Name).ToList();
                await Task.CompletedTask;
            }
            return cables;
        }
        public Task<LuCableType?> GetCableType(LuCableType model)
        {
            using (var context = new HccContext())
            {
                return Task.FromResult(context.LuCableTypes?.FirstOrDefault(c => c.Id == model.Id));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(LuCableType model)
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
        public async Task Delete(LuCableType model)
        {
            using (var context = new HccContext())
            {
                context.Remove(model);
                await context.SaveChangesAsync();
            }
        }
    }
}
