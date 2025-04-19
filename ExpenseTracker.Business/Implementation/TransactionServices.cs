using ExpenseTracker.Business.Interface;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Data.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Transactions;

namespace ExpenseTracker.Business.Implementation
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transRepo;

        public TransactionService(ITransactionRepository transRepo)
        {
            _transRepo = transRepo;
        }

        public async Task AddTransactionAsync(Transaction dto)
        {
            var transaction = new Transaction
            {
                UserId = dto.UserId,
                Type = dto.Type,
                Amount = dto.Amount,
                Category = dto.Category,
                Note = dto.Note,
                TransactionDate = dto.TransactionDate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
             await _transRepo.AddAsync(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(string userId)
        {
            return await _transRepo.GetByUserIdAsync(userId);
        }

        public async Task<bool> UpdateTransactionAsync(Transaction dto)
        {
            var transaction = new Transaction
            {
                Id = dto.Id,
                Category = dto.Category,
                Note = dto.Note,
                Amount = dto.Amount,
                TransactionDate = dto.TransactionDate,
                UpdatedAt = DateTime.UtcNow
            };
            return await _transRepo.UpdateAsync(transaction);
        }

        public async Task<bool> DeleteTransactionAsync(string transactionId)
        {
            return await _transRepo.DeleteAsync(transactionId);
        }

        public async Task<object> GetDashboardDataAsync(DashboardFilterDto filter)
        {
            var all = await _transRepo.GetByUserIdAsync(filter.UserId);
            var now = DateTime.UtcNow;

            IEnumerable<Transaction> filtered = filter.FilterType switch
            {
                "daily" => all.Where(t => t.TransactionDate.Date == now.Date),
                "weekly" => all.Where(t => t.TransactionDate >= now.AddDays(-7)),
                "monthly" => all.Where(t => t.TransactionDate.Month == now.Month && t.TransactionDate.Year == now.Year),
                "yearly" => all.Where(t => t.TransactionDate.Year == now.Year),
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
