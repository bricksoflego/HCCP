using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services;
public class EComponentService(HccContext context) : IEComponentService
{
    private readonly HccContext _context = context;

    /// <summary>
    /// Retrieves a list of electronic components from the database, ordered by their name.
    /// </summary>
    /// <returns>A task that returns a list of Ecomponent objects sorted by name.</returns>
    public async Task<List<Ecomponent>> GetComponents()
    {
        return await _context.Ecomponents.OrderBy(c => c.Name).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific electronic component from the database based on the provided model's ECID.
    /// </summary>
    /// <param name="model">The Ecomponent model containing the ECID to search for.</param>
    /// <returns>A task that returns the matching Ecomponent object, or null if not found.</returns>
    public async Task<Ecomponent?> GetComponent(Ecomponent model)
    {
        return await _context.Ecomponents.FirstOrDefaultAsync(c => c.Ecid == model.Ecid);
    }

    /// <summary>
    /// Updates an existing electronic component model in the database, or adds it if it doesn't exist. 
    /// Saves the changes asynchronously.
    /// </summary>
    /// <param name="model">The Ecomponent model to upsert.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Upsert(Ecomponent model)
    {
        _context.Update(model);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the specified electronic component model from the database and saves the changes asynchronously.
    /// </summary>
    /// <param name="model">The Ecomponent model to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Delete(Ecomponent model)
    {
        _context.Remove(model);
        await _context.SaveChangesAsync();
    }
}