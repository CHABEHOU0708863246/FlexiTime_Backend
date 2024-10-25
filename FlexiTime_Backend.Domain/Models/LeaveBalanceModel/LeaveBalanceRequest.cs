using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.LeaveBalanceModel
{
    public class LeaveBalanceRequest
    {
        [Required(ErrorMessage = "L'ID de l'utilisateur est obligatoire.")]
        public string UserId { get; set; } = string.Empty; // Référence à l'utilisateur

        [Required(ErrorMessage = "Le solde de congés payés est obligatoire.")]
        [Range(0, double.MaxValue, ErrorMessage = "Le solde de congés payés doit être positif.")]
        public double PaidLeaveBalance { get; set; }

        [Required(ErrorMessage = "Le solde de congés non payés est obligatoire.")]
        [Range(0, double.MaxValue, ErrorMessage = "Le solde de congés non payés doit être positif.")]
        public double UnpaidLeaveBalance { get; set; }

        [Required(ErrorMessage = "Le solde de congés maladie est obligatoire.")]
        [Range(0, double.MaxValue, ErrorMessage = "Le solde de congés maladie doit être positif.")]
        public double SickLeaveBalance { get; set; }
    }
}
