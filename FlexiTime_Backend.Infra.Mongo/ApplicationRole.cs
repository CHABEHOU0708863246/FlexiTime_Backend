using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDbGenericRepository.Attributes;

namespace FlexiTime_Backend.Infra.Mongo
{
    [CollectionName("roles")]
    public class ApplicationRole : MongoIdentityRole<ObjectId>
    {
        public string? Code { get; set; }
    }
}
