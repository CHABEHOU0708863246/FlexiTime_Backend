using FlexiTime_Backend.Infra.Mongo;

namespace FlexiTime_Backend.Domain.Models.Users
{
    public class UserResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; } = new List<string>();
        public ApplicationUser? User { get; set; }

        // Méthode utilitaire pour ajouter une erreur
        public void AddError(string error)
        {
            Errors = Errors.Append(error);
        }
    }


}
