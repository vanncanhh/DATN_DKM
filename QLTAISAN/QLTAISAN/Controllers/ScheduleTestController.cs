namespace QLTAISAN.Controllers
{
    public class ScheduleTestController : Controller
    {
        private readonly IScheduleTestService _scheduleTestService;

        public ScheduleTestController(IScheduleTestService scheduleTestService)
        {
            _scheduleTestService = scheduleTestService;
        }

        // Hiển thị danh sách
        public async Task<IActionResult> ScheduleTest()
        {
            var users = await _scheduleTestService.GetAllUsersAsync();
            var scheduleTests = await _scheduleTestService.GetAllScheduleTestsAsync();
            ViewData["User"] = users;
            ViewData["ScheduleTests"] = scheduleTests;
            return View(scheduleTests);
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
