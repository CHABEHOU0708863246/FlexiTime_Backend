namespace FlexiTime_Backend.Domain.Models.Roles
{
    public class RoleResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public Role? Role { get; set; }
    }
}
