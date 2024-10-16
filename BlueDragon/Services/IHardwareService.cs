using BlueDragon.Models;

namespace BlueDragon.Services;
public interface IHardwareService
{
    Task<List<Hardware>> GetHardware();
    Task<Hardware?> GetSelectedHardware(Hardware model);
    Task Upsert(Hardware model);
    Task Delete(Hardware model);
}