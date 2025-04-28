using ExpenseTracker.Business.Interface;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("add-expense")]
        [Authorize(Roles="CommonUser")]

        public async Task<IActionResult> AddTransaction(Transaction dto)
        {
            await _transactionService.AddTransactionAsync(dto);
            return Ok("Transaction added");
        }

        [HttpGet("GetAllTransactions")]
        [Authorize]
        public async Task<IActionResult> GetTransactions()
        {
            var transactions = await _transactionService.GetUserTransactionsAsync();
            return Ok(transactions);
        }

        [HttpPost("update-transaction")]
        public async Task<IActionResult> UpdateTransaction(Transaction dto)
        {
            var success = await _transactionService.UpdateTransactionAsync(dto);
            if (!success) return NotFound("Transaction not found");

            return Ok("Transaction updated");
        }

        [HttpDelete("delete-transaction/{id}")]
        public async Task<IActionResult> DeleteTransaction(string id)
        {
            var success = await _transactionService.DeleteTransactionAsync(id);
            if (!success) return NotFound("Transaction not found");

            return Ok("Transaction deleted");
        }

        [HttpPost("dashboard")]
        public async Task<IActionResult> GetDashboardData(DashboardFilterDto filter)
        {
            var data = await _transactionService.GetDashboardDataAsync(filter);
            return Ok(data);
        }
    }
}
