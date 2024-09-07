using System.Net;

namespace FlexiTime_Backend.Domain.Exceptions
{
    /// <summary>
    /// Représente une exception spécifique au service dans l'application.
    /// Utilisée pour signaler des erreurs liées au fonctionnement du service avec des codes d'état HTTP.
    /// </summary>
    public class ServiceException : Exception
    {
        /// <summary>
        /// Obtient ou définit le code d'état interne de l'erreur.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Obtient ou définit le message d'erreur.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'état HTTP associé à l'erreur.
        /// </summary>
        public HttpStatusCode HttpStatus { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ServiceException"/> avec un code d'erreur interne,
        /// une description de l'erreur et un code d'état HTTP.
        /// </summary>
        /// <param name="internalErrorCode">Le code d'erreur interne représentant l'erreur.</param>
        /// <param name="errorDescription">La description de l'erreur.</param>
        /// <param name="httpStatus">Le code d'état HTTP associé à l'erreur.</param>
        public ServiceException(int internalErrorCode, string errorDescription, HttpStatusCode httpStatus)
        {
            StatusCode = internalErrorCode;
            ErrorMessage = errorDescription;
            HttpStatus = httpStatus;
        }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ServiceException"/> avec un code d'erreur interne
        /// et une description de l'erreur, sans code d'état HTTP.
        /// </summary>
        /// <param name="internalErrorCode">Le code d'erreur interne représentant l'erreur.</param>
        /// <param name="errorDescription">La description de l'erreur.</param>
        public ServiceException(int internalErrorCode, string errorDescription)
        {
            StatusCode = internalErrorCode;
            ErrorMessage = errorDescription;
        }
    }
}
