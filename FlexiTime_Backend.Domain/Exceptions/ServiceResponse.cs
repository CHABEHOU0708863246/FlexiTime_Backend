namespace FlexiTime_Backend.Domain.Exceptions
{
    /// <summary>
    /// Représente une réponse de service standardisée dans l'application.
    /// Utilisée pour encapsuler les réponses des services avec un code d'état, un message, et des données supplémentaires.
    /// </summary>
    public class ServiceResponse
    {
        /// <summary>
        /// Obtient ou définit le code d'état de la réponse.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Obtient ou définit le message de la réponse.
        /// Par défaut, il est initialisé à "success".
        /// </summary>
        public string Message { get; set; } = "success";

        /// <summary>
        /// Obtient ou définit les données supplémentaires de la réponse.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ServiceResponse"/> avec un code d'état, une description et des données.
        /// </summary>
        /// <param name="statusCode">Le code d'état de la réponse.</param>
        /// <param name="description">La description de la réponse.</param>
        /// <param name="data">Les données supplémentaires associées à la réponse.</param>
        public ServiceResponse(int statusCode, string description, object data)
        {
            StatusCode = statusCode;
            Message = description;
            Data = data;
        }
    }
}
