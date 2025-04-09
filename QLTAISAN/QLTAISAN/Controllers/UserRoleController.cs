namespace QLTAISAN.Controllers
{
    public class UserRoleController : Controller
    {
        QuanLyTaiSanCtyDATNContext data = new QuanLyTaiSanCtyDATNContext();

        //[HasCredential(RoleID = "VIEW_USER")]
        public ActionResult UserIndex()
        {
            var dao = new UserDao();
            var users = data.UserLogins.Where(x => x.IsDeleted == false).ToList();
            var userRole = new List<InformationUser>();
            foreach (var item in users)
            {
                //  var firstRoleId = data.UserLogins.Where(x => x.);
                //  var firstRoleId = data.UserLogins.FirstOrDefault()?.GroupID;
                if (!string.IsNullOrEmpty(item.GroupId))
                {
                    var NameGrRole = data.UserGroups.Where(x => x.Id == item.GroupId).Select(x => x.Name).First();
                    userRole.Add(new InformationUser()
                    {
                        Id = item.Id,
                        Username = item.UserName,
                        RoleGroupID = item.GroupId,
                        RoleGroupName = NameGrRole,
                        FullName = item.FullName,

                    });
                }
                else
                {
                    userRole.Add(new InformationUser()
                    {
                        Id = item.Id,
                        Username = item.UserName,
                        RoleGroupID = item.GroupId,
                        RoleGroupName = "Chưa được phân quyền",
                        FullName = item.FullName,

                    });
                }
            }
            ViewData["Users"] = userRole;
            ViewData["Roles"] = data.UserGroups.ToList();
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[HasCredential(RoleID = "ADD_USER")]
        public ActionResult RegisterUser(string FullName, string Role, string Username, string Password)
        {
            var dao = new UserDao();
            var result = dao.UpdateRoleUser(FullName, Username, Role, Encryptor.MD5Hash(Password));
            return Json(result);
        }
        [HttpGet]
        //[HasCredential(RoleID = "CHANGE_INFO_USER")]
        public JsonResult GetInfoAccount(string userId)
        {
            data.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            //  bool result = true;
            var u = userId.Trim();
            var lstInfo = data.UserLogins.Where(x => x.UserName == u).FirstOrDefault();
            return Json(new { lstInfo });
        }
        [HttpPost]
        //[HasCredential(RoleID = "CHANGE_USER_GROUP")]
        public JsonResult ChangeRoleByUserId(int ID, string FullName, string Username, string Role, string Password)
        {
            // bool result = true;
            var dao = new UserDao();
            var result = dao.UpdateRole(ID, FullName, Username, Role, Password);
            return Json(result);
        }
        [HttpPost]
        //[HasCredential(RoleID = "DELETE_USER")]
        public JsonResult DeleteUser(int userId)
        {
            var dao = new UserDao();
            var result = dao.DeleteRoleUser(userId);
            return Json(result);
        }
    }
}
