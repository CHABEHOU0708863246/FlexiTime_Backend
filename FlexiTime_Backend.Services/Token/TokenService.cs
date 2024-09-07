using FlexiTime_Backend.Domain.Configurations;
using FlexiTime_Backend.Infra.Mongo;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FlexiTime_Backend.Services.Token
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SecurityOption _securityOption;
        public TokenService(UserManager<ApplicationUser> userManager, IOptions<SecurityOption> securityOption)
        {
            _userManager = userManager;
            _securityOption = securityOption.Value;
        }

        #region Generate Token

        // Méthode pour générer un token JWT pour un utilisateur
        public async Task<string> GenerateTokenAsync(ApplicationUser user, bool rememberMe)
        {
            // Récupère les rôles de l'utilisateur
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x));

            // Création des réclamations (claims) de base pour le JWT
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Email??string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Email??string.Empty),
                new Claim("email", user.Email??string.Empty),
            };

            // Ajout des réclamations de rôle
            claims.AddRange(roleClaims);

            // Création de la clé de sécurité symétrique
            SymmetricSecurityKey securityKey = new(GetSecurityKey());

            // Création du jeton JWT
            var generatetoken = new JwtSecurityToken(
                _securityOption.Issuer,
                _securityOption.Audience,
                claims,
                expires: DateTime.UtcNow.Add(rememberMe ? TimeSpan.FromDays(1) : TimeSpan.FromMinutes(30)),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            // Génération du token en tant que chaîne de caractères
            var token = new JwtSecurityTokenHandler().WriteToken(generatetoken);
            return token;
        }
        #endregion

        #region Generate Refresh Token

        // Méthode pour générer un refresh token (un token de renouvellement) sécurisé
        public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        #endregion

        #region Validate Token and Claims

        // Méthode pour extraire les réclamations d'un token expiré
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            // Paramètres de validation du token sans validation de la durée de vie
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(GetSecurityKey()),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            // Validation du token et extraction des réclamations
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            // Vérification du type d'algorithme
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Jeton non valide");

            return principal;
        }

        #endregion

        #region Generate Token with Multiple Claims

        // Méthode pour générer un token avec des réclamations multiples et une expiration personnalisée
        public string GenerateTokenWithMultipleClaims(IEnumerable<Claim> claims, double expiration = 3600)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(GetSecurityKey());

            // Ajout d'une réclamation pour l'identifiant unique du jeton
            claims.Concat(new[] { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) });

            // Création du jeton JWT
            var jwt = new JwtSecurityToken(
                _securityOption.Issuer,
                _securityOption.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(expiration),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            // Génération du token en tant que chaîne de caractères
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);
            return tokenString;
        }

        #endregion

        #region Validate Token

        // Méthode pour valider un token JWT
        public SecurityToken ValidateToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Le jeton ne peut pas être nul ou vide.", nameof(token));

            var parameters = GetTokenValidationParameters() ??
                throw new InvalidOperationException("Les paramètres de validation des jetons n’ont pas pu être récupérés.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token) ??
                throw new SecurityTokenException("Erreur de lecture du jeton JWT.");

            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                throw new SecurityTokenException("Le jeton JWT est expiré.");
            }

            var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken securityToken);

            return principal == null ? throw new SecurityTokenException("Le jeton JWT n’a pas pu être validé.") : securityToken;
        }

        #endregion

        #region Token Validation Parameters

        // Méthode pour obtenir les paramètres de validation du token
        public TokenValidationParameters GetTokenValidationParameters()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_securityOption.Issuer))
                    throw new ArgumentException("L'émetteur ne peut pas être null ou vide.", nameof(_securityOption.Issuer));

                TokenValidationParameters parameters = new()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(GetSecurityKey()),
                    ValidIssuer = _securityOption.Issuer,
                    ValidIssuers = [_securityOption.Issuer],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                return parameters;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Helper Methods

        // Méthode privée pour obtenir la clé de sécurité symétrique à partir du secret stocké dans les options de sécurité
        private byte[] GetSecurityKey() => Encoding.UTF8.GetBytes(_securityOption.Secret);

        #endregion
    }
}
