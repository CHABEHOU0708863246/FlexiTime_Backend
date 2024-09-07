using FlexiTime_Backend.Domain.Models.Users;
using Microsoft.AspNetCore.Identity;

namespace FlexiTime_Backend.Services.Users
{
    public interface IUserService
    {
        Task<UserResponse> RegisterUserAsync(UserRequest request);
        IEnumerable<User> GetAllUsers();
        Task<User?> GetUserByIdAsync(string id);
        Task<IdentityResult?> EnableOrDesableUserAccountAsync(string id);
    }
}
