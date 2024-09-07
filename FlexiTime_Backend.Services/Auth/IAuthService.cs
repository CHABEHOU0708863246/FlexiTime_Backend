using FlexiTime_Backend.Domain.Models.Login;
using FlexiTime_Backend.Domain.Models.Password;
using FlexiTime_Backend.Domain.Models.Res;
using FlexiTime_Backend.Domain.Models.Token;
using Microsoft.AspNetCore.Identity;

namespace FlexiTime_Backend.Services.Auth
{
    /// <summary>
    /// Interface pour les services d'authentification.
    /// Définit les opérations liées à l'authentification, la gestion des mots de passe et les jetons.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Connecte un utilisateur en utilisant les informations d'identification fournies.
        /// </summary>
        /// <param name="request">Les informations de connexion de l'utilisateur.</param>
        /// <returns>Un résultat de connexion indiquant le succès ou l'échec de la tentative de connexion.</returns>
        Task<SignInResult> LogInAsync(LoginRequest request);

        /// <summary>
        /// Authentifie un utilisateur et retourne les détails du jeton d'authentification.
        /// </summary>
        /// <param name="request">Les informations de connexion de l'utilisateur.</param>
        /// <returns>Une réponse de connexion contenant les détails du jeton d'authentification.</returns>
        Task<LoginResponse> AuthenticateAsync(LoginRequest request);

        /// <summary>
        /// Déconnecte l'utilisateur actuellement connecté.
        /// </summary>
        /// <returns>Un booléen indiquant le succès ou l'échec de la déconnexion.</returns>
        Task<bool> LogoutAsync();

        /// <summary>
        /// Rafraîchit le jeton d'authentification de l'utilisateur.
        /// </summary>
        /// <param name="request">La demande de rafraîchissement du jeton.</param>
        /// <returns>Une réponse de connexion contenant le nouveau jeton d'authentification.</returns>
        Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);

        /// <summary>
        /// Gère la demande de réinitialisation du mot de passe de l'utilisateur.
        /// </summary>
        /// <param name="request">La demande de réinitialisation du mot de passe.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la demande de réinitialisation du mot de passe.</returns>
        Task<Response> ForgotPasswordAsync(ForgotPasswordRequest request, CancellationToken cancellationToken);

        /// <summary>
        /// Réinitialise le mot de passe de l'utilisateur.
        /// </summary>
        /// <param name="request">La demande de réinitialisation du mot de passe avec le nouveau mot de passe.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la réinitialisation du mot de passe.</returns>
        Task<Response> ResetPasswordAsync(ResetPasswordRequest request);

        /// <summary>
        /// Change le mot de passe de l'utilisateur.
        /// </summary>
        /// <param name="userName">Le nom d'utilisateur dont le mot de passe doit être changé.</param>
        /// <param name="changePassword">Les détails de la demande de changement de mot de passe.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec du changement de mot de passe.</returns>
        Task<Response> ChangePasswordAsync(string userName, ChangePasswordRequest changePassword);
    }
}
