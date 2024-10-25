namespace FlexiTime_Backend.Domain.Models.LogsCollection
{
    public class LogResponse
    {
        public bool Succeeded { get; set; } // Indique si l'opération a réussi
        public IEnumerable<string> Errors { get; set; } = new List<string>(); // Liste d'erreurs en cas d'échec
        public Log? Log { get; set; } // Le log créé ou modifié
    }
}
