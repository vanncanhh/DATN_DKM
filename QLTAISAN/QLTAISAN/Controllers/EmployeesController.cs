using Microsoft.AspNetCore.Mvc;

namespace QLTAISAN.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> UserManagement()
        {
            var users = await _employeeService.GetAllUsersAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> SearchUser(int status)
        {
            var users = await _employeeService.SearchUsersAsync(status);
            ViewBag.Status = status;
            return View("UserManagement", users);
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User user)
        {
            await _employeeService.AddUserAsync(user);
            return RedirectToAction("UserManagement");
        }

        public async Task<IActionResult> DetailUser(int id)
        {
            var user = await _employeeService.GetUserByIdAsync(id);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DetailUser(User user)
        {
            await _employeeService.UpdateUserAsync(user);
            return RedirectToAction("UserManagement");
        }

        [HttpPost]
        public async Task<JsonResult> DeleteUser(int id)
        {
            var result = await _employeeService.DeleteUserAsync(id);
            return Json(result);
        }

        public async Task<IActionResult> Role()
        {
            var roles = await _employeeService.GetAllRolesAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName, string notes)
        {
            await _employeeService.AddRoleAsync(roleName, notes);
            return RedirectToAction("Role");
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(int id, string roleName, string notes)
        {
            var result = await _employeeService.UpdateRoleAsync(id, roleName, notes);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteRole(int id)
        {
            var result = await _employeeService.DeleteRoleAsync(id);
            return Json(result);
        }
    }
}
