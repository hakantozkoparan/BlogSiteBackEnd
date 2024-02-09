using Microsoft.AspNetCore.Identity;

namespace BlogSite.Models
{
    public class User : IdentityUser<Guid>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public List<Blog> Blogs { get; set; } = new List<Blog>(); //Bir kişinin birden fazla yazısı olabilir.
    }
}
