using Microsoft.AspNetCore.Mvc;

namespace AikaEmu.WebServer.Controllers
{
    public class NewsController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}