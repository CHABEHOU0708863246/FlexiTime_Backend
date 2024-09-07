using MongoDB.Driver;

namespace FlexiTime_Backend.Infra.Mongo.Context
{
    /// <summary>
    /// Interface pour le contexte MongoDB.
    /// Fournit l'accès à la base de données MongoDB et aux collections spécifiques.
    /// </summary>
    public interface IMongoDbContext
    {
        /// <summary>
        /// Obtient l'instance de la base de données MongoDB.
        /// </summary>
        IMongoDatabase Database { get; }

        /// <summary>
        /// Obtient une collection spécifique de la base de données MongoDB.
        /// </summary>
        /// <typeparam name="TCollection">Le type des documents dans la collection.</typeparam>
        /// <param name="name">Le nom de la collection.</param>
        /// <returns>La collection MongoDB spécifiée.</returns>
        IMongoCollection<TCollection> GetCollection<TCollection>(string name);
    }
}
