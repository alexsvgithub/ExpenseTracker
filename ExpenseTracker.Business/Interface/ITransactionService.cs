﻿using ExpenseTracker.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Core.DTOs;

namespace ExpenseTracker.Business.Interface
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(Transaction dto);
        Task<IEnumerable<Transaction>> GetUserTransactionsAsync();
        Task<bool> UpdateTransactionAsync(Transaction dto);
        Task<bool> DeleteTransactionAsync(string transactionId);
        Task<List<string>> GetAllCategories();
        Task<object> GetDashboardDataAsync(DashboardFilterDto filter);
    }
}
