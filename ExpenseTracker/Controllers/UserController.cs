using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
