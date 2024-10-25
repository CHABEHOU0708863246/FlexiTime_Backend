using FlexiTime_Backend.Domain.Models.Holiday;
using FlexiTime_Backend.Domain.Models.LeaveBalanceModel;
using FlexiTime_Backend.Domain.Models.LeaveModel;
using MongoDB.Driver;

namespace FlexiTime_Backend.Services.Leaves
{
    /// <summary>
    /// Classe de service pour la gestion des congés.
    /// Implémente les opérations définies dans l'interface ILeaveService.
    /// </summary>
    public class LeaveService : ILeaveService
    {
        private readonly IMongoCollection<Leave> _leaveCollection;
        private readonly IMongoCollection<LeaveBalance> _leaveBalanceCollection;
        private readonly IMongoCollection<PublicHolidayRequest> _holidayCollection;

        public LeaveService(
            IMongoDatabase database)
        {
            _leaveCollection = database.GetCollection<Leave>("Leaves");
            _leaveBalanceCollection = database.GetCollection<LeaveBalance>("LeaveBalances");
            _holidayCollection = database.GetCollection<PublicHolidayRequest>("Holidays");
        }

        #region Create and Manage Leave Requests

        /// <summary>
        /// Soumet une nouvelle demande de congé pour l'utilisateur.
        /// </summary>
        /// <param name="request">Les détails de la demande de congé.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de la demande de congé.</returns>
        public async Task<LeaveResponse> CreateLeaveRequestAsync(LeaveRequest request)
        {
            try
            {
                // 1. Calculer la durée réelle en excluant les jours fériés
                var duration = await CalculateBusinessDaysAsync(request.StartDate, request.EndDate);
                if (duration <= 0)
                {
                    return new LeaveResponse
                    {
                        Succeeded = false,
                        Errors = new List<string> { "La période de congé sélectionnée n'inclut aucun jour ouvrable." }
                    };
                }

                // 2. Vérifier le solde de congés disponible
                var leaveBalance = await _leaveBalanceCollection
                    .Find(lb => lb.UserId == request.UserId)
                    .FirstOrDefaultAsync();

                if (leaveBalance == null)
                {
                    return new LeaveResponse
                    {
                        Succeeded = false,
                        Errors = new List<string> { "Solde de congés non trouvé pour cet utilisateur." }
                    };
                }

                // Vérifier si le solde est suffisant selon le type de congé
                bool hasSufficientBalance = request.Type switch
                {
                    LeaveType.Paid => leaveBalance.PaidLeaveBalance >= duration,
                    LeaveType.Unpaid => leaveBalance.UnpaidLeaveBalance >= duration,
                    LeaveType.Sick => leaveBalance.SickLeaveBalance >= duration,
                    _ => false
                };

                if (!hasSufficientBalance)
                {
                    return new LeaveResponse
                    {
                        Succeeded = false,
                        Errors = new List<string> { $"Solde de congés insuffisant. Solde actuel: {GetCurrentBalance(leaveBalance, request.Type)}, Durée demandée: {duration} jours." }
                    };
                }

                // 3. Créer la demande de congé
                var leave = new Leave
                {
                    UserId = request.UserId,
                    Type = request.Type,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Status = LeaveStatus.Pending,
                    Comment = request.Comment,
                    Reason = request.Reason,
                    RequestedAt = DateTime.UtcNow
                };

                await _leaveCollection.InsertOneAsync(leave);

                return new LeaveResponse { Succeeded = true, Leave = leave };
            }
            catch (Exception ex)
            {
                return new LeaveResponse
                {
                    Succeeded = false,
                    Errors = new List<string> { $"Erreur lors de la création de la demande de congé: {ex.Message}" }
                };
            }
        }

        private async Task<double> CalculateBusinessDaysAsync(DateTime startDate, DateTime endDate)
        {
            var holidays = await _holidayCollection
                .Find(h => h.HolidayDate >= startDate && h.HolidayDate <= endDate)
                .ToListAsync();

            double businessDays = 0;
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                if (IsBusinessDay(date, holidays))
                {
                    businessDays++;
                }
            }

            return businessDays;
        }

        private bool IsBusinessDay(DateTime date, List<PublicHolidayRequest> holidays)
        {
            // Exclure les weekends
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            // Exclure les jours fériés
            return !holidays.Any(h => h.HolidayDate.Date == date.Date);
        }

        private double GetCurrentBalance(LeaveBalance balance, LeaveType type)
        {
            return type switch
            {
                LeaveType.Paid => balance.PaidLeaveBalance,
                LeaveType.Unpaid => balance.UnpaidLeaveBalance,
                LeaveType.Sick => balance.SickLeaveBalance,
                _ => 0
            };
        }

