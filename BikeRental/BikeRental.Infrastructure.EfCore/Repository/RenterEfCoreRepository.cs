using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

public class RenterEfCoreRepository(BikeRentalDbContext context) : IRepository<Renter, int>
{
    private readonly DbSet<Renter> _renters = context.Renters!;

    public async Task<Renter> Create(Renter entity)
    {
        var result = await _renters.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await _renters.FindAsync(id);
        if (entity == null)
            return false;

        _renters.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Renter?> Read(int id) =>
        await _renters.FindAsync(id);

    public async Task<IList<Renter>> ReadAll() =>
        await _renters.ToListAsync();

    public async Task<Renter> Update(Renter entity)
    {
        _renters.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }

    Task<Renter> IRepository<Renter, int>.Create(Renter entity)
    {
        throw new System.NotImplementedException();
    }

    Task<bool> IRepository<Renter, int>.Delete(int id)
    {
        throw new System.NotImplementedException();
    }

    Task<Renter?> IRepository<Renter, int>.Read(int id)
    {
        throw new System.NotImplementedException();
    }

    Task<IList<Renter>> IRepository<Renter, int>.ReadAll()
    {
        throw new System.NotImplementedException();
    }

    Task<Renter> IRepository<Renter, int>.Update(Renter entity)
    {
        throw new System.NotImplementedException();
    }
}