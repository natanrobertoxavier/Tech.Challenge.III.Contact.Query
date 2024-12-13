using Microsoft.EntityFrameworkCore;

namespace Contact.Query.Infrastructure.RepositoryAccess;
public class TechChallengeContext(DbContextOptions<TechChallengeContext> options) : DbContext(options)
{
    public DbSet<Domain.Entities.Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TechChallengeContext).Assembly);
    }
}