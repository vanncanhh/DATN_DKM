namespace QLTAISAN.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // Trang hiển thị danh sách vai trò và nhóm người dùng
        public async Task<IActionResult> RoleIndex()
        {
            ViewData["ListUserGroup"] = await _roleService.GetAllUserGroupsAsync();
            ViewData["ListRoleForGroup"] = await _roleService.GetAllRolesAsync();
            return View();
        }

        // Thêm nhóm người dùng
        [HttpPost]
        public async Task<IActionResult> AddUserGroup(string id, string name)
        {
            var result = await _roleService.AddUserGroupAsync(id, name);
            return Json(result);
        }

        // Xóa nhóm người dùng
        [HttpPost]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var result = await _roleService.DeleteUserGroupAsync(id);
            return Json(result);
        }

        // Lấy vai trò cho nhóm người dùng
        [HttpPost]
        public async Task<IActionResult> GetRoleForGroup(string groupId)
        {
            var roles = await _roleService.GetRolesForGroupAsync(groupId);
            var result = new { lstRole = roles };
            return Json(result);
        }

        // Thêm vai trò cho nhóm người dùng
        [HttpPost]
        public async Task<IActionResult> AddRoleForGroup(string roleId, string groupId)
        {
            var result = await _roleService.AddRoleForGroupAsync(roleId, groupId);
            return Json(result);
        }
    }
}
