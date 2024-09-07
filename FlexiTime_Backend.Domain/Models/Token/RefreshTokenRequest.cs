using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Token
{
    public class RefreshTokenRequest
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        // Ajout du constructeur
        public RefreshTokenRequest(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }
    }
}
