using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

/// <summary>
/// Entity Framework Core repository implementation for rental entities.
/// Provides data access operations for rental entities using EF Core with MongoDB provider.
/// </summary>
public class RentalEfCoreRepository(BikeRentalDbContext context) : IRepository<Rental, int>
{
    private readonly DbSet<Rental> _rentals = context.Rentals!;

    /// <summary>
    /// Retrieves a rental entity by its identifier
    /// </summary>
    /// <param name="id">Rental identifier</param>
    /// <returns>The rental entity or null if not found</returns>
    public async Task<Rental?> Read(int id)
    {
        return await _rentals
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <summary>
    /// Retrieves all rental entities
    /// </summary>
    /// <returns>List of all rental entities</returns>
    public async Task<IList<Rental>> ReadAll()
    {
        return await _rentals.ToListAsync();
    }

    /// <summary>
    /// Creates a new rental entity
    /// </summary>
    /// <param name="entity">Rental entity to create</param>
    /// <returns>The created rental entity</returns>
    public async Task<Rental> Create(Rental entity)
    {
        var result = await _rentals.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Updates an existing rental entity
    /// </summary>
    /// <param name="entity">Rental entity with updated data</param>
    /// <returns>The updated rental entity</returns>
    public async Task<Rental> Update(Rental entity)
    {
        _rentals.Update(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes a rental entity by its identifier
    /// </summary>
    /// <param name="id">Rental identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null)
            return false;

        _rentals.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }
}