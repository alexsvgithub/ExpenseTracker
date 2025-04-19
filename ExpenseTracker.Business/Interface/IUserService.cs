using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Models;

namespace ExpenseTracker.Business.Interface
{
    public interface IUserService
    {
        Task<string> LoginAsync(User dto);
        Task<User> GetByIdAsync(string userId);
        Task<bool> UpdateProfileAsync(UpdateProfileDto dto);
    }
}
