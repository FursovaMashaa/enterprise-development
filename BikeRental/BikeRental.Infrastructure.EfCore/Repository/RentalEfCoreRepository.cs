using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

public class RentalEfCoreRepository(BikeRentalDbContext context) : IRepository<Rental, int>
{
    private readonly DbSet<Rental> _rentals = context.Rentals!;

    public async Task<Rental> Create(Rental entity)
    {
        var result = await _rentals.AddAsync(entity);
        await context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await _rentals
            .Include(r => r.Bike)
            .Include(r => r.Renter)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (entity == null)
            return false;

        _rentals.Remove(entity);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<Rental?> Read(int id) =>
        await _rentals
            .Include(r => r.Bike)
            .Include(r => r.Renter)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IList<Rental>> ReadAll() =>
        await _rentals
            .Include(r => r.Bike)
            .Include(r => r.Renter)
            .ToListAsync();

    public async Task<Rental> Update(Rental entity)
    {
        _rentals.Update(entity);
        await context.SaveChangesAsync();
        return (await Read(entity.Id))!;
    }

    Task<Rental> IRepository<Rental, int>.Create(Rental entity)
    {
        throw new System.NotImplementedException();
    }

    Task<bool> IRepository<Rental, int>.Delete(int id)
    {
        throw new System.NotImplementedException();
    }

    Task<Rental?> IRepository<Rental, int>.Read(int id)
    {
        throw new System.NotImplementedException();
    }

    Task<IList<Rental>> IRepository<Rental, int>.ReadAll()
    {
        throw new System.NotImplementedException();
    }

    Task<Rental> IRepository<Rental, int>.Update(Rental entity)
    {
        throw new System.NotImplementedException();
    }
}