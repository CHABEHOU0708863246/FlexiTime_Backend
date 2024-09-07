using FlexiTime_Backend.Infra.Mongo.Context;
using MongoDB.Driver;

namespace FlexiTime_Backend.Infra.Mongo.Models
{
    /// <summary>
    /// Implémentation du repository générique pour les opérations MongoDB.
    /// Implémente l'interface <see cref="IRepository{T}"/> pour les opérations CRUD sur les entités MongoDB.
    /// </summary>
    /// <typeparam name="T">Le type de l'entité MongoDB qui doit dériver de <see cref="MongoEntity"/>.</typeparam>
    public class MongoDbRepository<T> : IRepository<T> where T : MongoEntity
    {
        /// <summary>
        /// Collection MongoDB sur laquelle effectuer les opérations.
        /// </summary>
        private readonly IMongoCollection<T> _collection;

        /// <summary>
        /// Contexte MongoDB pour accéder à la base de données.
        /// </summary>
        private readonly IMongoDbContext _dbContext;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="MongoDbRepository{T}"/> avec le contexte MongoDB spécifié.
        /// </summary>
        /// <param name="dbContext">Le contexte MongoDB pour accéder à la base de données.</param>
        public MongoDbRepository(IMongoDbContext dbContext)
        {
            _dbContext = dbContext;
            // Récupère la collection MongoDB correspondant au type T.
            _collection = _dbContext.GetCollection<T>(typeof(T).Name);
        }

        /// <summary>
        /// Ajoute un nouvel objet à la collection MongoDB de manière asynchrone.
        /// </summary>
        /// <param name="obj">L'objet à ajouter.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public virtual async Task AddAsync(T obj, CancellationToken cancellationToken)
        {
            await _collection.InsertOneAsync(obj, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Met à jour un objet existant dans la collection MongoDB de manière asynchrone.
        /// </summary>
        /// <param name="obj">L'objet avec les modifications.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public virtual async Task UpdateAsync(T obj, CancellationToken cancellationToken)
        {
            await _collection.ReplaceOneAsync(x => x.Id == obj.Id, obj, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// Obtient un objet par son identifiant de manière asynchrone.
        /// </summary>
        /// <param name="id">L'identifiant de l'objet.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, avec l'objet trouvé.</returns>
        public virtual async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _collection.Find(Builders<T>.Filter.Eq(x => x.Id, id))
                .SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Supprime un objet par son identifiant de manière asynchrone.
        /// </summary>
        /// <param name="id">L'identifiant de l'objet à supprimer.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        public virtual async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            await _collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
        }

        /// <summary>
        /// Recherche tous les objets dans la collection MongoDB de manière asynchrone.
        /// </summary>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, avec une liste des objets trouvés.</returns>
        public virtual async Task<IList<T>> FindAsync(CancellationToken cancellationToken)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Compte le nombre d'objets correspondant à un filtre spécifié de manière asynchrone.
        /// </summary>
        /// <param name="filter">La définition du filtre pour le comptage.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, avec le nombre d'objets trouvés.</returns>
        public async Task<long> CountAsync(FilterDefinition<T> filter)
        {
            return await _collection.CountDocumentsAsync(filter);
        }

        /// <summary>
        /// Obtient une requête IQueryable pour interroger la collection MongoDB.
        /// </summary>
        /// <returns>Un IQueryable permettant d'exécuter des requêtes LINQ sur la collection MongoDB.</returns>
        public virtual IQueryable<T> AsQueryable()
        {
            return _collection.AsQueryable();
        }

        /// <summary>
        /// Recherche des objets dans la collection MongoDB en fonction d'un filtre spécifié de manière asynchrone.
        /// </summary>
        /// <param name="filterDefinition">La définition du filtre pour la recherche.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, avec une liste des objets trouvés.</returns>
        public async Task<IList<T>> FindAsync(FilterDefinition<T> filterDefinition, CancellationToken cancellationToken)
        {
            var results = await _collection.Find(filterDefinition).ToListAsync(cancellationToken);
            return results;
        }
    }
}
