namespace FlexiTime_Backend.Domain.Models.LeaveBalanceModel
{
    public class LeaveBalanceResponse
    {
        public bool Succeeded { get; set; } // Indique si l'opération a réussi
        public IEnumerable<string> Errors { get; set; } = new List<string>(); // Liste des erreurs en cas d'échec
        public LeaveBalance? LeaveBalance { get; set; } // Détails du solde de congés en cas de succès
    }
}
