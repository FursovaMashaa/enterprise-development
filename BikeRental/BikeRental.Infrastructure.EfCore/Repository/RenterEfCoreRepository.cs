using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

/// <summary>
/// Entity Framework Core repository implementation for renter entities.
/// Provides data access operations for renter entities using EF Core with MongoDB provider.
/// </summary>
public class RenterEfCoreRepository : IRepository<Renter, int>
{
    private readonly BikeRentalDbContext _context;
    private readonly DbSet<Renter> _renters;

    /// <summary>
    /// Initializes a new instance of the <see cref="RenterEfCoreRepository"/> class
    /// </summary>
    /// <param name="context">Database context for bike rental system</param>
    public RenterEfCoreRepository(BikeRentalDbContext context)
    {
        _context = context;
        _renters = context.Renters!;
    }

    /// <summary>
    /// Retrieves a renter entity by its identifier
    /// </summary>
    /// <param name="id">Renter identifier</param>
    /// <returns>The renter entity or null if not found</returns>
    public async Task<Renter?> Read(int id)
    {
        return await _renters.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all renter entities
    /// </summary>
    /// <returns>List of all renter entities</returns>
    public async Task<IList<Renter>> ReadAll()
    {
        return await _renters.ToListAsync();
    }

    /// <summary>
    /// Creates a new renter entity
    /// </summary>
    /// <param name="entity">Renter entity to create</param>
    /// <returns>The created renter entity</returns>
    public async Task<Renter> Create(Renter entity)
    {
        var result = await _renters.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Updates an existing renter entity
    /// </summary>
    /// <param name="entity">Renter entity with updated data</param>
    /// <returns>The updated renter entity</returns>
    public async Task<Renter> Update(Renter entity)
    {
        _renters.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes a renter entity by its identifier
    /// </summary>
    /// <param name="id">Renter identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null)
            return false;

        _renters.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}