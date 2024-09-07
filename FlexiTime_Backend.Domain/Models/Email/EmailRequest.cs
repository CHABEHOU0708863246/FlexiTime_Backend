using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Email
{
    public class EmailRequest
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        // Constructeur par défaut
        public EmailRequest() { }

        // Constructeur avec quatre arguments
        public EmailRequest(string from, string to, string subject, string body)
        {
            From = from;
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}
