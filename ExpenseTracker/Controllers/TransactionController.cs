using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
