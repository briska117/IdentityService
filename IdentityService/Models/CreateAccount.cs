using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class CreateAccount
    {
        [Required]
        [EmailAddress]
        [MinLength(5)]
        public string? Email { get; set; }
        [Required]
        [MinLength(8)]
        public string? Password { get; set; }    

    }
}
