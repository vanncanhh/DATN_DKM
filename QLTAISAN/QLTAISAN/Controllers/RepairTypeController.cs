namespace QLTAISAN.Controllers
{
    public class RepairTypeController : Controller
    {
        private readonly IRepairTypeService _repairTypeService;

        public RepairTypeController(IRepairTypeService repairTypeService)
        {
            _repairTypeService = repairTypeService;
        }

        public async Task<IActionResult> RepairType()
        {
            var repairTypes = await _repairTypeService.GetAllRepairTypesAsync();
            ViewData["RepairTypes"] = repairTypes;
            return View(repairTypes);
        }

        [HttpGet]
        public async Task<JsonResult> GetDetail(int id)
        {
            var repairType = await _repairTypeService.GetRepairTypeByIdAsync(id);
            return Json(new { data = repairType });
        }

        public IActionResult AddRepairType()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRepairType(string typeName, string notes)
        {
            var result = await _repairTypeService.AddRepairTypeAsync(typeName, notes);
            return RedirectToAction("RepairType");
        }

        public async Task<IActionResult> EditRepairType(int id)
        {
            var repairType = await _repairTypeService.GetRepairTypeByIdAsync(id);
            return View(repairType);
        }

        [HttpPost]
        public async Task<JsonResult> EditRepairType(int id, string typeName, string notes)
        {
            var result = await _repairTypeService.EditRepairTypeAsync(id, typeName, notes);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRepairType(int id)
        {
            var result = await _repairTypeService.DeleteRepairTypeAsync(id);
            return Json(result);
        }
    }
}
