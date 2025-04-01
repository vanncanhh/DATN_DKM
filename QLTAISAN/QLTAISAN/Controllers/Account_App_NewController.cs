namespace QLTAISAN.Controllers
{
    public class Account_App_NewController : Controller
    {
        public ActionResult Login_New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login_New(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, model.Password);

                if (result == 1) // Kiểm tra đăng nhập thành công
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLoginSec
                    {
                        UserName = user.UserName,
                        UserID = user.Id,
                        GroupID = user.GroupId
                    };

                    // Lưu thông tin người dùng vào session
                    HttpContext.Session.SetString(CommonConstants.USER_SESSION, userSession.UserName);

                    // Lưu quyền truy cập vào session
                    List<string> privilegeLevelsNew = dao.GetListCredential(user.UserName.Trim());
                    HttpContext.Session.SetString(CommonConstants.SESSION_CREDENTIALS, string.Join(",", privilegeLevelsNew));

                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Tài khoản của bạn không có quyền truy cập.");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng nhập không đúng.");
                }
            }
            return View(model);
        }
    }
}
