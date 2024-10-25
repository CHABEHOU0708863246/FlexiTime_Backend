using FlexiTime_Backend.Domain.Models.Users;
using FlexiTime_Backend.Infra.Mongo;
using FlexiTime_Backend.Services.LeavesBalances;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FlexiTime_Backend.Services.Users
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<UserService> _logger;
        private readonly ILeaveBalanceService _leaveBalanceService;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
                           ILogger<UserService> logger, ILeaveBalanceService leaveBalanceService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _leaveBalanceService = leaveBalanceService;
        }

        #region Enregistre un nouvel utilisateur et crée des rôles si nécessaire
        public async Task<UserResponse> RegisterUserAsync(UserRequest userRequest)
        {
            ArgumentNullException.ThrowIfNull(userRequest, nameof(userRequest));

            if (string.IsNullOrEmpty(userRequest.Email))
            {
                throw new ArgumentException("L'e-mail ne peut pas être nul ou vide.", nameof(userRequest.Email));
            }

            var userExist = await _userManager.FindByEmailAsync(userRequest.Email);
            if (userExist != null)
            {
                return new UserResponse { Succeeded = false, Errors = new[] { "L'e-mail existe déjà." }, User = null };
            }

            if (userRequest.Roles == null || !userRequest.Roles.Any())
            {
                throw new ArgumentException(nameof(userRequest.Roles), "Les rôles ne peuvent pas être nuls ou vides.");
            }

            var roles = userRequest.Roles.Distinct().ToList();
            var existingRoles = new HashSet<string>(_roleManager.Roles.Select(r => r.Name));
            var rolesToCreate = roles.Except(existingRoles).ToList();

            foreach (var role in rolesToCreate)
            {
                var roleCreationResult = await _roleManager.CreateAsync(new ApplicationRole { Name = role });
                if (!roleCreationResult.Succeeded)
                {
                    return new UserResponse { Succeeded = false, Errors = roleCreationResult.Errors.Select(e => $"Échec lors de la création du rôle {role}: {e.Description}"), User = null };
                }
                existingRoles.Add(role);
            }

            var newUser = new ApplicationUser
            {
                Email = userRequest.Email,
                PhoneNumber = userRequest.PhoneNumber,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                UserName = userRequest.Email,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                HireDate = userRequest.HireDate,
                IsPartTime = userRequest.IsPartTime,
                WorkingHours = userRequest.WorkingHours
            };

            if (string.IsNullOrEmpty(userRequest.Password))
            {
                throw new ArgumentException(nameof(userRequest.Password), "Le mot de passe ne peut pas être nul ou vide.");
            }

            try
            {
                // Création de l'utilisateur
                var createUserResult = await _userManager.CreateAsync(newUser, userRequest.Password);
                if (createUserResult.Succeeded)
                {
                    // Ajout des rôles
                    await _userManager.AddToRolesAsync(newUser, roles);

                    // Initialiser le solde de congés ici
                    await _leaveBalanceService.CreateInitialLeaveBalanceAsync(newUser.Id.ToString(), userRequest.HireDate, userRequest.IsPartTime);

                    return new UserResponse { Succeeded = true, User = newUser };
                }

                var addToRolesResult = await _userManager.AddToRolesAsync(newUser, roles);
                if (!addToRolesResult.Succeeded)
                {
                    var deletionResult = await _userManager.DeleteAsync(newUser);
                    if (!deletionResult.Succeeded)
                    {
                        var errorMessages = addToRolesResult.Errors.Select(e => e.Description)
                            .Concat(new[] { "Erreur lors de la suppression de l’utilisateur après l’échec de l’ajout des rôles." });
                        return new UserResponse { Succeeded = false, Errors = errorMessages, User = null };
                    }
                    return new UserResponse { Succeeded = false, Errors = addToRolesResult.Errors.Select(e => e.Description), User = null };
                }

                // Appeler la méthode pour mettre à jour le solde de congés
                await _leaveBalanceService.UpdateLeaveBalanceAutomatically(newUser.Id.ToString());
            }
            catch (Exception ex)
            {
                return new UserResponse { Succeeded = false, Errors = new[] { $"Erreur inattendue : {ex.Message}" }, User = null };
            }

            return new UserResponse { Succeeded = true, Errors = Enumerable.Empty<string>(), User = newUser };
        }

        #endregion

        #region Retourne tous les utilisateurs
        public IEnumerable<User> GetAllUsers()
        {
            var users = _userManager.Users.ToList();
            var userDtos = users.Select(async u =>
            {
                var roles = await _userManager.GetRolesAsync(u);
                return new User
                {
                    Id = u.Id.ToString(),
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,
                    IsEnabled = u.IsEnabled,
                    Roles = roles.ToList()
                };
            });

            return Task.WhenAll(userDtos).Result;
        }
        #endregion

        #region Retourne un utilisateur par son identifiant
        public async Task<User?> GetUserByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new User
            {
                Id = user.Id.ToString(),
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IsEnabled = user.IsEnabled,
                Roles = roles.ToList()
            };

            return userDto;
        }
        #endregion

        #region Active ou désactive le compte utilisateur
        public async Task<IdentityResult?> EnableOrDesableUserAccountAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return null;

            user.IsEnabled = !user.IsEnabled;
            var result = await _userManager.UpdateAsync(user);

            return result;
        }
        #endregion
    }
}
