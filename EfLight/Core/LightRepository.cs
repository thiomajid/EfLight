using EfLight.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EfLight.Core;

public abstract class LightRepository<TContext> :
    ILightRepository
    where TContext : DbContext
{
    protected readonly TContext _context;

    public LightRepository(TContext context)
    {
        _context = context;
    }
}