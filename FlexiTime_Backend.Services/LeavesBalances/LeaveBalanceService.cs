using FlexiTime_Backend.Domain.Models.LeaveBalanceModel;
using MongoDB.Driver;

namespace FlexiTime_Backend.Services.LeavesBalances
{
    /// <summary>
    /// Service de gestion des soldes de congés.
    /// Met en œuvre les opérations définies dans l'interface ILeaveBalanceService.
    /// </summary>
    public class LeaveBalanceService : ILeaveBalanceService
    {
        private readonly IMongoCollection<LeaveBalance> _leaveBalanceCollection;

        public LeaveBalanceService(IMongoDatabase database)
        {
            _leaveBalanceCollection = database.GetCollection<LeaveBalance>("LeaveBalances");
        }

        public async Task CreateInitialLeaveBalanceAsync(string userId, DateTime hireDate, bool isPartTime)
        {
            var initialLeaveBalance = new LeaveBalance
            {
                UserId = userId,
                PaidLeaveBalance = CalculateInitialLeave(hireDate, isPartTime),
                UnpaidLeaveBalance = 0,
                SickLeaveBalance = 0,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _leaveBalanceCollection.InsertOneAsync(initialLeaveBalance);
        }

        private double CalculateInitialLeave(DateTime hireDate, bool isPartTime)
        {
            // Calculer le nombre de jours de congé à attribuer basé sur la date d'embauche
            int monthsSinceHire = (DateTime.UtcNow.Year - hireDate.Year) * 12 + DateTime.UtcNow.Month - hireDate.Month;
            return monthsSinceHire * (isPartTime ? 1.25 : 2.5);
        }


        #region Get Leave Balance

        /// <summary>
        /// Récupère le solde de congés d'un utilisateur par ID.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut récupérer le solde.</param>
        /// <returns>Le solde de congés correspondant.</returns>
        public async Task<LeaveBalance?> GetLeaveBalanceByUserIdAsync(string userId)
        {
            // Vérifier si le solde de congé existe
            var leaveBalance = await _leaveBalanceCollection.Find(lb => lb.UserId == userId).FirstOrDefaultAsync();
            if (leaveBalance == null)
            {

                // Créer un nouveau solde de congé
                leaveBalance = new LeaveBalance
                {
                    UserId = userId,
                    PaidLeaveBalance = 0,
                    UnpaidLeaveBalance = 0,
                    SickLeaveBalance = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _leaveBalanceCollection.InsertOneAsync(leaveBalance);
                Console.WriteLine($"Nouveau solde de congé créé pour l'utilisateur avec ID {userId}.");
            }
            return leaveBalance;
        }


        #endregion

        #region Update Leave Balance

        /// <summary>
        /// Met à jour le solde de congés d'un utilisateur.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut mettre à jour le solde.</param>
        /// <param name="request">Les nouvelles valeurs de solde de congés.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la mise à jour.</returns>
        public async Task<LeaveBalanceResponse> UpdateLeaveBalanceAsync(string userId, LeaveBalanceRequest request)
        {
            var update = Builders<LeaveBalance>.Update
                .Set(lb => lb.PaidLeaveBalance, request.PaidLeaveBalance)
                .Set(lb => lb.UnpaidLeaveBalance, request.UnpaidLeaveBalance)
                .Set(lb => lb.SickLeaveBalance, request.SickLeaveBalance)
                .Set(lb => lb.UpdatedAt, DateTime.UtcNow);

            var result = await _leaveBalanceCollection.UpdateOneAsync(lb => lb.UserId == userId, update);

            return new LeaveBalanceResponse
            {
                Succeeded = result.ModifiedCount > 0,
                Errors = result.ModifiedCount == 0 ? new List<string> { "Mise à jour échouée, l'utilisateur n'existe peut-être pas." } : null
            };
        }

        #endregion

        #region Delete Leave Balance

        /// <summary>
        /// Supprime le solde de congés d'un utilisateur par ID.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut supprimer le solde.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la suppression.</returns>
        public async Task<LeaveBalanceResponse> DeleteLeaveBalanceAsync(string userId)
        {
            var result = await _leaveBalanceCollection.DeleteOneAsync(lb => lb.UserId == userId);
            return new LeaveBalanceResponse
            {
                Succeeded = result.DeletedCount > 0,
                Errors = result.DeletedCount == 0 ? new List<string> { "Suppression échouée, l'utilisateur n'existe peut-être pas." } : null
            };
        }

        #endregion

        #region Update Leave Balance Automatically
        public async Task<LeaveBalanceResponse> UpdateLeaveBalanceAutomatically(string userId)
        {
            var leaveBalance = await GetLeaveBalanceByUserIdAsync(userId);
            if (leaveBalance == null)
            {
                return new LeaveBalanceResponse { Succeeded = false, Errors = new List<string> { "Solde de congé introuvable." } };
            }

            // Calculer le nombre de mois écoulés depuis la date d'embauche
            var hireDate = leaveBalance.CreatedAt;
            var currentDate = DateTime.UtcNow;
            var monthsSinceHire = (currentDate.Year - hireDate.Year) * 12 +
                                  (currentDate.Month - hireDate.Month) +
                                  (currentDate.Day - hireDate.Day) / 30;

            // Calculer le solde de congés
            double daysPerMonth = 30;
            double annualLeaveDays = 2.5;
            double totalLeaveDays = Math.Ceiling(monthsSinceHire * annualLeaveDays);

            leaveBalance.PaidLeaveBalance = Math.Min(totalLeaveDays, 90);

            // Mettre à jour la base de données
            var updateRequest = new LeaveBalanceRequest
            {
                PaidLeaveBalance = leaveBalance.PaidLeaveBalance,
                UnpaidLeaveBalance = leaveBalance.UnpaidLeaveBalance,
                SickLeaveBalance = leaveBalance.SickLeaveBalance
            };

            return await UpdateLeaveBalanceAsync(userId, updateRequest);
        }
        #endregion


    }
}
