using Microsoft.Extensions.Configuration;

namespace FlexiTime_Backend.Infra.Mongo.Models
{
    /// <summary>
    /// Classe pour gérer la connexion à la base de données MongoDB en utilisant les paramètres de configuration.
    /// Implémente l'interface <see cref="IMongoDbConnection"/> pour fournir les informations de connexion MongoDB.
    /// </summary>
    public class MongoDBConnection : IMongoDbConnection
    {
        /// <summary>
        /// Instance de configuration pour accéder aux paramètres de configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MongoDBConnection"/> avec la configuration spécifiée.
        /// </summary>
        /// <param name="configuration">L'instance de configuration pour accéder aux paramètres de connexion.</param>
        public MongoDBConnection(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Obtient la chaîne de connexion MongoDB en appelant la méthode privée <see cref="DddMongoStore"/>.
        /// </summary>
        /// <returns>La chaîne de connexion MongoDB.</returns>
        public string MongoStore() => DddMongoStore();

        /// <summary>
        /// Récupère la chaîne de connexion MongoDB depuis les paramètres de configuration.
        /// </summary>
        /// <returns>La chaîne de connexion MongoDB.</returns>
        /// <exception cref="ArgumentNullException">Lève une exception si la configuration est nulle.</exception>
        private string DddMongoStore()
        {
            // Vérifie que la configuration n'est pas nulle
            if (_configuration == null)
            {
                throw new ArgumentNullException(nameof(_configuration));
            }

            // Retourne la chaîne de connexion MongoDB à partir des paramètres de configuration
            return _configuration["DatabaseSettings:ConnectionString"];
        }
    }
}
