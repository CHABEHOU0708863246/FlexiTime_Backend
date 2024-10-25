using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexiTime_Backend.Domain.Models.LeaveBalanceModel
{
    public class LeaveBalance
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRequired]
        public string UserId { get; set; } = string.Empty; // Référence à l'utilisateur

        public double PaidLeaveBalance { get; set; } // Solde de congés payés
        public double UnpaidLeaveBalance { get; set; } // Solde de congés non payés
        public double SickLeaveBalance { get; set; } // Solde de congés maladie

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
