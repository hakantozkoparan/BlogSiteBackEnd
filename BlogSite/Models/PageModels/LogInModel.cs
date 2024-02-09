using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.PageModels
{
    public class LogInModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public bool RememberMe { get; set; }

    }
}
