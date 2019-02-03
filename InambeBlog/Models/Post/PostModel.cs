using System;
using InambeBlog.Models.User;
using System.ComponentModel.DataAnnotations;

namespace InambeBlog.Models.Post
{
    public class PostModel
    {
        public int Id { get; set; }
        
        [Required]
        public UserModel AuthorUser { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        [Required]
        public string Body { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}
