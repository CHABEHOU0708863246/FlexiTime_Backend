using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexiTime_Backend.Domain.Models.Holiday
{
    public class PublicHoliday
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRequired]
        public string HolidayName { get; set; } = string.Empty;

        [BsonRequired]
        public DateTime HolidayDate { get; set; }

        public string CountryCode { get; set; } = string.Empty; // Si spécifique à une région
    }
}
