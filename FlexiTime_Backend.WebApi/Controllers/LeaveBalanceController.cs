using FlexiTime_Backend.Domain.Models.LeaveBalanceModel;
using FlexiTime_Backend.Services.LeavesBalances;
using FlexiTime_Backend.Utilities.UpdateLeaveBalanceWithTimer;
using Microsoft.AspNetCore.Mvc;

namespace FlexiTime_Backend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveBalanceController : HelperController
    {
        private readonly ILeaveBalanceService _leaveBalanceService;
        private readonly LeaveBalanceUpdateService _leaveBalanceUpdateService;

        public LeaveBalanceController(ILeaveBalanceService leaveBalanceService, LeaveBalanceUpdateService leaveBalanceUpdateService)
        {
            _leaveBalanceService = leaveBalanceService;
            _leaveBalanceUpdateService = leaveBalanceUpdateService;
        }


        #region Get Leave Balance

        /// <summary>
        /// Récupère le solde de congés d'un utilisateur par ID.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut récupérer le solde.</param>
        /// <returns>Le solde de congés correspondant.</returns>
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetLeaveBalanceByUserId(string userId)
        {
            var leaveBalance = await _leaveBalanceService.GetLeaveBalanceByUserIdAsync(userId);
            if (leaveBalance != null)
            {
                return Ok(leaveBalance);
            }
            return NotFound($"Aucun solde de congé trouvé pour l'utilisateur avec ID {userId}.");
        }

        #endregion

        #region Update Leave Balance

        /// <summary>
        /// Met à jour le solde de congés d'un utilisateur.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut mettre à jour le solde.</param>
        /// <param name="request">Les nouvelles valeurs de solde de congés.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la mise à jour.</returns>
        [HttpPut]
        [Route("{userId}/update")]
        public async Task<IActionResult> UpdateLeaveBalance(string userId, [FromBody] LeaveBalanceRequest request)
        {
            var response = await _leaveBalanceService.UpdateLeaveBalanceAsync(userId, request);
            if (response.Succeeded)
            {
                return NoContent(); // Mise à jour réussie
            }
            return BadRequest(response.Errors);
        }

        #endregion

        #region Delete Leave Balance

        /// <summary>
        /// Supprime le solde de congés d'un utilisateur par ID.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut supprimer le solde.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la suppression.</returns>
        [HttpDelete]
        [Route("{userId}/delete")]
        public async Task<IActionResult> DeleteLeaveBalance(string userId)
        {
            var response = await _leaveBalanceService.DeleteLeaveBalanceAsync(userId);
            if (response.Succeeded)
            {
                return NoContent(); // Suppression réussie
            }
            return BadRequest(response.Errors);
        }

        #endregion

        #region Trigger Leave Balance Update

        /// <summary>
        /// Met à jour automatiquement le solde de congés pour tous les utilisateurs.
        /// </summary>
        /// <returns>Un message indiquant le succès de l'opération.</returns>
        [HttpPost]
        [Route("update-all")]
        public async Task<IActionResult> TriggerAutomaticLeaveBalanceUpdate()
        {
            await _leaveBalanceUpdateService.RunUpdateNowAsync();
            return Ok("Mise à jour automatique du solde de congés lancée avec succès.");
        }

        #endregion


    }
}
