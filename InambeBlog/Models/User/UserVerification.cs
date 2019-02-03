using System;

namespace InambeBlog.Models.User
{
    public class UserVerification
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        public string Hash { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
