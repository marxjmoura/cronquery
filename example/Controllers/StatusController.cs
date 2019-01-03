using Microsoft.AspNetCore.Mvc;

namespace example.Controllers
{
    public class StatusController : Controller
    {
        [HttpGet, Route("/")]
        public IActionResult Index()
        {
            return Content("Jobs are running...");
        }
    }
}
