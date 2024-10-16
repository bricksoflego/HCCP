using BlueDragon.Data;
using BlueDragon.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueDragon.Services;
public class BrandNameService(HccContext context) : IBrandNameService
{
    private readonly HccContext _context = context;

    /// <summary>
    /// Retrieves a list of brand names from the database, ordered by their name.
    /// </summary>
    /// <returns>A task that returns a list of LuBrandName objects sorted by name.</returns>
    public virtual async Task<List<LuBrandName>> GetBrandNames()
    {
        return await _context.LuBrandNames.OrderBy(c => c.Name).ToListAsync();
    }

    /// <summary>
    /// Retrieves a brand name from the database based on the provided model's ID.
    /// </summary>
    /// <param name="model">The model containing the ID of the brand name to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, containing the found <see cref="LuBrandName"/> or null if not found.</returns>
    public virtual async Task<LuBrandName?> GetBrandName(LuBrandName model)
    {
        return await _context.LuBrandNames.FirstOrDefaultAsync(c => c.Id == model.Id);
    }

    /// <summary>
    /// Updates or inserts the specified brand name model in the database.
    /// </summary>
    /// <param name="model">The brand name model to be updated or inserted.</param
    public async Task Upsert(LuBrandName model)
    {
        _context.Update(model);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the specified brand name model from the database.
    /// </summary>
    /// <param name="model">The brand name model to be deleted.</param>
    /// <returns>A task representing the asynchronous operation
    public async Task Delete(LuBrandName model)
    {
        _context.Remove(model);
        await _context.SaveChangesAsync();
    }
}