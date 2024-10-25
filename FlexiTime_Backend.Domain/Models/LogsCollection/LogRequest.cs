using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.LogsCollection
{
    public class LogRequest
    {
        [Required(ErrorMessage = "L'action est obligatoire.")]
        public string Action { get; set; } = string.Empty; // Type d'action

        [Required(ErrorMessage = "L'utilisateur ayant effectué l'action est obligatoire.")]
        public string PerformedBy { get; set; } = string.Empty; // Id de l'utilisateur ayant effectué l'action

        [Required(ErrorMessage = "L'utilisateur concerné est obligatoire.")]
        public string AffectedUser { get; set; } = string.Empty; // Id de l'utilisateur concerné

        public string? Description { get; set; } // Description optionnelle de l'action
    }
}
