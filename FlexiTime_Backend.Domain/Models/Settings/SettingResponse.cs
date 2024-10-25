namespace FlexiTime_Backend.Domain.Models.Settings
{
    public class SettingResponse
    {
        public bool Succeeded { get; set; }  // Indique si l'opération a réussi ou non
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        // Contient les messages d'erreurs s'il y en a

        public Setting? Setting { get; set; }
        // Retourne les détails du setting après l'opération (ex. récupération ou création)
    }
}
