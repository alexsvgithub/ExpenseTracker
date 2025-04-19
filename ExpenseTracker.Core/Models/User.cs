using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.Models
{
    public class User
    {
        public string _id { get; set; } = Guid.NewGuid().ToString();
        public string userName { get; set; }
        public string emailId { get; set; }
        public string passwordHash { get; set; }
        public string salt { get; set; }
        public string role { get; set; } = "CommonUser";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class RegisterDto
    {
        public string emailId { get; set; }
        public string password { get; set; }
        public string userName { get; set; }
    }

    public class LoginDto
    {
        public string EmailId { get; set; }
        public string Password { get; set; }
    }
}
