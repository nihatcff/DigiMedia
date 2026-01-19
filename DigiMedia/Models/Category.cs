using DigiMedia.Models.Common;

namespace DigiMedia.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Project> Projects { get; set; } = [];
    }
}
