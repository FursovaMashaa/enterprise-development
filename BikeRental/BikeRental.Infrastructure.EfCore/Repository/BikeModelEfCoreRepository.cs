using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

public class BikeModelEfCoreRepository(BikeRentalDbContext context) : IRepository<BikeModel, int>
{
    private readonly DbSet<BikeModel> _models = context.BikeModels!;

    public async Task<BikeModel> Create(BikeModel entity)
    {
        var result = await _models.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await _models.FindAsync(id);
        if (entity == null)
            return false;

        _models.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<BikeModel?> Read(int id) =>
        await _models.FindAsync(id);

    public async Task<IList<BikeModel>> ReadAll() =>
        await _models.ToListAsync();

    public async Task<BikeModel> Update(BikeModel entity)
    {
        _models.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }

    Task<BikeModel> IRepository<BikeModel, int>.Create(BikeModel entity)
    {
        throw new System.NotImplementedException();
    }

    Task<bool> IRepository<BikeModel, int>.Delete(int id)
    {
        throw new System.NotImplementedException();
    }

    Task<BikeModel?> IRepository<BikeModel, int>.Read(int id)
    {
        throw new System.NotImplementedException();
    }

    Task<IList<BikeModel>> IRepository<BikeModel, int>.ReadAll()
    {
        throw new System.NotImplementedException();
    }

    Task<BikeModel> IRepository<BikeModel, int>.Update(BikeModel entity)
    {
        throw new System.NotImplementedException();
    }
}