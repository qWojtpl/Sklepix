using Microsoft.AspNetCore.Mvc;

namespace Sklepix.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
