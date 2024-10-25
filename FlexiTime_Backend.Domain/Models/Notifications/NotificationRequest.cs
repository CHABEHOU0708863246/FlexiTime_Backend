using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Notifications
{
    public class NotificationRequest
    {
        [Required(ErrorMessage = "L'ID de l'utilisateur est obligatoire.")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le message est obligatoire.")]
        [StringLength(500, ErrorMessage = "Le message ne peut pas dépasser 500 caractères.")]
        public string Message { get; set; } = string.Empty;

        // L'état de lecture est facultatif car par défaut il peut être faux lors de la création
        public bool IsRead { get; set; } = false;
    }
}
