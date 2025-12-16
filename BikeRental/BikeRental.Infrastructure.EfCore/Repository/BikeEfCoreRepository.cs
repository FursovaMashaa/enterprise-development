using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

public class BikeEfCoreRepository(BikeRentalDbContext context) : IRepository<Bike, int>
{
    private readonly DbSet<Bike> _bikes = context.Bikes!;

    public async Task<Bike?> Create(Bike entity)
    {
        var result = await _bikes.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await _bikes
            .Include(b => b.Model)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
            return false;

        _bikes.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Bike?> Read(int id) =>
        await _bikes
            .Include(b => b.Model)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IList<Bike>> ReadAll() =>
        await _bikes
            .Include(b => b.Model)
            .ToListAsync();

    public async Task<Bike> Update(Bike entity)
    {
        _bikes.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }

    Task<Bike> IRepository<Bike, int>.Create(Bike entity)
    {
        throw new System.NotImplementedException();
    }

    Task<bool> IRepository<Bike, int>.Delete(int id)
    {
        throw new System.NotImplementedException();
    }

    Task<Bike?> IRepository<Bike, int>.Read(int id)
    {
        throw new System.NotImplementedException();
    }

    Task<IList<Bike>> IRepository<Bike, int>.ReadAll()
    {
        throw new System.NotImplementedException();
    }

    Task<Bike> IRepository<Bike, int>.Update(Bike entity)
    {
        throw new System.NotImplementedException();
    }
}