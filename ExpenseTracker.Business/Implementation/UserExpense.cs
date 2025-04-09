using ExpenseTracker.Business.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Implementation
{
    public class UserExpense : IUserExpense
    {

        public async Task<JObject> GetAllExpenseOfUser(string userId)
        {
            var returnvalue = @"{'test':'abc'}";
            await Task.Delay(1000);

            return JObject.Parse(returnvalue.ToString());

        }
    }

}
