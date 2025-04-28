using ExpenseTracker.Business.Interface;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Data.Interface;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
//using System.Transactions;

namespace ExpenseTracker.Business.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transRepo;
        private readonly IHttpContextAccessor _httpContext;
        private readonly String userid;

        public TransactionService(ITransactionRepository transRepo, IHttpContextAccessor httpContext)
        {
            _transRepo = transRepo;
            _httpContext = httpContext;
            userid =  _httpContext.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString(); 
        }

        public async Task AddTransactionAsync(Transaction dto)
        {
            
            var transaction = new Transaction
            {
                UserId = userid,
                Type = dto.Type,
                Amount = dto.Amount,
                Category = dto.Category,
                Note = dto.Note,
                TransactionDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
             await _transRepo.AddAsync(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync()
        {
            return await _transRepo.GetByUserIdAsync(userid);
        }

        public async Task<bool> UpdateTransactionAsync(Transaction dto)
        {
            var transaction = new Transaction
            {
                _id = dto._id,
                UserId = userid,
                Type = dto.Type,
                Category = dto.Category,
                Note = dto.Note,
                Amount = dto.Amount,
                UpdatedAt = DateTime.UtcNow
            };
            return await _transRepo.UpdateAsync(transaction);
        }

        public async Task<bool> DeleteTransactionAsync(string transactionId)
        {
            return await _transRepo.DeleteAsync(transactionId);
        }

        public async Task<List<string>> GetAllCategories()
        {
            return await _transRepo.GetAllCategories(userid);
        }

        public async Task<object> GetDashboardDataAsync(DashboardFilterDto filter)
        {
            var all = await _transRepo.GetByUserIdAsync(filter.UserId);
            var now = DateTime.UtcNow;

            IEnumerable<Transaction> filtered = filter.FilterType switch
            {
                "daily" => all.Where(t => t.TransactionDate?.Date == now.Date),
                "weekly" => all.Where(t => t.TransactionDate >= now.AddDays(-7)),
                "monthly" => all.Where(t => t.TransactionDate?.Month == now.Month && t.TransactionDate?.Year == now.Year),
                "yearly" => all.Where(t => t.TransactionDate?.Year == now.Year),
                _ => all
            };

            return new
            {
                TotalIncome = filtered.Where(t => t.Type == "income").Sum(t => t.Amount),
                TotalExpense = filtered.Where(t => t.Type == "expense").Sum(t => t.Amount),
                Transactions = filtered.OrderByDescending(t => t.TransactionDate)
            };
        }

        //Task ITransactionService.AddTransactionAsync(TransactionDto dto)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<bool> ITransactionService.UpdateTransactionAsync(UpdateTransactionDto dto)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
