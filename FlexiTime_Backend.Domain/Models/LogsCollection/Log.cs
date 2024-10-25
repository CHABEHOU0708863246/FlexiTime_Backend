using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexiTime_Backend.Domain.Models.LogsCollection
{
    public class Log
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRequired]
        public string Action { get; set; } = string.Empty; // Type d'action (ex: "Demande approuvée", "Mot de passe réinitialisé")

        [BsonRequired]
        public string PerformedBy { get; set; } = string.Empty; // Id de l'utilisateur ayant effectué l'action

        [BsonRequired]
        public string AffectedUser { get; set; } = string.Empty; // Id de l'utilisateur concerné

        [BsonDateTimeOptions]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Date et heure de l'action

        public string? Description { get; set; } // Description optionnelle pour plus de détails sur l'action
    }
}
