using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Settings
{
    public class SettingRequest
    {
        [Required(ErrorMessage = "Le nombre de jours de congés annuels est obligatoire.")]
        [Range(1, 365, ErrorMessage = "Le nombre de jours de congés annuels doit être compris entre 1 et 365.")]
        public int NombreJoursCongesAnnuel { get; set; } // Ex: 30 jours/an

        [Required(ErrorMessage = "Le nombre de jours d'acquisition par mois est obligatoire.")]
        [Range(0, 31, ErrorMessage = "Les jours d'acquisition par mois doivent être compris entre 0 et 31.")]
        public double JoursAcquisitionParMois { get; set; } // Ex: 2.5 jours par mois

        [Required(ErrorMessage = "L'option autorisant les congés anticipés est obligatoire.")]
        public bool AutoriserCongesAnticipes { get; set; }

        [Required(ErrorMessage = "Les types de congés sont obligatoires.")]
        public List<string> TypesConges { get; set; } = new List<string>(); // Ex: payé, non payé, maladie, etc.
    }
}
