using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Departments
{
    public class DepartmentRequest
    {
        [Required(ErrorMessage = "Le nom du département est obligatoire.")]
        [StringLength(100, ErrorMessage = "Le nom du département ne doit pas dépasser 100 caractères.")]
        public string DepartmentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'identifiant du gestionnaire est obligatoire.")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ManagerId { get; set; } = string.Empty; // Assure-toi que c'est un ObjectId valide
    }
}
