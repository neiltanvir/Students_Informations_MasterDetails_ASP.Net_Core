using Microsoft.AspNetCore.Mvc;

namespace Students_Information.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
