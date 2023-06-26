using BlueDragon.Data;
using BlueDragon.Models;

namespace BlueDragon.Services
{
    public class HardwareService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<List<Hardware>> GetHardware()
        {
            List<Hardware> hardware = new();
            using (var context = new HccContext())
            {
                hardware = context.Hardwares.OrderBy(c => c.Name).ToList();
                await Task.CompletedTask;
            }
            return hardware;
        }

        public Task<Hardware?> GetSelectedHardware(Hardware model)
        {
            using (var context = new HccContext())
            {
                return Task.FromResult(context.Hardwares?.FirstOrDefault(c => c.Hid == model.Hid));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task Upsert(Hardware model)
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
        public async Task Delete(Hardware model)
        {
            using (var context = new HccContext())
            {
                context.Remove(model);
                await context.SaveChangesAsync();
            }
        }
    }
}
