using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

public class BikeModelEfCoreRepository : IRepository<BikeModel, int>
{
    private readonly BikeRentalDbContext _context;
    private readonly DbSet<BikeModel> _models;

    public BikeModelEfCoreRepository(BikeRentalDbContext context)
    {
        _context = context;
        _models = context.BikeModels!;
    }

    public async Task<BikeModel?> Read(int id)
    {
        return await _models.FindAsync(id);
    }

    public async Task<IList<BikeModel>> ReadAll()
    {
        return await _models.ToListAsync();
    }

    public async Task<BikeModel> Create(BikeModel entity)
    {
        var result = await _models.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<BikeModel> Update(BikeModel entity)
    {
        _models.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

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