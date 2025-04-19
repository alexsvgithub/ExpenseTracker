using ExpenseTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data.Interface
{
    public interface IUserRepository
    {
        Task<string> RegisterNewUser(User user);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByIdAsync(string id);
        Task<bool> UpdateAsync(User user);
    }
}
