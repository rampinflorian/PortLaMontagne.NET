using Microsoft.AspNetCore.Http;

namespace PortLaMontagne.Areas.User.Form.PersonalData
{
    public class InputForm
    {
        public AdministrativeForm AdministrativeForm { get; set; }
        public UserForm UserForm { get; set; }
    }
}