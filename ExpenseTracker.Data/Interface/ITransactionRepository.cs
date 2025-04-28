using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Models;
using MongoDB.Driver;

namespace ExpenseTracker.Data.Interface
{
    public interface ITransactionRepository 
    {
        Task AddAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetByUserIdAsync(string userId);
        Task<bool> UpdateAsync(Transaction transaction);
        Task<bool> DeleteAsync(string transactionId);
        Task<List<string>> GetAllCategories(string userId);
    }
}
