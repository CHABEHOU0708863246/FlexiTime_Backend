namespace FlexiTime_Backend.Domain.Models.LeaveModel
{
    public class LeaveResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public Leave? Leave { get; set; } // L'objet Leave retourné en cas de succès
    }
}
