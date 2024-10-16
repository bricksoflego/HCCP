using BlueDragon.Models;

namespace BlueDragon.Services;
public interface ICableService
{
    Task<List<Cable>> GetCables();
    Task<Cable?> GetCable(Cable model);
    Task Upsert(Cable model);
    Task Delete(Cable model);
}