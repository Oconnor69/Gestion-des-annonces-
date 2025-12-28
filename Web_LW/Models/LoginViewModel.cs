using System.ComponentModel.DataAnnotations;

namespace Web_LW.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string MotDePasse { get; set; }
    }
}
