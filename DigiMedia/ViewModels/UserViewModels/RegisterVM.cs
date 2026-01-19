using System.ComponentModel.DataAnnotations;

namespace DigiMedia.ViewModels.UserViewModels
{
    public class RegisterVM
    {
        [Required,MaxLength(512),MinLength(6)]
        public string Fullname { get; set; } = string.Empty;
        [Required,EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required,MaxLength(512),MinLength(6)]
        public string Username { get; set; } = string.Empty;
        [Required,MaxLength(256),MinLength(5),DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required,MaxLength(256),MinLength(5),DataType(DataType.Password),Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
}
