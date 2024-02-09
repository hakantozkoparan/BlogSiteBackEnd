namespace BlogSite.Models.PageModels
{
    public class UpdateNewsModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool isPublish { get; set; }
        public int CategoryId { get; set; }
    }
}
