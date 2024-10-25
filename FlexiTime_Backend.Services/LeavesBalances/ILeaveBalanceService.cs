using FlexiTime_Backend.Domain.Models.LeaveBalanceModel;

namespace FlexiTime_Backend.Services.LeavesBalances
{
    /// <summary>
    /// Interface pour les services de gestion des soldes de congés.
    /// Définit les opérations liées à la création, la mise à jour et la récupération des soldes de congés.
    /// </summary>
    public interface ILeaveBalanceService
    {


        /// <summary>
        /// Récupère le solde de congés d'un utilisateur par ID.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut récupérer le solde.</param>
        /// <returns>Le solde de congés correspondant.</returns>
        Task<LeaveBalance?> GetLeaveBalanceByUserIdAsync(string userId);

        /// <summary>
        /// Met à jour le solde de congés d'un utilisateur.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut mettre à jour le solde.</param>
        /// <param name="request">Les nouvelles valeurs de solde de congés.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la mise à jour.</returns>
        Task<LeaveBalanceResponse> UpdateLeaveBalanceAsync(string userId, LeaveBalanceRequest request);

        /// <summary>
        /// Met à jour automatiquement le solde de congés d'un utilisateur.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut mettre à jour le solde.</param>
        /// <param name="request">Les nouvelles valeurs de solde de congés.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la mise à jour.</returns>
        Task<LeaveBalanceResponse> UpdateLeaveBalanceAutomatically(string userId);

        /// <summary>
        /// Supprime le solde de congés d'un utilisateur par ID.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut supprimer le solde.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la suppression.</returns>
        Task<LeaveBalanceResponse> DeleteLeaveBalanceAsync(string userId);

        /// <summary>
        /// Crée le solde de congés d'un utilisateur par ID en fonction de sa date d'embauche et de son temps de travail.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut supprimer le solde.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la suppression.</returns>
        Task CreateInitialLeaveBalanceAsync(string userId, DateTime hireDate, bool isPartTime);

    }
}
