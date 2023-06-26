using BlueDragon.Data;
using BlueDragon.Models;

namespace BlueDragon.Services
{
    public class PeripheralService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Peripheral>> GetPeripherals()
        {
            List<Peripheral> peripherals = new();
            using (var context = new HccContext())
            {
                peripherals = context.Peripherals.OrderBy(c => c.Name).ToList();
                await Task.CompletedTask;
            }
            return peripherals;
        }


        public Task<Peripheral?> GetPeripheral(Peripheral model)
        {
            using (var context = new HccContext())
            {
                return Task.FromResult(context.Peripherals?.FirstOrDefault(c => c.Pcid == model.Pcid)); 
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(Peripheral model)
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
        public async Task Delete(Peripheral model)
        {
            using (var context = new HccContext())
            {
                context.Remove(model);
                await context.SaveChangesAsync();
            }
        }

    }
}
