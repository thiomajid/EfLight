using Microsoft.EntityFrameworkCore;

namespace WebApplication1;

public class MyContext : DbContext
{
    public MyContext(DbContextOptions<MyContext> options) : base(options)
    {
    }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseInMemoryDatabase(databaseName: "MyContext");
    //}
}
