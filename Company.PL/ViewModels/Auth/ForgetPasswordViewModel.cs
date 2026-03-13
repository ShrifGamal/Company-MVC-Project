using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Auth
{
    public class ForgetPasswordViewModel
    {

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid EmailAddress")]
        public string Email { get; set; }
    }
}
