using Microsoft.EntityFrameworkCore;
using SlabLabs.Api.Models;

namespace SlabLabs.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Item> Items => Set<Item>();
}
