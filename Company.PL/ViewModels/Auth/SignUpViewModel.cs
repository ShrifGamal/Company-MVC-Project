using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Auth
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "UserName Is Required")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "FristName Is Required")]
        public string FristName { get; set; }
        [Required(ErrorMessage = "LastName Is Required")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage ="Invalid EmailAddress")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword Is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password) , ErrorMessage ="Confirmed Password dose not Match Password")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }

    }
}
