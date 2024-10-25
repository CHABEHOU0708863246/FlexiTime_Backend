using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexiTime_Backend.Domain.Models.Reports
{
    public class Report
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRequired]
        public ReportType ReportType { get; set; } // Type de rapport

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow; // Date de génération du rapport

        [BsonRequired]
        public string GeneratedBy { get; set; } = string.Empty; // Id de l'utilisateur générant le rapport

        [BsonRequired]
        public List<string> Data { get; set; } = new List<string>(); // Contenu du rapport

        // Audit fields
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }

    public enum ReportType
    {
        Absences,
        ApprovedLeaves,
        PendingLeaves,
        UserActivity,
        CustomReport
    }
}
