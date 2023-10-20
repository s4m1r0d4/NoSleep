using System.Reflection;
using Microsoft.EntityFrameworkCore;
using LasPollasHermanas.Shared.Models;

namespace LasPollasHermanas.Server.Models;

public class DildoStoreContext : DbContext
{
    public DbSet<Dildo>      Dildos      => Set<Dildo>();
    public DbSet<Account>    Accounts    => Set<Account>();
    public DbSet<MortalUser> MortalUsers => Set<MortalUser>();
    public DbSet<AdminUser>  AdminUsers  => Set<AdminUser>();

    public DildoStoreContext(DbContextOptions<DildoStoreContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
