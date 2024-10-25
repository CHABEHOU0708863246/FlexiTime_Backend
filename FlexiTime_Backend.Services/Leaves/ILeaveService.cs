using FlexiTime_Backend.Domain.Models.LeaveModel;

namespace FlexiTime_Backend.Services.Leaves
{
    /// <summary>
    /// Interface pour les services de gestion des congés.
    /// Définit les opérations liées à la création, la consultation et la gestion des congés.
    /// </summary>
    public interface ILeaveService
    {
        /// <summary>
        /// Soumet une nouvelle demande de congé pour l'utilisateur.
        /// </summary>
        /// <param name="request">Les détails de la demande de congé.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la demande de congé.</returns>
        Task<LeaveResponse> CreateLeaveRequestAsync(LeaveRequest request);

        /// <summary>
        /// Récupère toutes les demandes de congé d'un utilisateur.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut les demandes de congé.</param>
        /// <returns>Une liste de demandes de congé.</returns>
        Task<IEnumerable<Leave>> GetLeavesByUserIdAsync(string userId);

        /// <summary>
        /// Récupère une demande de congé spécifique par ID.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à récupérer.</param>
        /// <returns>La demande de congé correspondante.</returns>
        Task<Leave?> GetLeaveByIdAsync(string leaveId);

        /// <summary>
        /// Approuve une demande de congé.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à approuver.</param>
        /// <param name="approverId">L'ID de l'utilisateur qui approuve.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de l'approbation.</returns>
        Task<LeaveResponse> ApproveLeaveRequestAsync(string leaveId, string approverId);

        /// <summary>
        /// Rejette une demande de congé.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à rejeter.</param>
        /// <param name="approverId">L'ID de l'utilisateur qui rejette.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec du rejet.</returns>
        Task<LeaveResponse> RejectLeaveRequestAsync(string leaveId, string approverId);

        /// <summary>
        /// Annule une demande de congé.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à annuler.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de l'annulation.</returns>
        Task<LeaveResponse> CancelLeaveRequestAsync(string leaveId);

        /// <summary>
        /// Récupère toutes les demandes de congé avec des détails sur l'utilisateur.
        /// </summary>
        /// <returns>Une liste de toutes les demandes de congé.</returns>
        Task<IEnumerable<Leave>> GetAllLeavesAsync();
    }
}
