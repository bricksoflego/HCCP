using BlueDragon.Models;

namespace BlueDragon.Services;
public interface IEComponentService
{
    Task<List<Ecomponent>> GetComponents();
    Task<Ecomponent?> GetComponent(Ecomponent model);
    Task Upsert(Ecomponent model);
    Task Delete(Ecomponent model);
}