using CMMS.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CMMS.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<SparePart> SpareParts { get; set; }
}