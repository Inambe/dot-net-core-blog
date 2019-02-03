using System.ComponentModel.DataAnnotations;

namespace InambeBlog.Models.User.ViewModels
{
    public class SignInUserVM
    {
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Your Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Your Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public SignInViewState? State { get; set; }
        public bool RememberMe { get; set; }
    }
}
