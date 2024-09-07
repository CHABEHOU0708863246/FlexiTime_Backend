using FlexiTime_Backend.Domain.Models.Email;
using FlexiTime_Backend.Domain.Models.Login;
using FlexiTime_Backend.Domain.Models.Password;
using FlexiTime_Backend.Domain.Models.Res;
using FlexiTime_Backend.Domain.Models.Token;
using FlexiTime_Backend.Infra.Mongo;
using FlexiTime_Backend.Services.Email;
using FlexiTime_Backend.Services.Token;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Web;

namespace FlexiTime_Backend.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService, IEmailService emailService, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _emailService = emailService;
            _logger = logger;
        }

        #region Login

        /// <summary>
        /// Authentifie un utilisateur en utilisant l'email et le mot de passe fournis.
        /// </summary>
        /// <param name="request">Les informations de connexion.</param>
        /// <returns>Le résultat de la tentative de connexion.</returns>
        public async Task<SignInResult> LogInAsync(LoginRequest request)
            => await _signInManager.PasswordSignInAsync(request.Email, request.Password, request.RememberMe, lockoutOnFailure: true);

        #endregion

        #region Authenticate

        /// <summary>
        /// Authentifie un utilisateur et génère un token JWT et un refresh token.
        /// </summary>
        /// <param name="request">Les informations de connexion.</param>
        /// <returns>Un objet LoginResponse contenant le token et le refresh token.</returns>
        public async Task<LoginResponse> AuthenticateAsync(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning($"Authentication failed: User not found for email {request.Email}");
                return new LoginResponse(string.Empty, string.Empty, false, "Utilisateur non trouvé.");
            }

            if (!user.IsEnabled)
            {
                _logger.LogWarning($"Authentication failed: User {user.Email} is not active.");
                return new LoginResponse(string.Empty, string.Empty, false, "L'utilisateur n'est pas actif.");
            }

            try
            {
                var token = await _tokenService.GenerateTokenAsync(user, request.RememberMe);
                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);
                await _userManager.UpdateAsync(user);

                return new LoginResponse(token, user.RefreshToken, true);
            }
            catch (Exception ex)
            {
                return new LoginResponse(string.Empty, string.Empty, false, $"Erreur lors de l'authentification : {ex.Message}");
            }
        }

        #endregion

        #region Logout

        /// <summary>
        /// Déconnecte l'utilisateur actuel.
        /// </summary>
        /// <returns>Un booléen indiquant le succès de l'opération.</returns>
        public async Task<bool> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        #endregion

        #region RefreshToken

        /// <summary>
        /// Renouvelle le token JWT en utilisant le refresh token fourni.
        /// </summary>
        /// <param name="request">Les informations de demande de refresh token.</param>
        /// <returns>Un objet LoginResponse contenant le nouveau token et le nouveau refresh token.</returns>
        public async Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.Token);
            if (principal == null) return null;

            var username = principal.Identity?.Name;
            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) return null;

            var token = await _tokenService.GenerateTokenAsync(user, rememberMe: true);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(3);
            await _userManager.UpdateAsync(user);

            return new LoginResponse(token, user.RefreshToken, true);
        }

        #endregion

        #region ForgotPassword

        /// <summary>
        /// Gère la demande de réinitialisation du mot de passe en envoyant un email avec un lien de réinitialisation.
        /// </summary>
        /// <param name="request">Les informations de demande de réinitialisation du mot de passe.</param>
        /// <param name="cancellationToken">Le token d'annulation.</param>
        /// <returns>Une réponse indiquant si l'email a été envoyé avec succès.</returns>
        public async Task<Response> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(nameof(request.Email));
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null || user.IsEnabled is false)
                return new Response(false, "Veuillez entrer une adresse e-mail valide pour régénérer un nouveau mot de passe");

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var token = GenerateMailToken(new ForgotPassword(request.Email, code));
            var path = string.Concat(request.RedirectPath, HttpUtility.UrlEncode(token));
            var body = _emailService.GeneratePasswordResetEmailContent(path);
            EmailRequest emailRequest = new EmailRequest(string.Empty, user.Email!, "Réinitialisation de votre mot de passe", body);
            await _emailService.SendEmailAsync(emailRequest, cancellationToken);
            return new Response(true, "Vous allez recevoir un email pour regénérer un nouveau mot de passe. Si vous n'avez pas reçu l'email, pensez à regarder dans vos spams.");
        }

        #endregion

        #region ResetPassword

        /// <summary>
        /// Réinitialise le mot de passe de l'utilisateur en utilisant le token de réinitialisation et le nouveau mot de passe.
        /// </summary>
        /// <param name="request">Les informations de réinitialisation du mot de passe.</param>
        /// <returns>Une réponse indiquant le succès de la réinitialisation du mot de passe.</returns>
        public async Task<Response> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                return new Response(false, "Erreur lors du changement du mot de passe");

            var checkOldPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (checkOldPassword)
                return new Response(false, "Le nouveau mot de passe ne peut pas être identique au mot de passe actuel. Veuillez choisir un mot de passe différent.");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (result.Succeeded)
            {
                return new Response(result.Succeeded, "Le mot de passe a été changé avec succès. Vous pouvez maintenant vous connecter avec votre nouveau mot de passe.");
            }
            return new Response(result.Succeeded, "Votre Session a expiré");
        }

        #endregion

        #region ChangePassword

        /// <summary>
        /// Change le mot de passe de l'utilisateur actuel.
        /// </summary>
        /// <param name="userName">Le nom d'utilisateur.</param>
        /// <param name="changePassword">Les informations de changement de mot de passe.</param>
        /// <returns>Une réponse indiquant le succès ou les erreurs rencontrées lors du changement de mot de passe.</returns>
        public async Task<Response> ChangePasswordAsync(string userName, ChangePasswordRequest changePassword)
        {
            ArgumentNullException.ThrowIfNull(nameof(changePassword));

            var user = await _userManager.FindByNameAsync(userName);
            if (user is null) return new Response(false, "Utilisateur introuvable.");

            var result = await _userManager.ChangePasswordAsync(user, changePassword.OldPassword, changePassword.NewPassword);
            if (result.Succeeded)
            {
                return new Response(true, "Mot de passe changé avec succès.");
            }

            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return new Response(false, errorMessage);
        }

        #endregion

        #region GenerateMailToken

        /// <summary>
        /// Génère un token de réinitialisation de mot de passe pour l'envoi par email.
        /// </summary>
        /// <param name="request">Les informations de réinitialisation de mot de passe.</param>
        /// <returns>Le token généré.</returns>
        private string GenerateMailToken(ForgotPassword request)
        {
            try
            {
                var claimValues = new List<Claim>
                {
                    new Claim("Email", request.Email),
                    new Claim("Code", request.Code)
                };
                return _tokenService.GenerateTokenWithMultipleClaims(claimValues);
            }
            catch (Exception ex)
            {
                ex.Data[nameof(request.Code)] = request.Code;
                ex.Data[nameof(request.Email)] = request.Email;
                throw;
            }
        }

        #endregion
    }
}
