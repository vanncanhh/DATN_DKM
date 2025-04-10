namespace QLTAISAN.Controllers
{
    public class HomeController : Controller
    {
        QuanLyTaiSanCtyDATNContext data;

        public HomeController(QuanLyTaiSanCtyDATNContext _data)
        {
            data = _data;
        }
        public ActionResult Index()
        {
            var ListCount = new Dictionary<string, int>();
            int CountDevice = data.SearchDevice(null, null, null, null, null).AsEnumerable().Where(x => x.Status != 2).Count();
            ListCount.Add("CountDevice", CountDevice);
            int Deviceliquidation = data.SearchDevice(null, null, null, null, null).AsEnumerable().Where(x => x.Status == 2).Count();
            ListCount.Add("Deviceliquidation", Deviceliquidation);
            int DeviceType = data.DeviceTypes.Count();
            ListCount.Add("DeviceType", DeviceType);
            int Project = data.ProjectDkcs.Where(x => x.IsDeleted == false & x.TypeProject == 1).Count();
            ListCount.Add("Project", Project);
            int User = data.Users.Where(x => x.IsDeleted == false).Count();
            ListCount.Add("User", User);
            int RequestDevice = data.RequestDevices.Count();
            ListCount.Add("RequestDevice", RequestDevice);
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            return View(ListCount);
        }
        [Authorize(Roles = "Administrator")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        [Authorize(Roles = "StandardUser")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [AllowAnonymous]
        public ActionResult Unauthorized()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetData_PieChart()
        {
            var result_lst = data.GetData_PieChart();
            var _result = new { result = true, result_con = result_lst };
            return Json(_result);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetData_HorizontalChart()
        {
            var result_lst = data.GetData_HorizontalChart();
            var _result = new { result = true, result_con = result_lst };
            return Json(_result);
        }

        public IActionResult Error401()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }

            return View();
        }
    }
}
