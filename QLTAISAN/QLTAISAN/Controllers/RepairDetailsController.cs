namespace QLTAISAN.Controllers
{
    public class RepairDetailsController : Controller
    {
        private readonly IRepairDetailsService _repairDetailsService;

        public RepairDetailsController(IRepairDetailsService repairDetailsService)
        {
            _repairDetailsService = repairDetailsService;
        }

        public async Task<IActionResult> RepairDetails()
        {
            var repairDetails = await _repairDetailsService.GetAllRepairDetailsAsync();
            ViewData["RepairDetails"] = repairDetails;
            return View(repairDetails);
        }

        [HttpPost]
        public async Task<IActionResult> SearchRepairDetails(int? repairType, int? userId, int? deviceId, int? status)
        {
            var repairDetails = await _repairDetailsService.SearchRepairDetailsAsync(repairType, userId, deviceId, status);
            ViewBag.RepairType = repairType;
            ViewBag.UserId = userId;
            ViewBag.DeviceId = deviceId;
            ViewBag.Status = status;
            return View("RepairDetails", repairDetails);
        }

        public async Task<IActionResult> AddRepairDetails()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRepairDetails(RepairDetail repairDetail)
        {
            await _repairDetailsService.AddRepairDetailAsync(repairDetail);
            return RedirectToAction("RepairDetails");
        }

        public async Task<IActionResult> EditRepairDetails(int id)
        {
            var repairDetail = await _repairDetailsService.GetRepairDetailByIdAsync(id);
            return View(repairDetail);
        }

        [HttpPost]
        public async Task<IActionResult> EditRepairDetails(RepairDetail repairDetail)
        {
            await _repairDetailsService.UpdateRepairDetailAsync(repairDetail);
            return RedirectToAction("RepairDetails");
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRepairDetails(string id)
        {
            var result = await _repairDetailsService.DeleteRepairDetailsAsync(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRepairDetail(int id)
        {
            var result = await _repairDetailsService.DeleteRepairDetailAsync(id);
            return Json(result);
        }
    }
}
