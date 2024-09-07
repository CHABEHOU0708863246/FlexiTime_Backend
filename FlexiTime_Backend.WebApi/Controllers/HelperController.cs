using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlexiTime_Backend.WebApi.Controllers
{
    /// <summary>
    /// Controller de base pour les contrôleurs qui ont besoin d'une gestion d'erreurs et de récupération de propriétés d'utilisateur.
    /// </summary>
    public abstract class HelperController : ControllerBase
    {
        /// <summary>
        /// Gère les résultats d'identité pour retourner les erreurs appropriées.
        /// </summary>
        /// <param name="result">Le résultat de l'opération d'identité.</param>
        /// <returns>Une réponse HTTP appropriée en fonction du résultat.</returns>
        protected IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return StatusCode(500, "Le résultat de l'identité est nul.");
            }

            if (!result.Succeeded)
            {
                // Ajoute les erreurs du résultat au ModelState
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                // Retourne un BadRequest avec les erreurs si ModelState n'est pas valide
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Si ModelState est valide mais le résultat n'est pas réussi
                return BadRequest("Une erreur inconnue est survenue.");
            }

            // Aucun problème, retourne OK (ou null pour indiquer le succès)
            return Ok();
        }

        /// <summary>
        /// Récupère la valeur de la propriété de l'utilisateur à partir des revendications.
        /// </summary>
        /// <param name="key">La clé de la revendication.</param>
        /// <returns>La valeur de la revendication ou null si elle n'existe pas.</returns>
        protected string GetPropertyValue(string key)
        {
            // Assure que la clé n'est pas nulle ou vide
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            // Retourne la valeur de la revendication correspondante
            return User.Claims.FirstOrDefault(x => x.Type.Equals(key, StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }
}
