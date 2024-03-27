using Microsoft.AspNetCore.Mvc;

namespace MetroTracker.Controllers
{
    public class MapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
