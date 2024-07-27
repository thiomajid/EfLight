using EfLight.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EfLight.Core;

public abstract class LightRepository(DbContext context) : ILightRepository
{
    protected readonly DbContext _context = context;
}