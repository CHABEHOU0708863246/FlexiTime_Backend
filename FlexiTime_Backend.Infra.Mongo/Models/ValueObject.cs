namespace FlexiTime_Backend.Infra.Mongo.Models
{
    /// <summary>
    /// Classe abstraite pour implémenter un objet valeur (Value Object) dans un modèle de domaine.
    /// Les objets valeur sont des objets définis par leurs attributs et non par leur identité.
    /// </summary>
    /// <typeparam name="T">Le type spécifique qui dérive de <see cref="ValueObject{T}"/>.</typeparam>
    public abstract class ValueObject<T> where T : ValueObject<T>
    {
        /// <summary>
        /// Compare l'objet actuel avec un autre objet pour vérifier leur égalité.
        /// </summary>
        /// <param name="obj">L'objet à comparer avec l'objet actuel.</param>
        /// <returns>Vrai si les objets sont égaux, sinon faux.</returns>
        public override bool Equals(object obj)
        {
            var valueObject = obj as T;
            return EqualsCore(valueObject);
        }

        /// <summary>
        /// Méthode protégée abstraite pour comparer l'objet actuel avec un autre objet valeur spécifique.
        /// Les classes dérivées doivent implémenter cette méthode pour définir la logique d'égalité.
        /// </summary>
        /// <param name="other">L'autre objet valeur avec lequel comparer.</param>
        /// <returns>Vrai si les objets sont égaux, sinon faux.</returns>
        protected abstract bool EqualsCore(T other);

        /// <summary>
        /// Obtient le code de hachage de l'objet actuel.
        /// </summary>
        /// <returns>Le code de hachage de l'objet actuel.</returns>
        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }

        /// <summary>
        /// Méthode protégée abstraite pour obtenir le code de hachage de l'objet.
        /// Les classes dérivées doivent implémenter cette méthode pour définir la logique de calcul du code de hachage.
        /// </summary>
        /// <returns>Le code de hachage de l'objet.</returns>
        protected abstract int GetHashCodeCore();

        /// <summary>
        /// Opérateur de comparaison d'égalité pour les objets valeur.
        /// </summary>
        /// <param name="a">Le premier objet valeur.</param>
        /// <param name="b">Le deuxième objet valeur.</param>
        /// <returns>Vrai si les objets sont égaux, sinon faux.</returns>
        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        /// <summary>
        /// Opérateur de comparaison de non-égalité pour les objets valeur.
        /// </summary>
        /// <param name="a">Le premier objet valeur.</param>
        /// <param name="b">Le deuxième objet valeur.</param>
        /// <returns>Vrai si les objets ne sont pas égaux, sinon faux.</returns>
        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !(a == b);
        }
    }
}
