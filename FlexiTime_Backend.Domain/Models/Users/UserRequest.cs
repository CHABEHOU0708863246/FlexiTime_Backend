using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Users
{
    public class UserRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public int WorkingHours { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit comporter au moins {2} caractères.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public bool IsPartTime { get; set; } = false;
        public DateTime HireDate { get; set; }
    }
}
