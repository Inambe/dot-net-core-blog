using System.ComponentModel.DataAnnotations;

namespace InambeBlog.Models.Post.ViewModels
{
    public class EditPostVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int AuthorUserId { get; set; }
        
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100)]
        [Display(Name = "Post Title")]
        public string Title { get; set; }
        
        [Required(ErrorMessage = "Body is required")]
        [MaxLength(5000)]
        [Display(Name = "Post Body")]
        public string Body { get; set; }
    }
}
