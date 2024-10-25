using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexiTime_Backend.Domain.Models.Notifications
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; } // Référence vers l'utilisateur concerné

        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
