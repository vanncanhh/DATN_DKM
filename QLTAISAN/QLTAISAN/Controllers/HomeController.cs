using Microsoft.AspNetCore.Authorization;

namespace QLTAISAN.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var statistics = await _homeService.GetStatisticsAsync();
            ViewData["Statistics"] = statistics;
            return View(statistics);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [Authorize(Roles = "StandardUser")]
        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [AllowAnonymous]
        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}
