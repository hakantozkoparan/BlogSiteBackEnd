using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BlogSite.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogSite.Context
{
    public class NewsContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    /* Buranın temeli şuna bağlanıyor;
    User sınıfını IdentityUser<Guid> kalıtımını alırsa IdentityDbContext mecburi şekilde <User, IdentityRole<Guid>, Guid>
    olarak kalıtım alması gerekiyor çünkü <> içinin belirli bir sırası var ve ilk üç böyle yazılıyor eğer ki userclaims,
    roleclaims vs de kullanılacaksa bunları <> yazdığında içindeki sıraya bakarak yazman gerekiyor.
    Ek olarak yazılan Guid "Id" parametresini belirtir. Id'nin Guid değer olacağını bildiriyor. */
    {
        public NewsContext(DbContextOptions<NewsContext> options)
        : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Identity tablolarının adlarını özelleştirme
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            modelBuilder.Entity<User>()
                .Property(x => x.Name)
                .HasMaxLength(30);
            modelBuilder.Entity<User>()
                .Property(x => x.Surname)
                .HasMaxLength(50);
        }
    }
}
