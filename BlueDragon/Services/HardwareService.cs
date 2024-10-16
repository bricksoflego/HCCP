using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services;
public class HardwareService(HccContext context) : IHardwareService
{
    private readonly HccContext _context = context;

    /// <summary>
    /// Retrieves a list of hardware devices from the database, ordered by their name.
    /// </summary>
    /// <returns>A task that returns a list of Hardware objects sorted by name.</returns>
    public async Task<List<Hardware>> GetHardware()
    {
        return await _context.Hardwares.OrderBy(c => c.Name).ToListAsync();
    }

    /// <summary>
    /// Retrieves a specific hardware device from the database based on the provided model's HID.
    /// </summary>
    /// <param name="model">The hardware model containing the HID to search for.</param>
    /// <returns>A task that returns the matching Hardware object, or null if not found.</returns>
    public async Task<Hardware?> GetSelectedHardware(Hardware model)
    {
        return await _context.Hardwares.FirstOrDefaultAsync(c => c.Hid == model.Hid);
    }

    /// <summary>
    /// Updates an existing hardware model in the database or adds it if it doesn't exist. 
    /// Saves the changes asynchronously.
    /// </summary>
    /// <param name="model">The hardware model to upsert.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Upsert(Hardware model)
    {
        _context.Update(model);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the specified hardware model from the database and saves the changes asynchronously.
    /// </summary>
    /// <param name="model">The hardware model to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Delete(Hardware model)
    {
        _context.Remove(model);
        await _context.SaveChangesAsync();
    }
}