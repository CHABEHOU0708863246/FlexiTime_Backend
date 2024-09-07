using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Password
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Code { get; set; }

        // Constructeur par défaut
        public ForgotPassword() { }

        // Constructeur avec deux arguments
        public ForgotPassword(string email, string code)
        {
            Email = email;
            Code = code;
        }
    }
}