        /// <summary>
        /// Approuve une demande de congé.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à approuver.</param>
        /// <param name="approverId">L'ID de l'utilisateur qui approuve.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de l'approbation.</returns>
        public async Task<LeaveResponse> ApproveLeaveRequestAsync(string leaveId, string approverId)
        {
            var leave = await _leaveCollection.Find(l => l.Id == leaveId).FirstOrDefaultAsync();
            if (leave == null)
            {
                return new LeaveResponse { Succeeded = false, Errors = new List<string> { "Demande de congé introuvable." } };
            }

            // Calculer la durée du congé approuvé
            var duration = (leave.EndDate - leave.StartDate).TotalDays + 1;

            // Récupérer le solde de congé de l'utilisateur
            var leaveBalance = await _leaveBalanceCollection.Find(lb => lb.UserId == leave.UserId).FirstOrDefaultAsync();
            if (leaveBalance == null)
            {
                return new LeaveResponse { Succeeded = false, Errors = new List<string> { "Solde de congé introuvable pour cet utilisateur." } };
            }

            // Déduire la durée du congé approuvé du solde correspondant
            var updateBalance = leave.Type switch
            {
                LeaveType.Paid => Builders<LeaveBalance>.Update.Inc(lb => lb.PaidLeaveBalance, -duration),
                LeaveType.Unpaid => Builders<LeaveBalance>.Update.Inc(lb => lb.UnpaidLeaveBalance, -duration),
                LeaveType.Sick => Builders<LeaveBalance>.Update.Inc(lb => lb.SickLeaveBalance, -duration),
                _ => null
            };

            if (updateBalance != null)
            {
                await _leaveBalanceCollection.UpdateOneAsync(lb => lb.UserId == leave.UserId, updateBalance);
            }

            // Mettre à jour le statut de la demande de congé
            var updateLeave = Builders<Leave>.Update
                .Set(l => l.Status, LeaveStatus.Approved)
                .Set(l => l.ApprovedBy, approverId)
                .Set(l => l.ApprovedAt, DateTime.UtcNow);

            var result = await _leaveCollection.UpdateOneAsync(l => l.Id == leaveId, updateLeave);

            return new LeaveResponse { Succeeded = result.ModifiedCount > 0 };
        }


        /// <summary>
        /// Rejette une demande de congé.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à rejeter.</param>
        /// <param name="approverId">L'ID de l'utilisateur qui rejette.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec du rejet.</returns>
        public async Task<LeaveResponse> RejectLeaveRequestAsync(string leaveId, string approverId)
        {
            var update = Builders<Leave>.Update
                .Set(l => l.Status, LeaveStatus.Rejected)
                .Set(l => l.ApprovedBy, approverId)
                .Set(l => l.ApprovedAt, DateTime.UtcNow);

            var result = await _leaveCollection.UpdateOneAsync(l => l.Id == leaveId, update);

            return new LeaveResponse { Succeeded = result.ModifiedCount > 0 };
        }

        /// <summary>
        /// Annule une demande de congé.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à annuler.</param>
        /// <returns>Une réponse indiquant le succès ou l'échec de l'annulation.</returns>
        public async Task<LeaveResponse> CancelLeaveRequestAsync(string leaveId)
        {
            var update = Builders<Leave>.Update.Set(l => l.Status, LeaveStatus.Cancelled);
            var result = await _leaveCollection.UpdateOneAsync(l => l.Id == leaveId, update);

            return new LeaveResponse { Succeeded = result.ModifiedCount > 0 };
        }

        #endregion

        #region Retrieve Leave Requests

        /// <summary>
        /// Récupère toutes les demandes de congé d'un utilisateur.
        /// </summary>
        /// <param name="userId">L'ID de l'utilisateur dont on veut les demandes de congé.</param>
        /// <returns>Une liste de demandes de congé.</returns>
        public async Task<IEnumerable<Leave>> GetLeavesByUserIdAsync(string userId)
        {
            return await _leaveCollection.Find(l => l.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// Récupère une demande de congé spécifique par ID.
        /// </summary>
        /// <param name="leaveId">L'ID de la demande de congé à récupérer.</param>
        /// <returns>La demande de congé correspondante.</returns>
        public async Task<Leave?> GetLeaveByIdAsync(string leaveId)
        {
            return await _leaveCollection.Find(l => l.Id == leaveId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Récupère toutes les demandes de congé avec des détails sur l'utilisateur.
        /// </summary>
        /// <returns>Une liste de toutes les demandes de congé.</returns>
        public async Task<IEnumerable<Leave>> GetAllLeavesAsync()
        {
            return await _leaveCollection.Find(FilterDefinition<Leave>.Empty).ToListAsync();
        }

        #endregion
    }
}
