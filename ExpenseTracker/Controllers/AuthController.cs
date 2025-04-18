using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
