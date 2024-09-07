using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace FlexiTime_Backend.Infra.Mongo
{
    [CollectionName("users")]
    public class ApplicationUser : MongoIdentityUser<ObjectId>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpiryTime { get; set; }

    }
}
