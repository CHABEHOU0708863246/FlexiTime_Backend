using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexiTime_Backend.Domain.Models.Departments
{
    public class Department
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty; // Initialiser avec une valeur par défaut pour éviter les exceptions

        [BsonRequired]
        public string DepartmentName { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string ManagerId { get; set; } = string.Empty; // Référence au gestionnaire de l'équipe
    }
}
