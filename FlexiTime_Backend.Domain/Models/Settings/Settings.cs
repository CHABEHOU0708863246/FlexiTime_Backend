using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FlexiTime_Backend.Domain.Models.Settings
{
    public class Setting
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int NombreJoursCongesAnnuel { get; set; } // Ex: 30 jours/an
        public double JoursAcquisitionParMois { get; set; } // Ex: 2.5 jours par mois
        public bool AutoriserCongesAnticipes { get; set; }

        public List<string> TypesConges { get; set; } // Ex: payé, non payé, maladie, etc.
    }
}
