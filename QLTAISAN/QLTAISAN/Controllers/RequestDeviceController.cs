namespace QLTAISAN.Controllers
{
    public class RequestDeviceController : Controller
    {
        QuanLyTaiSanCtyDATNContext Ql = new QuanLyTaiSanCtyDATNContext();

        //[HasCredential(RoleID = "VIEW_REQUEST_DEVICE")]
        public ActionResult RequestDevice()
        {
            ViewData["User"] = Ql.Users.ToList();
            ViewData["RequestDevices"] = Ql.RequestDevices.ToList();
            var x = Ql.RequestDevices.ToList();
            var lstRequestDevices = Ql.SearchRequestDeviceNew(null, null).AsEnumerable().ToList();
            return View(lstRequestDevices);
        }

        [HttpPost]
        public ActionResult SeachRequestDevices(FormCollection colection, RequestDevice RequestDevice)
        {
            ViewData["User"] = Ql.Users.ToList();
            ViewData["RequestDevices"] = Ql.RequestDevices.ToList();
            int? Status = colection["Status"].Equals("") ? (int?)null : Convert.ToInt32(colection["Status"]);
            var lstRequestDevices = Ql.SearchRequestDeviceNew(Status, false).AsEnumerable().ToList();
            var ViewRequestDevices = lstRequestDevices;
            ViewBag.Status = Status;
            return View("RequestDevice", ViewRequestDevices);
        }

        //[HasCredential(RoleID = "ADD_REQUEST_DEVICE")]
        public ActionResult AddRequestDevice()
        {
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            ViewData["DeviceTypes"] = Ql.DeviceTypes.ToList();
            return View();
        }

        [HttpPost]
        //[HasCredential(RoleID = "ADD_REQUEST_DEVICE")]
        public ActionResult AddRequestDevice(FormCollection colection, RequestDevice RequestDevice)
        {
            int? UserRequest = colection["UserRequest"].Equals("") ? (int?)null : Convert.ToInt32(colection["UserRequest"]);
            DateTime? DateOfRequest = colection["DateOfRequest"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfRequest"]);
            DateTime? DateOfUse = colection["DateOfUse"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfUse"]);
            String DeviceName = colection["DeviceName"];
            int? TypeOfDevice = colection["TypeOfDevice"].Equals("") ? (int?)null : Convert.ToInt32(colection["TypeOfDevice"]);
            String Configuration = colection["Configuration"];
            String Notes = colection["Notes"];
            int? Status = colection["Status"].Equals("-1") ? (int?)null : Convert.ToInt32(colection["Status"]);
            int? NumDevice = colection["NumDevice"].Equals("") ? (int?)null : Convert.ToInt32(colection["NumDevice"]);
            Ql.AddRequestDevice(UserRequest, DateOfRequest, DateOfUse, DeviceName, TypeOfDevice, Configuration, Notes, Status, NumDevice, null);
            return RedirectToAction("RequestDevice", "RequestDevice");
        }

        //[HasCredential(RoleID = "EDIT_REQUEST_DEVICE")]
        public ActionResult EditRequestDevice(int Id)
        {
            ViewData["DeviceTypes"] = Ql.DeviceTypes.ToList();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            return View(Ql.RequestDevices.Find(Id));
        }
        [HttpPost]
        //[HasCredential(RoleID = "EDIT_REQUEST_DEVICE")]
        public ActionResult EditRequestDevice(FormCollection colection, RequestDevice RequestDevice)
        {
            int? IdRequest = colection["IdRequest"].Equals("-1") ? (int?)null : Convert.ToInt32(colection["IdRequest"]);
            int? UserRequest = colection["UserRequest"].Equals("0") ? (int?)null : Convert.ToInt32(colection["UserRequest"]);
            DateTime? DateOfRequest = colection["DateOfRequest"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfRequest"]);
            DateTime? DateOfUse = colection["DateOfUse"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["DateOfUse"]);
            String DeviceName = colection["DeviceName"];
            int? TypeOfDevice = colection["TypeOfDevice"].Equals("0") ? (int?)null : Convert.ToInt32(colection["TypeOfDevice"]);
            String Configuration = colection["Configuration"];
            String Notes = colection["Notes"];
            int? Status = colection["Status"].Equals("") ? (int?)null : Convert.ToInt32(colection["Status"]);
            bool? Approved = Convert.ToBoolean(colection["Approved"]);
            int? NumDevice = colection["NumDevice"].Equals("") ? (int?)null : Convert.ToInt32(colection["NumDevice"]);
            String NoteProcess = colection["NoteProcess"];
            String NoteReasonRefuse = colection["NoteReasonRefuse"];
            String NameUserApproved = colection["NameUserApproved"];
            Ql.UpdateRequestDevice(IdRequest, UserRequest, DateOfRequest, DateOfUse, DeviceName, TypeOfDevice, Configuration, Notes, Approved, null, Status, NumDevice, NoteProcess, NoteReasonRefuse, NameUserApproved);
            ViewData["DeviceTypes"] = Ql.DeviceTypes.ToList();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            return View(Ql.RequestDevices.Find(IdRequest));

        }

        //[HasCredential(RoleID = "DELETE_REQUEST_DEVICE")]
        public JsonResult DeleteRequestDevice(string Id)
        {
            string a = "," + Id + ",";
            bool result = false;
            int checkdele = Ql.DeleteRequestDevice(a);
            if (checkdele > 0)
                result = true;
            return Json(result);
        }
        [HttpPost]
        //[HasCredential(RoleID = "ADD_DEVICE_TYPE")]
        public JsonResult AddDeviceType(string TypeName, string TypeSymbol, string Notes)
        {
            Ql.AddDeviceType(TypeName, TypeSymbol, Notes);
            bool result = true;
            return Json(result);
        }
    }
}
