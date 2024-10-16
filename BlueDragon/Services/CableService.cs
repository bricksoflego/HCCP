using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services;
public class CableService(HccContext context) : ICableService
{
    private readonly HccContext _context = context;

    /// <summary>
    /// Retrieves a list of cables from the database, ordered by their cable type.
    /// </summary>
    /// <returns>A task that returns a list of Cable objects sorted by cable type.</returns>
    public async Task<List<Cable>> GetCables()
    {
        return await _context.Cables.OrderBy(c => c.CableType).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific cable from the database based on the provided model's CID.
    /// </summary>
    /// <param name="model">The Cable model containing the CID to search for.</param>
    /// <returns>A task that returns the matching Cable object, or null if not found.</returns>
    public async Task<Cable?> GetCable(Cable model)
    {
        return await _context.Cables.FirstOrDefaultAsync(c => c.Cid == model.Cid);
    }

    /// <summary>
    /// Updates an existing cable model in the database, or adds it if it doesn't exist. 
    /// Saves the changes asynchronously.
    /// </summary>
    /// <param name="model">The Cable model to upsert.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Upsert(Cable model)
    {
        _context.Update(model);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the specified cable model from the database and saves the changes asynchronously.
    /// </summary>
    /// <param name="model">The Cable model to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Delete(Cable model)
    {
        _context.Remove(model);
        await _context.SaveChangesAsync();
    }
}