using System.ComponentModel.DataAnnotations;

namespace InambeBlog.Models.User.ViewModels
{
    public class SignUpUserVM
    {
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Your Name")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Email is required")]
        [Display(Name = "Your  Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Create Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
