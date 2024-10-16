using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services;
public class PeripheralService(HccContext context) : IPeripheralService
{
    private readonly HccContext _context = context;

    /// <summary>
    /// Retrieves a list of peripheral devices from the database, ordered by their name.
    /// </summary>
    /// <returns>A task that returns a list of Peripheral objects sorted by name.</returns>
    public async Task<List<Peripheral>> GetPeripherals()
    {
        return await _context.Peripherals.OrderBy(c => c.Name).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific peripheral from the database based on the provided model's PCID.
    /// </summary>
    /// <param name="model">The peripheral model containing the PCID to search for.</param>
    /// <returns>A task that returns the matching Peripheral object, or null if not found.</returns>
    public async Task<Peripheral?> GetPeripheral(Peripheral model)
    {
        return await _context.Peripherals.FirstOrDefaultAsync(c => c.Pcid == model.Pcid);
    }

    /// <summary>
    /// Updates an existing peripheral model in the database, or adds it if it doesn't exist. 
    /// Saves the changes asynchronously.
    /// </summary>
    /// <param name="model">The peripheral model to upsert.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Upsert(Peripheral model)
    {
        _context.Update(model);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the specified peripheral model from the database and saves the changes asynchronously.
    /// </summary>
    /// <param name="model">The peripheral model to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Delete(Peripheral model)
    {
        _context.Remove(model);
        await _context.SaveChangesAsync();
    }
}