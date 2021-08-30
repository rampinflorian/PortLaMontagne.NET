using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PortLaMontagne.Models
{
    public class Partner
    {
        public int PartnerId { get; set; }
        [Display(Name = "Nom")]
        [Required]
        public string Name { get; set; }
        [Display(Name = "URL du site Web")]
        [Required]
        public string Website { get; set; }
        [Display(Name = "Image")]
        [Required]
        public string Image { get; set; }

        [NotMapped]
        [Required]
        public IFormFile FormFile { get; set; }
    }
}