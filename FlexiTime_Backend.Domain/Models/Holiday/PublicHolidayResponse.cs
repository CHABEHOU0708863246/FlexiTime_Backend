namespace FlexiTime_Backend.Domain.Models.Holiday
{
    public class PublicHolidayResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public PublicHoliday? PublicHoliday { get; set; }
    }
}
