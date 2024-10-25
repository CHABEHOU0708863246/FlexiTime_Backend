using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexiTime_Backend.Domain.Models.LeaveModel
{
    public class Leave
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        public string UserId { get; set; } // Référence vers la collection 'Users'

        [BsonRepresentation(BsonType.String)]
        public LeaveType Type { get; set; } = LeaveType.Paid; // Type de congé : 'Payé', 'Non payé', 'Maladie', etc.
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [BsonRepresentation(BsonType.String)]
        public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
        public string Comment { get; set; } = string.Empty;
        public string ApprovedBy { get; set; } // Référence au gestionnaire (UserId)
        public string Reason { get; set; } // Justification facultative de la demande
        public DateTime RequestedAt { get; set; } // Date de soumission de la demande
        public DateTime? ApprovedAt { get; set; } // Date d'approbation ou rejet

        // Validation pour s'assurer que la date de fin est après la date de début
        public bool IsValid()
        {
            return EndDate >= StartDate;
        }
    }

    public enum LeaveStatus
    {
        Pending,
        Approved,
        Rejected,
        Cancelled
    }

    public enum LeaveType
    {
        Paid,
        Unpaid,
        Sick,
        Parental,
        Other
    }
}
