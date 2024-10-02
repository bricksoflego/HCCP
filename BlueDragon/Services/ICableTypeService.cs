using BlueDragon.Models;

namespace BlueDragon.Services
{
    public interface ICableTypeService
    {
        Task<List<LuCableType>> GetCableTypes();
        Task<LuCableType?> GetCableType(LuCableType model);
        Task Upsert(LuCableType model);
        Task Delete(LuCableType model);
    }
}
