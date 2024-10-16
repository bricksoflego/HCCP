using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services;
public class CableTypeService(HccContext context) : ICableTypeService
{
    private readonly HccContext _context = context;

    /// <summary>
    /// Retrieves a list of cable types from the database, ordered by their name.
    /// </summary>
    /// <returns>A task that returns a list of LuCableType objects sorted by name.</returns>
    public virtual async Task<List<LuCableType>> GetCableTypes()
    {
        return await _context.LuCableTypes.OrderBy(c => c.Name).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific cable type from the database based on the provided model's ID.
    /// </summary>
    /// <param name="model">The LuCableType model containing the ID to search for.</param>
    /// <returns>A task that returns the matching LuCableType object, or null if not found.</returns>
    public virtual async Task<LuCableType?> GetCableType(LuCableType model)
    {
        return await _context.LuCableTypes.FirstOrDefaultAsync(c => c.Id == model.Id);
    }

    /// <summary>
    /// Updates an existing cable type model in the database, or adds it if it doesn't exist. 
    /// Saves the changes asynchronously.
    /// </summary>
    /// <param name="model">The LuCableType model to upsert.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Upsert(LuCableType model)
    {
        _context.Update(model);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the specified cable type model from the database and saves the changes asynchronously.
    /// </summary>
    /// <param name="model">The LuCableType model to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Delete(LuCableType model)
    {
        _context.Remove(model);
        await _context.SaveChangesAsync();
    }
}