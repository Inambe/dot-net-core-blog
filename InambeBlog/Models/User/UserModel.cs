using System;
using InambeBlog.Models.Post;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InambeBlog.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        
        public ICollection<PostModel> UserPosts { get; set; }
        
        [Required,MaxLength(50)]
        public string Name { get; set; }
        
        [Required,MaxLength(100)]
        public string Email { get; set; }
        
        [Required,MaxLength(50)]
        public string PasswordHash { get; set; }
        
        [Required,MaxLength(30)]
        public string PasswordSalt { get; set; }
        
        public bool IsActivated { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        public DateTime UpdatedOn { get; set; }
    }
}
