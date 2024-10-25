using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FlexiTime_Backend.Domain.Models.LeaveModel
{
    public class LeaveRequest
    {
        [Required(ErrorMessage = "L'ID de l'utilisateur est obligatoire.")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le type de congé est obligatoire.")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LeaveType Type { get; set; } = LeaveType.Paid;

        [Required(ErrorMessage = "La date de début est obligatoire.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "La date de fin est obligatoire.")]
        [DataType(DataType.Date)]
        [CompareDates("StartDate", ErrorMessage = "La date de fin doit être postérieure à la date de début.")]
        public DateTime EndDate { get; set; }

        [StringLength(500, ErrorMessage = "Le commentaire ne peut pas dépasser 500 caractères.")]
        public string Comment { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La raison ne peut pas dépasser 500 caractères.")]
        public string Reason { get; set; } = string.Empty;
    }

    // Custom Validation Attribute for comparing dates
    public class CompareDatesAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public CompareDatesAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDate = (DateTime)validationContext.ObjectType.GetProperty(_comparisonProperty)?.GetValue(validationContext.ObjectInstance);
            var endDate = (DateTime)value;

            if (endDate < startDate)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
