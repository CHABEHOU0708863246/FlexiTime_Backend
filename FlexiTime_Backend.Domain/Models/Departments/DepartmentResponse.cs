namespace FlexiTime_Backend.Domain.Models.Departments
{
    public class DepartmentResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public Department? Department { get; set; }
    }
}
