using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Roles
{
    public class RoleRequest
    {
        [Required]
        public string Code { get; set; }

        [Required]
        public string RoleName { get; set; }

        public RoleRequest(string code, string roleName)
        {
            Code = code;
            RoleName = roleName;
        }
    }
}
