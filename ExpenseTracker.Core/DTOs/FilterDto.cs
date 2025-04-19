using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Core.DTOs
{
    public class DashboardFilterDto
    {
        public string UserId { get; set; }
        public string FilterType { get; set; } // daily, weekly, monthly, yearly
    }
}
