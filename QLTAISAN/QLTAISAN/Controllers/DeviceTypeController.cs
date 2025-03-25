using Microsoft.AspNetCore.Mvc;

namespace QLTAISAN.Controllers
{
    public class DeviceTypeController : Controller
    {
        private readonly IDeviceTypeService _deviceTypeService;

        public DeviceTypeController(IDeviceTypeService deviceTypeService)
        {
            _deviceTypeService = deviceTypeService;
        }

        public async Task<IActionResult> DeviceType()
        {
            var deviceTypes = await _deviceTypeService.GetAllDeviceTypesAsync();
            return View(deviceTypes);
        }

        [HttpPost]
        public async Task<IActionResult> AddDeviceType(string typeName, string typeSymbol, string notes)
        {
            await _deviceTypeService.AddDeviceTypeAsync(typeName, typeSymbol, notes);
            return RedirectToAction("DeviceType");
        }

        [HttpGet]
        public async Task<JsonResult> GetDetail(int id)
        {
            var deviceType = await _deviceTypeService.GetDeviceTypeByIdAsync(id);
            return Json(new { data = deviceType });
        }

        [HttpPost]
        public async Task<JsonResult> EditDeviceType(int id, string typeName, string typeSymbol, string notes)
        {
            var result = await _deviceTypeService.UpdateDeviceTypeAsync(id, typeName, typeSymbol, notes);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteDeviceType(int id)
        {
            var result = await _deviceTypeService.DeleteDeviceTypeAsync(id);
            return Json(result);
        }

        public async Task<IActionResult> StatisticalDeviceType()
        {
            var deviceTypes = await _deviceTypeService.GetAllDeviceTypesAsync();
            return View(deviceTypes);
        }
    }
}
