namespace QLTAISAN.Controllers
{
    public class RoleController : Controller
    {
        QuanLyTaiSanCtyDATNContext data = new QuanLyTaiSanCtyDATNContext();
        // GET: Role

        //[HasCredential(RoleID = "VIEW_GROUP_ROLE")]
        public ActionResult RoleIndex()
        {
            ViewData["ListUserGroup"] = data.UserGroups.ToList();
            ViewData["ListRoleForGroup"] = data.Roles.ToList();
            return View();
        }

        [HttpPost]        
        //[HasCredential(RoleID = "ADD_GROUP_ROLE")]
        public ActionResult AddUserGroup(string ID, string Name)
        {
            var dao = new UserDao();
            bool result = dao.AddUserGroup(ID.Trim(), Name.Trim());
            return Json(result );
        }

        [HttpPost]
        //[HasCredential(RoleID = "DELETE_GROUP_ROLE")]
        public ActionResult ConfirmDelete(string ID)
        {
            var dao = new UserDao();
            bool result = dao.DeleteUserGroup(ID.Trim());
            return Json(result );
        }

        [HttpPost]
        //[HasCredential(RoleID = "ADD_ROLE_FOR_GROUP")]        
        public ActionResult GetRoleForeGroup(string GroupID)
        {
            //  var dao = new UserDao();
            var lstRole = data.Credentials.Where(x => x.UserGroupId == GroupID).Select(x => x.RoleId).ToList();
            // bool result = dao.DeleteUserGroup(RoleID.Trim());
            // bool Result = true; 
            var result = new { lstRole };
            return Json(result );
        }

        [HttpPost]        
        //[HasCredential(RoleID = "ADD_ROLE_FOR_GROUP")]
        public ActionResult AddRoleForGroup(string RoleId, string GroupId)
        {
            data.DeleteAllRole(GroupId);
            var dao = new UserDao();
            bool result = dao.AddRoleForGroup(RoleId, GroupId.Trim());
            return Json(result );
        }
    }
}
