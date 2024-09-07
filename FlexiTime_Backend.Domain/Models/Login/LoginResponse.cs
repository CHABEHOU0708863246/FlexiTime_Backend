using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Login
{
    public class LoginResponse
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        public bool Status { get; set; }
        public string ErrorMessage { get; set; }

        // Constructeur par défaut
        public LoginResponse() { }

        // Constructeur avec trois arguments
        public LoginResponse(string token, string refreshToken, bool status)
        {
            Token = token;
            RefreshToken = refreshToken;
            Status = status;
        }

        // Constructeur avec quatre arguments
        public LoginResponse(string token, string refreshToken, bool status, string errorMessage)
        {
            Token = token;
            RefreshToken = refreshToken;
            Status = status;
            ErrorMessage = errorMessage;
        }
    }
}
