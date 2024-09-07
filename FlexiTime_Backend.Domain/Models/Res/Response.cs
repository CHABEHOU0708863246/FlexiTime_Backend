using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Res
{
    public class Response
    {
        [Required]
        public bool Success { get; set; }

        [Required]
        public string Message { get; set; }

        // Constructeur par défaut
        public Response() { }

        // Constructeur avec deux arguments
        public Response(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
