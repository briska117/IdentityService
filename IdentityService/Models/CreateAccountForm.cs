using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
{
    public class CreateAccountForm:CreateAccount
    {
        public string? Phone { get; set; }
        [Required]
        public Role Role { get; set; } = Role.Customer; 

    }

    
}
