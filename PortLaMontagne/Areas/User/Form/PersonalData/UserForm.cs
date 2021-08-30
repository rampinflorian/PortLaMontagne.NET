using System.ComponentModel.DataAnnotations;

namespace PortLaMontagne.Areas.User.Form.PersonalData
{
    public class UserForm
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Prompt = "Mot de passe actuel")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Prompt = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Prompt = "Confirmation nouveau mot de passe")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}