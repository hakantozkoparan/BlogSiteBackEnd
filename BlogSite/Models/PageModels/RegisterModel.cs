using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.PageModels
{
    public class RegisterModel
    {
        public string UserName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
