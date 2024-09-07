using FlexiTime_Backend.Domain.Models.Res;
using FlexiTime_Backend.Domain.Models.Roles;
using FlexiTime_Backend.Services.Roles;
using Microsoft.AspNetCore.Mvc;

namespace FlexiTime_Backend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class RolesController : HelperController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        ///<summary>
        /// Obtenir la Liste des roles
        ///</summary>
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                return Ok(await _roleService.GetRolesAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        ///<summary>
        /// Obtenir un role par son Identifiant
        ///</summary>
        ///<param name="id"></param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleAsync(string id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new Response(false, "Données non valides"));
                var result = await _roleService.GetRoleAsync(id);

                return result.Data != null ? Ok(result) : NotFound(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }
        }

        ///<summary>
        /// Créer un Rôle
        ///</summary>

        [HttpPost("")]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleRequest request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new Response(false, "Données non valides"));

                var result = await _roleService.AddRoleAsync(request);

                if (result.Data is null) return BadRequest(result);

                return Created("result", result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

        }

        ///<summary>
        /// Modifier un Rôle par son Identifiant
        ///</summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoleAsync(string id, [FromBody] RoleRequest request)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new Response(false, "Données non valides"));

                var result = await _roleService.UpdateRoleAsync(id, request);

                return result.Data != null ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

        }

        ///<summary>
        /// Supprimer un Role par son Identifiant
        ///</summary>
        ///<param name="id"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleAsync(string id)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new Response(false, "Données non valides"));

                var result = await _roleService.DeleteRoleAsync(id);

                return result.Data != null ? Ok(result) : NotFound(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new Response(false, ex.Message));
            }

        }
    }
}
