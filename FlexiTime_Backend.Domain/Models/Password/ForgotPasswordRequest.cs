using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Password
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string RedirectPath { get; set; }
    }
}
