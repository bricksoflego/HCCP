using BlueDragon.Models;

namespace BlueDragon.Services;
public interface IBrandNameService
{
    Task<List<LuBrandName>> GetBrandNames();
    Task<LuBrandName?> GetBrandName(LuBrandName model);
    Task Upsert(LuBrandName model);
    Task Delete(LuBrandName model);
}