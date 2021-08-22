using Microsoft.AspNetCore.Identity;

namespace PortLaMontagne.Models
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string Description { get; set; }
        [PersonalData]
        public string Image { get; set; }
        public string Dificulty { get; set; }
        
    }
}