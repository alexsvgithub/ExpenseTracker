using ExpenseTracker.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.Models;
using MongoDB.Driver;
using ExpenseTracker.Data.Interface;

namespace ExpenseTracker.Data.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly MongoDbContext _context;

        public TransactionRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.InsertOneAsync(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetByUserIdAsync(string userId)
        {
            return await _context.Transactions.Find(t => t.UserId == userId).ToListAsync();
        }

        public async Task<bool> UpdateAsync(Transaction transaction)
        {
            var result = await _context.Transactions.ReplaceOneAsync(t => t.Id == transaction.Id, transaction);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteAsync(string transactionId)
        {
            var result = await _context.Transactions.DeleteOneAsync(t => t.Id == transactionId);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }
    }
}
