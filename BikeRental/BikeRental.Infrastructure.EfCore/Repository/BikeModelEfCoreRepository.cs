using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

/// <summary>
/// Entity Framework Core repository implementation for bike model entities.
/// Provides data access operations for bike model entities using EF Core with MongoDB provider.
/// </summary>
public class BikeModelEfCoreRepository : IRepository<BikeModel, int>
{
    private readonly BikeRentalDbContext _context;
    private readonly DbSet<BikeModel> _models;

    /// <summary>
    /// Initializes a new instance of the <see cref="BikeModelEfCoreRepository"/> class
    /// </summary>
    /// <param name="context">Database context for bike rental system</param>
    public BikeModelEfCoreRepository(BikeRentalDbContext context)
    {
        _context = context;
        _models = context.BikeModels!;
    }

    /// <summary>
    /// Retrieves a bike model entity by its identifier
    /// </summary>
    /// <param name="id">Bike model identifier</param>
    /// <returns>The bike model entity or null if not found</returns>
    public async Task<BikeModel?> Read(int id)
    {
        return await _models.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all bike model entities
    /// </summary>
    /// <returns>List of all bike model entities</returns>
    public async Task<IList<BikeModel>> ReadAll()
    {
        return await _models.ToListAsync();
    }

    /// <summary>
    /// Creates a new bike model entity
    /// </summary>
    /// <param name="entity">Bike model entity to create</param>
    /// <returns>The created bike model entity</returns>
    public async Task<BikeModel> Create(BikeModel entity)
    {
        var result = await _models.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    /// <summary>
    /// Updates an existing bike model entity
    /// </summary>
    /// <param name="entity">Bike model entity with updated data</param>
    /// <returns>The updated bike model entity</returns>
    public async Task<BikeModel> Update(BikeModel entity)
    {
        _models.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    /// <summary>
    /// Deletes a bike model entity by its identifier
    /// </summary>
    /// <param name="id">Bike model identifier</param>
    /// <returns>True if deletion was successful, false otherwise</returns>
    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null)
            return false;

        _models.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}