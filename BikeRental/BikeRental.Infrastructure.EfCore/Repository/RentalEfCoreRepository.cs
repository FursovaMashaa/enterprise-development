using BikeRental.Domain;
using BikeRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BikeRental.Infrastructure.EfCore.Repository;

public class RentalEfCoreRepository : IRepository<Rental, int>
{
    private readonly BikeRentalDbContext _context;
    private readonly DbSet<Rental> _rentals;

    public RentalEfCoreRepository(BikeRentalDbContext context)
    {
        _context = context;
        _rentals = context.Rentals!;
    }

    public async Task<Rental?> Read(int id)
    {
        return await _rentals
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IList<Rental>> ReadAll()
    {
        return await _rentals.ToListAsync();
    }

    public async Task<Rental> Create(Rental entity)
    {
        var result = await _rentals.AddAsync(entity);
        await _context.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<Rental> Update(Rental entity)
    {
        _rentals.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Delete(int id)
    {
        var entity = await Read(id);
        if (entity == null)
            return false;

        _rentals.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}