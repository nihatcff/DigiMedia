using DigiMedia.Models.Common;

namespace DigiMedia.Models
{
    public class Project:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string ImagePath { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
    }
}
