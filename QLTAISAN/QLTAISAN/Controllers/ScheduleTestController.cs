namespace QLTAISAN.Controllers
{
    public class ScheduleTestController : Controller
    {
        private readonly IScheduleTestService _scheduleTestService;
        QuanLyTaiSanCtyDATNContext Ql;

        public ScheduleTestController(IScheduleTestService scheduleTestService, QuanLyTaiSanCtyDATNContext ql)
        {
            _scheduleTestService = scheduleTestService;
            Ql = ql;
        }

        // Hiển thị danh sách
        public ActionResult ScheduleTest()
        {
            ViewData["User"] = Ql.Users.ToList();
            ViewData["ScheduleTests"] = Ql.ScheduleTests.ToList();
            var lstScheduleTests = Ql.GetSearchScheduleTest(null, null).ToList();
            return View(lstScheduleTests);
        }

        // Tìm kiếm
        [HttpPost]
        public async Task<IActionResult> SeachScheduleTest(int? userId, int? status)
        {
            var users = await _scheduleTestService.GetAllUsersAsync();
            var scheduleTests = await _scheduleTestService.SearchScheduleTestAsync(userId, status);
            ViewData["User"] = users;
            ViewData["ScheduleTests"] = scheduleTests;
            return View("ScheduleTest", scheduleTests);
        }

        // Trang thêm mới
        public async Task<IActionResult> AddScheduleTest()
        {
            ViewData["Devices"] = await _scheduleTestService.GetAllDevicesAsync();
            ViewData["User"] = await _scheduleTestService.GetAllUsersAsync();
            return View();
        }

        // Xử lý thêm mới
        [HttpPost]
        public async Task<IActionResult> AddScheduleTest(ScheduleTest scheduleTest)
        {
            await _scheduleTestService.AddScheduleTestAsync(scheduleTest);
            return RedirectToAction("ScheduleTest");
        }

        // Trang chỉnh sửa
        public async Task<IActionResult> EditScheduleTest(int id)
        {
            var scheduleTest = await _scheduleTestService.GetScheduleTestByIdAsync(id);
            if (scheduleTest == null) return NotFound();

            ViewData["Devices"] = await _scheduleTestService.GetAllDevicesAsync();
            ViewData["User"] = await _scheduleTestService.GetAllUsersAsync();
            return View(scheduleTest);
        }

        // Xử lý chỉnh sửa
        [HttpPost]
        public async Task<IActionResult> EditScheduleTest(ScheduleTest scheduleTest)
        {
            var result = await _scheduleTestService.UpdateScheduleTestAsync(scheduleTest);
            if (result)
            {
                return RedirectToAction("ScheduleTest");
            }
            return View(scheduleTest);
        }

        // Xóa lịch kiểm tra
        public async Task<JsonResult> DeleteScheduleTest(int id)
        {
            var result = await _scheduleTestService.DeleteScheduleTestAsync(id);
            return Json(new { success = result });
        }
    }
}
