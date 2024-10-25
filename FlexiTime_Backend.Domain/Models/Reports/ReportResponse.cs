namespace FlexiTime_Backend.Domain.Models.Reports
{
    public class ReportResponse
    {
        public bool Succeeded { get; set; } // Statut de la requête
        public IEnumerable<string> Errors { get; set; } = new List<string>(); // Liste d'erreurs, si applicable
        public Report? Report { get; set; } // Détails du rapport généré ou récupéré
    }
}
