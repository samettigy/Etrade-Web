using Microsoft.AspNetCore.Mvc;

namespace Etrade.Core.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (!User.IsInRole("Admin"))
                return Redirect("~/Home/Index");
            return View();
        }
    }
}
