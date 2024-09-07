using FlexiTime_Backend.Domain.Exceptions;
using FlexiTime_Backend.Domain.Models.Login;
using FlexiTime_Backend.Domain.Models.Password;
using FlexiTime_Backend.Domain.Models.Res;
using FlexiTime_Backend.Domain.Models.Token;
using FlexiTime_Backend.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlexiTime_Backend.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : HelperController
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Connexion
        /// </summary>
        /// <param name="requestLogin"></param>
        /// 
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest requestLogin)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new Response(false, "données non valides."));

                var result = await _authService.LogInAsync(requestLogin);

                if (!result.Succeeded)
                {

                    if (result.IsLockedOut)
                    {
                        return BadRequest(new Response(result.Succeeded, "Votre compte est bloqué."));
                    }
                    else if (result.IsNotAllowed)
                    {
                        return BadRequest(new Response(result.Succeeded, "L’utilisateur n’est pas autorisé à se connecter."));
                    }
                    else
                    {
                        return BadRequest(new Response(result.Succeeded, "Email ou Mot de passe incorrect."));
                    }
                }

                var userConnected = await _authService.AuthenticateAsync(requestLogin);
                if (userConnected != null)
                {
                    if (userConnected.Status is false) return BadRequest(new Response(result.Succeeded, "Le compte n’est pas actif."));

                    return Ok(new RefreshTokenRequest(userConnected.Token, userConnected.RefreshToken));

                }

                return BadRequest(new Response(result.Succeeded, "Erreur lors de l’authentification de l’utilisateur."));

            }
            catch (ServiceException ex)
            {
                return BadRequest(new Response(false, ex.ErrorMessage));
            }
        }

        /// <summary>
        /// Déconnexion
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutAsync();
            return Ok(result);
        }

        /// <summary>
        /// Refresh Token
        /// </summary>
        /// <param name="model"></param>
        /// 
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("données non valides");

                var response = await _authService.RefreshTokenAsync(model);

                if (response != null)
                {
                    return Ok(new RefreshTokenRequest(response.Token, response.RefreshToken));
                }

                return BadRequest("Jeton d’actualisation non valide");
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.ErrorMessage);
            }

        }

        /// <summary>
        /// Mot de passe oublié
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// 
        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("données non valides");
                var result = await _authService.ForgotPasswordAsync(request, cancellationToken);
                if (!result.Success) return BadRequest(result);

                return Ok(result);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.ErrorMessage);
            }

        }
        /// <summary>
        /// Renitialisation du mot de passe
        /// </summary>
        /// <param name="resetPassword"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest resetPassword, CancellationToken cancellationToken)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("données non valides");
                var result = await _authService.ResetPasswordAsync(resetPassword);

                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.ErrorMessage);
            }
        }

        /// <summary>
        /// Changer le Mot de passe
        /// </summary>
        /// <param name="changePassword"></param>
        /// <returns></returns>
        [Route("change-password")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePassword)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("données non valides");
                var userName = User?.Identity?.Name;
                if (userName == null) return BadRequest("Nom d'utilisateur non valide");
                var result = await _authService.ChangePasswordAsync(userName, changePassword);

                if (result.Success)
                    return Ok(result);
                return BadRequest(result);
            }
            catch (ServiceException ex)
            {
                return BadRequest(ex.ErrorMessage);
            }

        }


    }
}
