using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.DTOs
{
    public class TransactionDto
    {
        public string UserId { get; set; }
        public string TransactionId { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } // expense or income
        public double Amount { get; set; }
        public string Category { get; set; }
        public string Note { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }
    }

    public class UpdateTransactionDto
    {
        public string TransactionId { get; set; }
        public string Category { get; set; }
        public string Note { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
