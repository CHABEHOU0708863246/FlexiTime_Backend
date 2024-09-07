using MongoDB.Driver;

namespace FlexiTime_Backend.Infra.Mongo.Models
{
    /// <summary>
    /// Interface pour les opérations de repository sur une entité MongoDB.
    /// Définit les méthodes standard pour ajouter, rechercher, mettre à jour, obtenir, supprimer, et compter des entités MongoDB.
    /// </summary>
    /// <typeparam name="T">Le type de l'entité MongoDB qui doit dériver de <see cref="MongoEntity"/>.</typeparam>
    public interface IRepository<T> where T : MongoEntity
    {
        /// <summary>
        /// Ajoute un nouvel objet à la collection MongoDB de manière asynchrone.
        /// </summary>
        /// <param name="obj">L'objet à ajouter.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        Task AddAsync(T obj, CancellationToken cancellationToken);

        /// <summary>
        /// Recherche tous les objets dans la collection MongoDB de manière asynchrone.
        /// </summary>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, avec une liste des objets trouvés.</returns>
        Task<IList<T>> FindAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Recherche des objets dans la collection MongoDB en fonction d'un filtre spécifié de manière asynchrone.
        /// </summary>
        /// <param name="filterDefinition">La définition du filtre pour la recherche.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, avec une liste des objets trouvés.</returns>
        Task<IList<T>> FindAsync(FilterDefinition<T> filterDefinition, CancellationToken cancellationToken);

        /// <summary>
        /// Met à jour un objet dans la collection MongoDB de manière asynchrone.
        /// </summary>
        /// <param name="obj">L'objet avec les modifications.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        Task UpdateAsync(T obj, CancellationToken cancellationToken);

        /// <summary>
        /// Obtient un objet par son identifiant de manière asynchrone.
        /// </summary>
        /// <param name="id">L'identifiant de l'objet.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, avec l'objet trouvé.</returns>
        Task<T> GetByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Supprime un objet par son identifiant de manière asynchrone.
        /// </summary>
        /// <param name="id">L'identifiant de l'objet à supprimer.</param>
        /// <param name="cancellationToken">Le jeton d'annulation pour la tâche asynchrone.</param>
        /// <returns>Une tâche représentant l'opération asynchrone.</returns>
        Task DeleteAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Compte le nombre d'objets correspondant à un filtre spécifié de manière asynchrone.
        /// </summary>
        /// <param name="filter">La définition du filtre pour le comptage.</param>
        /// <returns>Une tâche représentant l'opération asynchrone, avec le nombre d'objets trouvés.</returns>
        Task<long> CountAsync(FilterDefinition<T> filter);

        /// <summary>
        /// Obtient une requête IQueryable pour interroger la collection MongoDB.
        /// </summary>
        /// <returns>Un IQueryable permettant d'exécuter des requêtes LINQ sur la collection MongoDB.</returns>
        IQueryable<T> AsQueryable();
    }
}
