using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Reports
{
    public class ReportRequest
    {
        [Required(ErrorMessage = "Le type de rapport est obligatoire.")]
        public string ReportType { get; set; } = string.Empty; // 'Absences', 'Congés approuvés', etc.

        [Required(ErrorMessage = "L'identifiant de l'utilisateur générant le rapport est obligatoire.")]
        public string GeneratedBy { get; set; } = string.Empty; // ID de l'utilisateur générant le rapport

        [Required(ErrorMessage = "Les données du rapport sont obligatoires.")]
        public List<string> Data { get; set; } = new List<string>(); // Contenu des rapports (statistiques)
    }
}
