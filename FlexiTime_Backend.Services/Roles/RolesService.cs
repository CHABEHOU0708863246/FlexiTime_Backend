using FlexiTime_Backend.Domain.Models.Res;
using FlexiTime_Backend.Domain.Models.Roles;
using FlexiTime_Backend.Infra.Mongo;
using Microsoft.AspNetCore.Identity;

namespace FlexiTime_Backend.Services.Roles
{
    public class RolesService : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        public RolesService(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        #region Méthode pour récupérer tous les rôles
        public async Task<IList<Role>> GetRolesAsync()
        {
            var roles = _roleManager.Roles.Select(x => new Role()
            {
                Id = x.Id.ToString(),
                RoleName = x.Name,
                Code = x.Code
            }).ToList();

            return await Task.FromResult(roles);
        }
        #endregion

        #region Méthode pour récupérer un rôle spécifique par son ID
        public async Task<ApiResponseData<Role>> GetRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null) return new ApiResponseData<Role>(false, "Rôle introuvable", null!);

            var res = new Role
            {
                Id = role.Id.ToString(),
                Code = role.Code,
                RoleName = role.Name
            };

            return new ApiResponseData<Role>(true, "Le rôle a été créé", res);
        }
        #endregion

        #region Méthode pour ajouter un nouveau rôle
        public async Task<ApiResponseData<Role>> AddRoleAsync(RoleRequest request)
        {
            // Vérifie que la requête n'est pas nulle
            ArgumentNullException.ThrowIfNull(request, nameof(request));

            // Vérifie si le rôle existe déjà
            bool roleExist = _roleManager.Roles.Any(e => e.Name == request.RoleName);

            if (roleExist)
                return new ApiResponseData<Role>(false, "Ce rôle existe !", null!);

            // Crée un nouvel objet ApplicationRole
            var role = new ApplicationRole
            {
                Code = request.Code,
                Name = request.RoleName
            };

            // Crée le rôle dans le système
            var roleCreated = await _roleManager.CreateAsync(role);

            if (!roleCreated.Succeeded)
                throw new Exception("Une erreur inattendue s’est produite lors de l'ajout du rôle");

            var createdRole = new Role()
            {
                Id = role.Id.ToString(),
                RoleName = role.Name,
                Code = role.Code
            };

            return new ApiResponseData<Role>(true, "Rôle créé avec succès !", createdRole);
        }
        #endregion

        #region Méthode pour mettre à jour un rôle existant
        public async Task<ApiResponseData<Role>> UpdateRoleAsync(string roleId, RoleRequest request)
        {
            // Recherche le rôle à mettre à jour
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return new ApiResponseData<Role>(false, "Role introuvable", null!);

            // Mise à jour des propriétés du rôle
            role.Name = request.RoleName;
            role.Code = request.Code;

            // Enregistre les modifications
            var result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
                throw new Exception("Une erreur inattendue s’est produite lors de la mise à jour du rôle");

            var updatedRole = new Role()
            {
                Id = role.Id.ToString(),
                RoleName = role.Name,
                Code = role.Code
            };

            return new ApiResponseData<Role>(true, "Les informations du rôle ont été modifiés !", updatedRole);
        }
        #endregion

        #region Méthode pour supprimer un rôle par son ID
        public async Task<ApiResponseData<Role>> DeleteRoleAsync(string roleId)
        {
            // Recherche le rôle à supprimer
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                return new ApiResponseData<Role>(false, "Rôle introuvable", null!);

            // Supprime le rôle
            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                throw new Exception("Une erreur inattendue s’est produite lors de la Suppression du rôle");

            var roleDeleted = new Role()
            {
                Id = role.Id.ToString(),
                RoleName = role.Name,
                Code = role.Code
            };

            return new ApiResponseData<Role>(true, "Suppression effectuée !", roleDeleted);
        }
        #endregion
    }
}
