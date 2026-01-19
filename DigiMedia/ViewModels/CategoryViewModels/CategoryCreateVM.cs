using System.ComponentModel.DataAnnotations;

namespace DigiMedia.ViewModels.CategoryViewModels
{
    public class CategoryCreateVM
    {
        [Required,MaxLength(256),MinLength(2)]
        public string Name { get; set; } = string.Empty;
    }
}
