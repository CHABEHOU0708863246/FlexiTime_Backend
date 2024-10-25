using FlexiTime_Backend.Domain.Models.LeaveModel;
using FlexiTime_Backend.Services.Leaves;
using Microsoft.AspNetCore.Mvc;

namespace FlexiTime_Backend.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveController : HelperController
    {
        private readonly ILeaveService _leaveService;

        public LeaveController(ILeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        #region Leave Request Management

        /// <summary>
        /// Soumet une nouvelle demande de congé.
        /// </summary>
        /// <param name="request">Les détails de la demande de congé.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la demande de congé.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateLeaveRequest([FromBody] LeaveRequest request)
        {
            var response = await _leaveService.CreateLeaveRequestAsync(request);
            if (response.Succeeded)
            {
                return CreatedAtAction(nameof(GetLeaveById), new { leaveId = response.Leave?.Id }, response.Leave);
            }
            return BadRequest(response.Errors);
        }

        /// <summary>
        /// Approuve une demande de congé.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à approuver.</param>
        /// <param name="approverId">L'ID de l'utilisateur qui approuve.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de l'approbation.</returns>
        [HttpPost("{leaveId}/approve")]
        public async Task<IActionResult> ApproveLeaveRequest(string leaveId, [FromQuery] string approverId)
        {
            var response = await _leaveService.ApproveLeaveRequestAsync(leaveId, approverId);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response.Errors);
        }

        /// <summary>
        /// Rejette une demande de congé.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à rejeter.</param>
        /// <param name="approverId">L'ID de l'utilisateur qui rejette.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec du rejet.</returns>
        [HttpPost("{leaveId}/reject")]
        public async Task<IActionResult> RejectLeaveRequest(string leaveId, [FromQuery] string approverId)
        {
            var response = await _leaveService.RejectLeaveRequestAsync(leaveId, approverId);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response.Errors);
        }

        /// <summary>
        /// Annule une demande de congé.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à annuler.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de l'annulation.</returns>
        [HttpPost("{leaveId}/cancel")]
        public async Task<IActionResult> CancelLeaveRequest(string leaveId)
        {
            var response = await _leaveService.CancelLeaveRequestAsync(leaveId);
            if (response.Succeeded)
            {
                return Ok(response);
            }
            return BadRequest(response.Errors);
        }

        #endregion

        #region Retrieve Leave Requests

        /// <summary>
        /// Récupère toutes les demandes de congé d'un utilisateur.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut les demandes de congé.</param>
        /// <returns>Une liste de demandes de congé.</returns>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetLeavesByUserId(string userId)
        {
            var leaves = await _leaveService.GetLeavesByUserIdAsync(userId);
            return Ok(leaves);
        }

        /// <summary>
        /// Récupère une demande de congé spécifique par ID.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à récupérer.</param>
        /// <returns>La demande de congé correspondante.</returns>
        [HttpGet("{leaveId}")]
        public async Task<IActionResult> GetLeaveById(string leaveId)
        {
            var leave = await _leaveService.GetLeaveByIdAsync(leaveId);
            if (leave == null)
            {
                return NotFound();
            }
            return Ok(leave);
        }

        /// <summary>
        /// Récupère toutes les demandes de congé.
        /// </summary>
        /// <returns>Une liste de toutes les demandes de congé.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllLeaves()
        {
            var leaves = await _leaveService.GetAllLeavesAsync();
            return Ok(leaves);
        }

        #endregion
    }
}
