namespace QLTAISAN.Controllers
{
    public class EmployeesController : Controller
    {
        QuanLyTaiSanCtyDATNContext data;

        public EmployeesController(QuanLyTaiSanCtyDATNContext _data)
        {
            data = _data;
        }
        public ActionResult UserManagement()
        {
            var lstUser = data.SearchUser(0).AsEnumerable().ToList();
            return View(lstUser);
        }
        [HttpPost]
        public ActionResult SearchUser(FormCollection collection)
        {
            int Status = Convert.ToInt32(collection["Status"]);
            ViewBag.status = Status;
            var charts = data.SearchUser(Status).AsEnumerable().ToList();
            var model = charts.ToList();
            return View("UserManagement", model);
        }

        public ActionResult CreateUser()
        {
            ViewData["Department"] = data.ProjectDkcs.Where(x => x.Status != 2 && x.IsDeleted == false).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(IFormCollection collection)
        {
            string UserName = collection["UserName"];
            string FullName = collection["FullName"];
            string Email = collection["Email"];
            string PhoneNumber = collection["PhoneNumber"];
            string Address = collection["Address"];
            string Department = collection["Department"];
            string Position = collection["Position"];
            if (string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(FullName) || string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError("", "Các trường bắt buộc không được để trống.");
                ViewData["Department"] = data.ProjectDkcs.Where(x => x.Status != 2 && x.IsDeleted == false).ToList();
                return View();
            }
            string hashedPassword = Encryptor.MD5Hash("12345");
            data.AddUser(UserName, hashedPassword, FullName, Email, PhoneNumber, Address, Department, Position, 0 , 0);
            return RedirectToAction("UserManagement", "Employees");
        }

        public ActionResult DetailUser(int Id)
        {
            ViewData["Department"] = data.ProjectDkcs.Where(x => x.Status != 2 && x.IsDeleted == false).ToList();
            var ID = data.Users.Where(x => x.Id == Id).Select(x => x.Department).FirstOrDefault();
            int ID1 = Convert.ToInt32(ID);
            ViewBag.ID = ID1;

            return View(data.Users.Find(Id));
        }
        [HttpPost]
        public ActionResult DetailUser(FormCollection collection)
        {
            int Id = Convert.ToInt32(collection["Id"]);
            string UserName = collection["UserName"];
            string FullName = collection["FullName"];
            string Email = collection["Email"];
            string PhoneNumber = collection["PhoneNumber"];
            string Address = collection["Address"];
            string Department = collection["Department"];
            string Position = collection["Position"];
            int Status = Convert.ToInt32(collection["Status"]);
            data.UpdateUser(Id, UserName, null, FullName, Email, PhoneNumber, Address, Department, Position, null, Status);
            return RedirectToAction("UserManagement", "Employees");
        }
        public JsonResult DeleteUser(int Id)
        {
            bool result = false;
            var charts = data.SearchDevice(null, null, null, null, null).AsEnumerable().Where(x => x.UserId == Id).ToList().Count();
            charts += data.SearchProject(Id, null, 0, null).AsEnumerable().ToList().Count();
            charts += data.SearchRepairDetails(null, Id, null, null).AsEnumerable().ToList().Count();
            charts += data.RequestDevices.Where(x => x.UserApproved == Id).ToList().Count();
            charts += data.RequestDevices.Where(x => x.UserRequest == Id).ToList().Count();
            charts += data.ScheduleTests.Where(x => x.UserTest == Id).ToList().Count();
            if (charts == 0)
            {
                data.DeleteUser(Id);
                result = true;
            }
            else result = false;
            return Json(result);
        }

        public ActionResult Role()
        {
            ViewData["Role"] = data.Roles.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AddRole(FormCollection collection)
        {
            string RoleName = collection["RoleName"];
            string Notes = collection["Notes"];
            data.AddRole(RoleName, Notes);
            return RedirectToAction("Role", "Employees");
        }
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            data.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var Role = data.Roles.Find(id);
            return Json(new
            {
                data = Role,
            });
        }
        [HttpPost]
        public JsonResult EditRole(int Id, string RoleName, string Notes)
        {
            bool result = true;
            data.UpdateRole(Id, RoleName, Notes);
            return Json(result);
        }
        public JsonResult DeleteRole(int Id)
        {
            bool result = false;
            int checkdele = data.DeleteRole(Id);
            if (checkdele > 0)
                result = true;
            return Json(result);
        }
    }
}
