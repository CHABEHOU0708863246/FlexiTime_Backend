using FlexiTime_Backend.Domain.Models.LeaveBalanceModel;
using FlexiTime_Backend.Domain.Models.Users;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace FlexiTime_Backend.Utilities.UpdateLeaveBalanceWithTimer
{
    public class LeaveBalanceUpdateService : BackgroundService
    {
        private readonly IMongoClient _mongoClient;
        private readonly ILogger<LeaveBalanceUpdateService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromHours(24);

        private IMongoCollection<User> _userCollection;
        private IMongoCollection<LeaveBalance> _leaveBalanceCollection;

        public LeaveBalanceUpdateService(IMongoClient mongoClient, ILogger<LeaveBalanceUpdateService> logger)
        {
            _mongoClient = mongoClient;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await UpdateAllLeaveBalances(stoppingToken);
                await Task.Delay(_interval, stoppingToken);
            }
        }

        public async Task RunUpdateNowAsync()
        {
            _logger.LogInformation("Début de la mise à jour manuelle des soldes de congés.");
            await UpdateAllLeaveBalances(CancellationToken.None);
            _logger.LogInformation("Fin de la mise à jour manuelle des soldes de congés.");
        }

        private async Task UpdateAllLeaveBalances(CancellationToken stoppingToken)
        {
            var database = _mongoClient.GetDatabase("FlexiTime_DB");
            var userCollection = database.GetCollection<User>("Users");
            var leaveBalanceCollection = database.GetCollection<LeaveBalance>("LeaveBalances");

            var users = await userCollection.Find(_ => true).ToListAsync(stoppingToken);
            foreach (var user in users)
            {
                await UpdateLeaveBalanceForUserAsync(user, leaveBalanceCollection, stoppingToken);
            }
        }

        private async Task UpdateLeaveBalanceForUserAsync(User user, IMongoCollection<LeaveBalance> leaveBalanceCollection, CancellationToken stoppingToken)
        {
            var today = DateTime.UtcNow;
            var hireDate = user.HireDate;

            if (today.Day == hireDate.Day)
            {
                var leaveBalance = await leaveBalanceCollection.Find(lb => lb.UserId == user.Id).FirstOrDefaultAsync(stoppingToken)
                    ?? new LeaveBalance { UserId = user.Id, PaidLeaveBalance = 0 };

                leaveBalance.PaidLeaveBalance += user.IsPartTime ? 1.25 : 2.5;

                await leaveBalanceCollection.ReplaceOneAsync(lb => lb.UserId == user.Id, leaveBalance, new ReplaceOptions { IsUpsert = true }, stoppingToken);
            }
        }
    }
}