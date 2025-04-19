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
        Task<string> RegisterUser(RegisterDto user);
        Task<string> LoginAsync(LoginDto dto);
        Task<User> GetByIdAsync(string userId);
        Task<bool> UpdateProfileAsync(UpdateProfileDto dto);
    }
}
