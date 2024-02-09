using System.ComponentModel.DataAnnotations;

namespace BlogSite.Models.PageModels
{
    public class CreateNewsModel
    {
        [Required]
        [MinLength(5)]
        public string Title { get; set; }
        [Required]
        [MinLength(10)]
        public string Description { get; set; }
        [Required]
        public bool isPublish { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public Guid UserId { get; set; }


    }
}
