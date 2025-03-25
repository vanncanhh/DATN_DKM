namespace QLTAISAN.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        public async Task<IActionResult> UserIndex()
        {
            var users = await _userRoleService.GetAllUsersAsync();
            ViewData["Users"] = users;
            return View(users);
        }

        [HttpPost]
        public async Task<JsonResult> RegisterUser(string fullName, string role, string username, string password)
        {
            var result = await _userRoleService.RegisterUserAsync(fullName, role, username, password);
            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetInfoAccount(string username)
        {
            var user = await _userRoleService.GetUserByUsernameAsync(username);
            return Json(new { user });
        }

        [HttpPost]
        public async Task<JsonResult> ChangeRoleByUserId(int id, string fullName, string username, string role, string password)
        {
            var result = await _userRoleService.ChangeRoleByUserIdAsync(id, fullName, username, role, password);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> DeleteUser(int userId)
        {
            var result = await _userRoleService.DeleteUserAsync(userId);
            return Json(result);
        }
    }
}
