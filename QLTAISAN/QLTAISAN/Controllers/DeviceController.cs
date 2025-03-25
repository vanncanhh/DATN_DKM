namespace QLTAISAN.Controllers
{
    public class DeviceController : Controller
    {
        private readonly IDeviceService _deviceService;
        public DeviceController(IDeviceService deviceService)
        {
            _deviceService = deviceService;
        }

        // Hiển thị danh sách thiết bị
        public IActionResult Index()
        {
            var devices = _deviceService.getDevicesList().ToList();
            return View(devices);
        }

        // Hiển thị chi tiết thiết bị
        public IActionResult Details(int id)
        {
            var device = _deviceService.getDeviceById(id);
            if (device == null)
            {
                return NotFound();
            }
            return View(device);
        }

        // Hiển thị form thêm thiết bị mới
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý thêm thiết bị
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Device device)
        {
            if (ModelState.IsValid)
            {
                _deviceService.insertDevice(device);
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }

        // Hiển thị form chỉnh sửa thiết bị
        public IActionResult Edit(int id)
        {
            var device = _deviceService.getDeviceById(id);
            if (device == null)
            {
                return NotFound();
            }
            return View(device);
        }

        // Xử lý cập nhật thiết bị
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Device device)
        {
            if (id != device.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _deviceService.updateDevice(device);
                return RedirectToAction(nameof(Index));
            }
            return View(device);
        }

        // Hiển thị trang xác nhận xóa thiết bị
        public IActionResult Delete(int id)
        {
            var device = _deviceService.getDeviceById(id);
            if (device == null)
            {
                return NotFound();
            }
            return View(device);
        }

        // Xử lý xóa thiết bị
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _deviceService.deleteDevice(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
