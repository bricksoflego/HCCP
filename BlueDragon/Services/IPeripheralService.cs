using BlueDragon.Models;

namespace BlueDragon.Services
{
    public interface IPeripheralService
    {
        Task<List<Peripheral>> GetPeripherals();
        Task<Peripheral?> GetPeripheral(Peripheral model);
        Task Upsert(Peripheral model);
        Task Delete(Peripheral model);
    }
}
