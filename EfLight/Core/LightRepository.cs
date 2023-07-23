using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EfLight.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace EfLight.Core;
public abstract class LightRepository : ILightRepository
{
    protected readonly DbContext _context;

    protected LightRepository(DbContext context)
    {
        _context = context;
    }
}
