using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

/// <summary>
/// Entity Framework Core repository implementation for bike entities.
/// Provides data access operations for bike entities using EF Core with MongoDB provider.
/// </summary>
public class BikeEfCoreRepository(BikeRentalDbContext context) : IRepository<Bike, int>
{
    private readonly DbSet<Bike> _bikes = context.Bikes;

    /// <summary>
    /// Retrieves a bike entity by its identifier
    /// </summary>
    /// <param name="id">Bike identifier</param>
    /// <returns>The bike entity or null if not found</returns>
    public async Task<Bike?> Read(int id)
    {
        return await _bikes.FirstOrDefaultAsync(e => e.Id == id);
    }

    /// <summary>
    /// Retrieves all bike entities
    /// </summary>
    /// <returns>List of all bike entities</returns>
    public async Task<IList<Bike>> ReadAll()
    {
        return await _bikes.ToListAsync();
    }

    /// <summary>
    /// Creates a new bike entity
    /// </summary>
    /// <param name="entity">Bike entity to create</param>
    /// <returns>The created bike entity</returns>
    public async Task<Bike> Create(Bike entity)
    {
        var result = await _bikes.AddAsync(entity);
        await context.SaveChangesAsync();  // Используем context, а не _context
        return result.Entity;
    }

    /// <summary>
    /// Updates an existing bike entity
    /// </summary>
    /// <param name="entity">Bike entity with updated data</param>
    /// <returns>The updated bike entity</returns>
    public async Task<Bike> Update(Bike entity)
    {
        _bikes.Update(entity);
        await context.SaveChangesAsync();  // Используем context, а не _context
        return entity;
    }

    /// <summary>
    /// Deletes a bike entity by its identifier
    /// </summary>
    /// <param name="id">Bike identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null)
            return false;

        _bikes.Remove(entity);
        await context.SaveChangesAsync();  // Используем context, а не _context
        return true;
    }
}