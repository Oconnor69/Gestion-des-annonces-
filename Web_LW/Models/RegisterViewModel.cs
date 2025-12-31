using System.ComponentModel.DataAnnotations;

namespace Web_LW.Models
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(80, MinimumLength = 2)]
        public string Nom { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telephone")]
        public string? Telephone { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string MotDePasse { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("MotDePasse", ErrorMessage = "Les mots de passe ne correspondent pas.")]
        public string ConfirmationMotDePasse { get; set; }
    }
}
