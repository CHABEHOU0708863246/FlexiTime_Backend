using FlexiTime_Backend.Infra.Mongo.Models;
using MongoDB.Driver;

namespace FlexiTime_Backend.Infra.Mongo.Context
{
    /// <summary>
    /// Implémentation du contexte MongoDB générique.
    /// Permet l'accès à la base de données MongoDB et aux collections spécifiques en utilisant une connexion générique.
    /// </summary>
    /// <typeparam name="T">Le type de la connexion MongoDB, qui doit implémenter <see cref="IMongoDbConnection"/>.</typeparam>
    public class MongoDbContext<T> : IMongoDbContext where T : IMongoDbConnection
    {
        /// <summary>
        /// Obtient ou définit le client MongoDB.
        /// </summary>
        private IMongoClient Client { get; set; }

        /// <summary>
        /// Obtient ou définit la base de données MongoDB.
        /// </summary>
        private IMongoDatabase _database { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MongoDbContext{T}"/> avec une connexion MongoDB spécifique.
        /// </summary>
        /// <param name="conn">La connexion MongoDB qui implémente <see cref="IMongoDbConnection"/>.</param>
        public MongoDbContext(T conn)
        {
            Create(conn);
        }

        /// <summary>
        /// Obtient l'instance de la base de données MongoDB.
        /// </summary>
        IMongoDatabase IMongoDbContext.Database => _database;

        /// <summary>
        /// Crée et configure la connexion MongoDB à partir des informations de connexion fournies.
        /// </summary>
        /// <param name="mongoConnection">L'instance de connexion MongoDB.</param>
        private void Create(IMongoDbConnection mongoConnection)
        {
            // Récupère la chaîne de connexion MongoDB à partir de l'instance de connexion
            var connectionString = mongoConnection.MongoStore();
            // Extrait le nom de la base de données à partir de l'URL de connexion
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            // Initialise le client MongoDB avec la chaîne de connexion
            Client = new MongoClient(connectionString);
            // Obtient la base de données spécifiée par le nom extrait
            _database = Client.GetDatabase(databaseName);
        }

        /// <summary>
        /// Obtient une collection spécifique de la base de données MongoDB.
        /// </summary>
        /// <typeparam name="TCollection">Le type des documents dans la collection.</typeparam>
        /// <param name="name">Le nom de la collection.</param>
        /// <returns>La collection MongoDB spécifiée.</returns>
        public IMongoCollection<TCollection> GetCollection<TCollection>(string name)
        {
            return _database.GetCollection<TCollection>(name);
        }
    }
}
