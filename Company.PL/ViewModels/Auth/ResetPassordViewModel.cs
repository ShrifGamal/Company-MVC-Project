using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels.Auth
{
    public class ResetPassordViewModel
    {
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword Is Required")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Confirmed Password dose not Match Password")]
        public string ConfirmPassword { get; set; }
    }
}
