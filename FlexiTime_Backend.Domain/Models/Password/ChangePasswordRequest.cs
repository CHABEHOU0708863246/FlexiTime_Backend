using System.ComponentModel.DataAnnotations;

namespace FlexiTime_Backend.Domain.Models.Password
{
    public class ChangePasswordRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "Le {0} doit comporter au moins {2} caractères.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}
