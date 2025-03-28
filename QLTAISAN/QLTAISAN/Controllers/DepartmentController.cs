namespace QLTAISAN.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly QuanLyTaiSanCtyDATNContext Ql;
        public DepartmentController(IDepartmentService departmentService, QuanLyTaiSanCtyDATNContext ql)
        {
            _departmentService = departmentService;
            Ql = ql;
        }
        public ActionResult Department()
        {
            ViewBag.ProjectNb = Ql.SearchProject(null, null, 1, null)
                        .AsEnumerable()
                        .Count();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
            var lstProject = Ql.SearchProject(null, null, 1, null)
                                    .AsEnumerable()
                                    .ToList();
            return View(lstProject);
        }

        [HttpPost]
        public async Task<ActionResult> SearchDepartment(string projectSymbol, int? managerProject)
        {
            var departments = await _departmentService.SearchDepartmentAsync(projectSymbol, managerProject);
            ViewBag.ProjectNb = departments.Count;
            return View("Department", departments);
        }

        [HttpPost]
        public async Task<ActionResult> AddDepartment(ProjectDkc project)
        {
            var result = await _departmentService.AddDepartmentAsync(project);
            if (!result)
            {
                ViewBag.Tb = "Mã bị trùng";
                return View();
            }
            return RedirectToAction("Department");
        }

        [HttpPost]
        public async Task<ActionResult> EditDepartment(ProjectDkc project)
        {
            var result = await _departmentService.EditDepartmentAsync(project);
            if (!result)
            {
                ViewBag.Tb = "Không tìm thấy phòng ban";
                return View();
            }
            return RedirectToAction("Department");
        }

        [HttpPost]
        public async Task<JsonResult> DeleteDepartment(int id)
        {
            var result = await _departmentService.DeleteDepartmentAsync(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddDeviceToDepartment(int departmentId, int deviceId, string notes)
        {
            var result = await _departmentService.AddDeviceToDepartmentAsync(departmentId, deviceId, notes);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> RemoveDeviceFromDepartment(int departmentId, int deviceId, string notes)
        {
            var result = await _departmentService.RemoveDeviceFromDepartmentAsync(departmentId, deviceId, notes);
            return Json(result);
        }
    }
}
