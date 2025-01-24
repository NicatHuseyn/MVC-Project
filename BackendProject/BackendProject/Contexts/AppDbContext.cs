using BackendProject.Models.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BackendProject.Contexts;

public class AppDbContext:IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }

    public DbSet<Slider> Sliders { get; set; } = null!;
    public DbSet<Service> Services { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<MenuCategory> MenuCategories { get; set; } = null!;
    public DbSet<Menu> Menus { get; set; } = null!;
    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<Basket> Baskets { get; set; }
    public DbSet<BasketProduct> BasketProducts { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<SocialMedia> SocialMedias { get; set; }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }
}
