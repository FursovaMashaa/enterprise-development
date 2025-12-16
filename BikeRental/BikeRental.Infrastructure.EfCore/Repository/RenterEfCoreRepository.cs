using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

public class RenterEfCoreRepository : IRepository<Renter, int>
{
    private readonly BikeRentalDbContext _context;
    private readonly DbSet<Renter> _renters;

    public RenterEfCoreRepository(BikeRentalDbContext context)
    {
        _context = context;
        _renters = context.Renters!;
    }

    public async Task<Renter?> Read(int id)
    {
        return await _renters.FindAsync(id);
    }

    public async Task<IList<Renter>> ReadAll()
    {
        return await _renters.ToListAsync();
    }

    public async Task<Renter> Create(Renter entity)
    {
        var result = await _renters.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Renter> Update(Renter entity)
    {
        _renters.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

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