using ExpenseTracker.Business.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserExpense : Controller
    {
        private readonly IUserExpense _userExpense;
        public UserExpense(IUserExpense userExpense) 
        {
            _userExpense = userExpense;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllExpenseOfUser(string userId)
        {
            try
            {
                var a = await _userExpense.GetAllExpenseOfUser(userId);

                return Ok(a);
            }catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest();
            }
        }
    }
}
