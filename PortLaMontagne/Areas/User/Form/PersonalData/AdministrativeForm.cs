using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace PortLaMontagne.Areas.User.Form.PersonalData
{
    public class AdministrativeForm
    {
        [Display(Prompt = "Prénom")]
        public string FirstName { get; set; }
        [Display(Prompt = "Nom")]
        public string LastName { get; set; }

        [Display(Prompt = "Description")]
        public string Description { get; set; }
        [Display(Name = "Image")]

        public IFormFile FormFile { get; set; }
    }
}