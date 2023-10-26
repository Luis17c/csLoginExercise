using Microsoft.EntityFrameworkCore;
using Models;

public class DbConnection : DbContext {
    public DbSet<User> Users { get; set; }

    public override int SaveChanges() {
        AddTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new ()) {
        AddTimestamps();
        return await base.SaveChangesAsync();
    }

    private void AddTimestamps() {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entity in entities)
            {  
                DateTime now = DateTime.UtcNow;

                if (entity.State == EntityState.Added) {
                    ((BaseEntity)entity.Entity).createdAt = now;
                }
                ((BaseEntity)entity.Entity).updatedAt = now;
            }
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            "Server=localhost:5432;" +
            "Port=5432;" +
            "Database=postgres;" +
            "User Id=postgres;" +
            "Password=admin"
        );
}