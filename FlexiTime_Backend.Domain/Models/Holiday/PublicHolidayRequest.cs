using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Holiday
{
    public class PublicHolidayRequest
    {
        [Required]
        public string HolidayName { get; set; } = string.Empty;

        [Required]
        public DateTime HolidayDate { get; set; }

        public string CountryCode { get; set; } = string.Empty;
    }
}
