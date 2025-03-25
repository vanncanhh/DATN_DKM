namespace QLTAISAN.Controllers
{
    public class RequestDeviceController : Controller
    {
        private readonly IRequestDeviceService _requestDeviceService;

        public RequestDeviceController(IRequestDeviceService requestDeviceService)
        {
            _requestDeviceService = requestDeviceService;
        }

        public async Task<IActionResult> RequestDevice()
        {
            var users = await _requestDeviceService.GetAllRequestDevicesAsync();
            ViewData["User"] = users;
            ViewData["RequestDevices"] = users;
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> SearchRequestDevices(int? status)
        {
            var requestDevices = await _requestDeviceService.SearchRequestDevicesAsync(status);
            ViewBag.Status = status;
            return View("RequestDevice", requestDevices);
        }

        public async Task<IActionResult> AddRequestDevice()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRequestDevice(RequestDevice requestDevice)
        {
            await _requestDeviceService.AddRequestDeviceAsync(requestDevice);
            return RedirectToAction("RequestDevice");
        }

        public async Task<IActionResult> EditRequestDevice(int id)
        {
            var requestDevice = await _requestDeviceService.GetRequestDeviceByIdAsync(id);
            return View(requestDevice);
        }

        [HttpPost]
        public async Task<IActionResult> EditRequestDevice(RequestDevice requestDevice)
        {
            await _requestDeviceService.UpdateRequestDeviceAsync(requestDevice);
            return RedirectToAction("RequestDevice");
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRequestDevice(string id)
        {
            var result = await _requestDeviceService.DeleteRequestDeviceAsync(id);
            return Json(new { success = result });
        }

        [HttpPost]
        public async Task<JsonResult> AddDeviceType(string typeName, string typeSymbol, string notes)
        {
            var result = await _requestDeviceService.AddDeviceTypeAsync(typeName, typeSymbol, notes);
            return Json(new { success = result });
        }
    }
}
