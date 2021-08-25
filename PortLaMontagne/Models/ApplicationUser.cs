using System;
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
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public bool IsNewsletter { get; set; } = false;

        public DateTime CreatedAt { get; set; }

        public string FullName()
        {
            return FirstName + " " + LastName;
        }

    }
}