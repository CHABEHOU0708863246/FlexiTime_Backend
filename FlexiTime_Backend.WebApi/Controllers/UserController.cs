using FlexiTime_Backend.Domain.Exceptions;
using FlexiTime_Backend.Domain.Models.Res;
using FlexiTime_Backend.Domain.Models.Users;
using FlexiTime_Backend.Services.Users;
using Microsoft.AspNetCore.Mvc;

namespace FlexiTime_Backend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : HelperController
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        ///<summary>
        /// Créer un utilisateur
        ///</summary>
        [HttpPost("")]
        public async Task<IActionResult> AddUser([FromBody] UserRequest request)
        {
            // Log the incoming request
            _logger.LogInformation("Received request to create a user: {@Request}", request);

            if (request == null)
            {
                _logger.LogWarning("Request body is null");
                return BadRequest("Request body cannot be null");
            }

            if (!ModelState.IsValid)
            {
                // Log the model state errors
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        _logger.LogWarning("Model state error: {Error}", error.ErrorMessage);
                    }
                }
                return BadRequest("Données non valides");
            }

            try
            {
                var result = await _userService.RegisterUserAsync(request);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created successfully: {@Result}", result);
                    return Created("result", result);
                }

                // Log the errors from the result
                foreach (var error in result.Errors)
                {
                    _logger.LogWarning("User registration error: {Error}", error.ToString());
                }
                return BadRequest(result.Errors);
            }
            catch (ServiceException ex)
            {
                // Log the exception details
                _logger.LogError(ex, "Service exception occurred while creating user");
                return BadRequest(ex.ErrorMessage);
            }
            catch (Exception ex)
            {
                // Log unexpected exceptions
                _logger.LogError(ex, "Unexpected error occurred while creating user");
                return StatusCode(500, "An unexpected error occurred");
            }
        }

        /// <summary>
        /// Obtenir un utilisateur par son Identifiant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("données non valides");
                }

                var user = await _userService.GetUserByIdAsync(id);

                if (user == null) return NotFound();

                return Ok(user);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.ErrorMessage);
            }
        }

        /// <summary>
        /// Obtenir la liste de tous utilisateurs
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        public IActionResult GetAll()
        {
            try
            {
                var users = _userService.GetAllUsers();
                return Ok(users);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.ErrorMessage);
            }
        }


        /// <summary>
        /// Activer ou désactiver le compte d'un utilisateur par son Identifiant
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> EnableOrDesableUserAccount(string id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("données non valides");
                }

                var result = await _userService.EnableOrDesableUserAccountAsync(id);
                if (result == null) return NotFound("Utilisateur introuvable");

                if (result.Succeeded)
                {
                    return Ok(new Response(result.Succeeded, "Mise à jour du compte effectué"));
                }
                return BadRequest();
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.ErrorMessage);
            }

        }
    }
}
