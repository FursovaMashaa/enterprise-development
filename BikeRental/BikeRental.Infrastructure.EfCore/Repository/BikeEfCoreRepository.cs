using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

public class BikeEfCoreRepository : IRepository<Bike, int>
{
    private readonly BikeRentalDbContext _context;
    private readonly DbSet<Bike> _bikes;

    public BikeEfCoreRepository(BikeRentalDbContext context)
    {
        _context = context;
        _bikes = context.Bikes!;
    }

    public async Task<Bike?> Read(int id)
    {
    return await _bikes.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IList<Bike>> ReadAll()
    {
        return await _bikes.ToListAsync();
    }

    public async Task<Bike> Create(Bike entity)
    {
        var result = await _bikes.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Bike> Update(Bike entity)
    {
        _bikes.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null)
            return false;

        _bikes.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}