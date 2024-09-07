using FlexiTime_Backend.Domain.Models.Res;
using FlexiTime_Backend.Domain.Models.Roles;

namespace FlexiTime_Backend.Services.Roles
{
    public interface IRoleService
    {
        Task<IList<Role>> GetRolesAsync();
        Task<ApiResponseData<Role>> GetRoleAsync(string roleId);
        Task<ApiResponseData<Role>> AddRoleAsync(RoleRequest request);
        Task<ApiResponseData<Role>> UpdateRoleAsync(string roleId, RoleRequest request);
        Task<ApiResponseData<Role>> DeleteRoleAsync(string roleId);
    }
}
