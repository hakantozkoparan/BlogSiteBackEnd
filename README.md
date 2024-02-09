# BlogSite
Bu repomda blog/haber sitesinin backend tarafı olacaktır.
## Kullandığım Paketler
```powershell
Microsoft.AspNetCore.Identity.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.EntityFrameworkCore

```

## Adımlar
### Connection String Ayarlanması
```json
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=172.18.112.1;Initial Catalog=MiddleEarth;Application Name=MiddleEarth;User ID=sa;Password=şifreyiyaz; MultipleActiveResultSets=True;TrustServerCertificate=true;"
  }
```
------------------
Localde şifresiz bağlantı ayarı
```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=NewNews;Trusted_Connection=True;TrustServerCertificate=True;"
  }
```
### Entity Nesne Oluşturma (Models)
Models klasörü açılır. User için bir sınıf açılır sebebi IdentityUser içerisinde Name, Surname gibi alanlar bulunmamasıdır. IdentityUser içinde bulunmayan alanları da kullanmak istediğin zaman bir sınıf açarak buraya tanımlamak gereklidir.
```csharp
// User.cs
public class User : IdentityUser<Guid>
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<Blog> Blogs { get; set; } = new List<Blog>(); //Bir kişinin birden fazla yazısı olabilir.
}
```
```csharp
// Blog.cs
public class Blog
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool isPublish {  get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}
```
```csharp
// Category.cs
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Blog> blogs { get; set; } = new List<Blog>();
}
```
### Context oluşturulması; NewsContext.cs
```csharp
    public class NewsContext : IdentityDbContext<User,IdentityRole<Guid>, Guid>
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
        public DbSet <Blog> Blogs { get; set; }

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
                .IsRequired()
                .HasMaxLength(30);
            modelBuilder.Entity<User>()
                .Property(x => x.Surname)
                .IsRequired()
                .HasMaxLength(50);
        }
    }
```
### Identity ve veri tabanı ayarlamaları

Program.cs sınıfı içerisinde aşağıdaki kod bloğu eklenir.

```csharp
builder.Services.AddDbContext<NewsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<NewsContext>();
```
### Migration Oluşturma
VSCode || VisualStudio
```bash
dotnet ef migrations add InitDatabase || add-migration InitDatabase
```

```bash
dotnet ef migrations remove || remove-migration
```
```bash
dotnet ef database update || update-database 
```
