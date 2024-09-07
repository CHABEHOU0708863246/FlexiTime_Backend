using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Roles
{
    public class Role
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        [Required]
        public string? RoleName { get; set; }

        [Required]
        public string? Code { get; set; }
        [Required]

        public bool IsVisible { get; set; } = true;
    }
}
