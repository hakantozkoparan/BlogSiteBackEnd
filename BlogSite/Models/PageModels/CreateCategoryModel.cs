using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.PageModels
{
    public class CreateCategoryModel
    {
        [Required]
        public string Name { get; set; }
    }
}
