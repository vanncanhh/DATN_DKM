using QRCoder;
using System.Collections.Generic;
using System.Drawing.Imaging;
namespace QLTAISAN.Controllers
{
    public class DeviceController : Controller
    {
        QuanLyTaiSanCtyDATNContext data;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DeviceController(QuanLyTaiSanCtyDATNContext _data, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            data = _data;
        }
        [HasCredential(RoleID = "VIEW_DEVICE")]
        public ActionResult Device()
        {
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.TypeProject == 1 & x.IsDeleted == false).ToList();
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).AsEnumerable().ToList();
            var lstDevice = data.SearchDevice(null, null, null, null, null)
                     .AsEnumerable()
                     .Where(x => x.Status != 2)
                     .ToList();
            ViewBag.CountDevice = data.SearchDevice(null, null, null, null, null)
                            .AsEnumerable()
                            .Where(x => x.Status != 2)
                            .Count();
            return View(lstDevice);
        }
        [HasCredential(RoleID = "SCAN_DEVICE")]
        public ActionResult ScanerDevice()
        {
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.Status != 3 & x.TypeProject == 1 & x.IsDeleted == false).ToList();
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).AsEnumerable().ToList();
            return View();
        }
        [HttpGet]
        [HasCredential(RoleID = "SCAN_DEVICE")]
        public JsonResult GetDeviceScaner(string DeviceCode)
        {
            bool Result = false;
            if (data.SearchDevice(null, null, null, null, null).AsEnumerable().Where(x => x.DeviceCode.Trim() == DeviceCode).Count() == 1)
            {
                ViewBag.Scaner = data.SearchDevice(null, null, null, null, null).AsEnumerable().Where(x => x.DeviceCode.Trim() == DeviceCode).First();
                Result = true;
            }
            else Result = false;
            var model = ViewBag.Scaner;
            var result = new { Result, model };
            return Json(result);
        }
        [HttpPost]
        public ActionResult SearchDevice(IFormCollection collection)
        {
            var d = data.DeviceTypes.ToList();
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.Status != 3 && x.TypeProject == 1 && x.IsDeleted == false).ToList();
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).AsEnumerable().ToList();

            // Lấy dữ liệu từ form sử dụng IFormCollection
            int Status = Convert.ToInt32(collection["Status"]);
            int? TypeOfDevice = collection["TypeOfDevice"].Equals("0") ? (int?)null : Convert.ToInt32(collection["TypeOfDevice"]);
            int Guarantee = Convert.ToInt32(collection["Guarantee"]);
            int? Project = collection["ProjectDKC"].Equals("0") ? (int?)null : Convert.ToInt32(collection["ProjectDKC"]);
            string DeviceCode = collection["DeviceCode"];

            // Tìm kiếm thiết bị theo các thông số đã nhập
            var charts = data.SearchDevice(Status, TypeOfDevice, Guarantee, Project, DeviceCode.Trim())
                             .AsEnumerable()
                             .Where(x => x.Status != 2)
                             .ToList();

            // Đếm số lượng thiết bị
            ViewBag.CountDevice = data.SearchDevice(Status, TypeOfDevice, Guarantee, Project, DeviceCode.Trim())
                                      .AsEnumerable()
                                      .Where(x => x.Status != 2)
                                      .Count();

            // Lưu trữ thông tin tìm kiếm vào ViewBag để truyền qua View
            ViewBag.status = Status;
            ViewBag.deviceCode = DeviceCode;
            ViewBag.type = TypeOfDevice;
            ViewBag.guarantee = Guarantee;
            ViewBag.poject = Project;

            // Trả về kết quả tìm kiếm qua View
            var model = charts.ToList();
            return View("Device", model);
        }
        //  [AuthorizationViewHandler]
        public ActionResult Deviceliquidation()
        {
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 0, null).AsEnumerable().ToList();
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.Status != 3 & x.IsDeleted == false).ToList();
            var lstDevice = data.SearchDevice(null, null, null, null, null)
                              .AsEnumerable()
                              .Where(x => x.Status == 2)
                              .ToList();
            return View(lstDevice);
        }
        [HasCredential(RoleID = "VIEW_DEVICE")]
        public ActionResult TypeDevice(int Id)
        {
            ViewBag.type = Id;
            var deviceType = data.DeviceTypes.SingleOrDefault(x => x.Id == Id);
            ViewBag.Title = deviceType != null ? deviceType.TypeName : "Unknown";

            ViewData["TypeOfDevice"] = data.DeviceTypes.Where(x => x.Id == Id).ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.IsDeleted == false && x.TypeProject == 1).ToList();
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).AsEnumerable().ToList();

            ViewBag.CountDevice = data.SearchDevice(null, Id, null, null, null)
                                      .AsEnumerable()
                                      .Where(x => x.Status != 2)
                                      .Count();

            var lstDevice = data.SearchDevice(null, Id, null, null, null)
                               .AsEnumerable()
                               .Where(x => x.Status != 2)
                               .ToList();

            return View(lstDevice);
        }


        [HttpPost]
        public ActionResult SearchTypeDevice(IFormCollection collection)
        {
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.TypeProject == 1 && x.IsDeleted == false).ToList();
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).AsEnumerable().ToList();

            int Status = Convert.ToInt32(collection["Status"]);
            int? TypeOfDevice = collection["TypeOfDevice"].Equals("0") ? (int?)null : Convert.ToInt32(collection["TypeOfDevice"]);
            int Guarantee = Convert.ToInt32(collection["Guarantee"]);
            int? Project = collection["ProjectDKC"].Equals("0") ? (int?)null : Convert.ToInt32(collection["ProjectDKC"]);
            string DeviceCode = collection["DeviceCode"];

            var charts = data.SearchDevice(Status, TypeOfDevice, Guarantee, Project, DeviceCode.Trim())
                             .AsEnumerable()
                             .Where(x => x.Status != 2)
                             .ToList();

            ViewBag.CountDevice = data.SearchDevice(Status, TypeOfDevice, Guarantee, Project, DeviceCode.Trim())
                                      .AsEnumerable()
                                      .Where(x => x.Status != 2)
                                      .Count();

            ViewBag.status = Status;
            ViewBag.deviceCode = DeviceCode;
            ViewBag.type = TypeOfDevice;
            ViewBag.guarantee = Guarantee;
            ViewBag.poject = Project;

            if (TypeOfDevice.HasValue)
            {
                var deviceType = data.DeviceTypes.SingleOrDefault(x => x.Id == TypeOfDevice);
                if (deviceType != null)
                {
                    ViewBag.Title = deviceType.TypeName;
                    ViewData["TypeOfDevice"] = new List<DeviceType> { deviceType };
                }
                else
                {
                    ViewBag.Title = "Unknown";
                    ViewData["TypeOfDevice"] = new List<DeviceType>();
                }
            }
            else
            {
                ViewBag.Title = "";
                ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            }

            return View("TypeDevice", charts);
        }

        [HasCredential(RoleID = "ADD_DEVICE")]
        public ActionResult Create(int Id)
        {
            ViewBag.TypeDevice = Id;
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["User"] = data.Users.Where(x => x.IsDeleted == false & x.Status == 0).ToList();
            ViewData["Supplier"] = data.Suppliers.ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.Status == 1 & x.TypeProject == 1 & x.IsDeleted == false).ToList();
            ViewData["Device"] = data.Devices.Where(x => x.IsDeleted == false).ToList();
            return View();
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_DEVICE")]
        public ActionResult Create(IFormCollection collection)
        {
            // Helper: chuyển đổi giá trị sang int? (trả về null nếu rỗng hoặc bằng "0")
            int? GetNullableInt(string key)
            {
                var value = collection[key];
                if (string.IsNullOrWhiteSpace(value) || value == "0")
                    return null;
                return Convert.ToInt32(value);
            }

            // Helper: chuyển đổi giá trị sang DateTime? (trả về null nếu rỗng)
            DateTime? GetNullableDateTime(string key)
            {
                var value = collection[key];
                if (string.IsNullOrWhiteSpace(value))
                    return null;
                return Convert.ToDateTime(value);
            }

            // Lấy giá trị từ form
            int? typeOfDevice = GetNullableInt("TypeOfDevice");
            string deviceCode = collection["DeviceCode"];
            // Nếu NewCode được sử dụng ở nơi khác thì có thể lấy giá trị, hiện tại chưa sử dụng
            string newCode = collection["NewCode"];
            string deviceName = collection["DeviceName"];
            double price = string.IsNullOrWhiteSpace(collection["Price"]) ? 0 : Convert.ToDouble(collection["Price"]);
            string configuration = collection["Configuration"];
            string notes = collection["Notesdv"];
            int? supplierId = GetNullableInt("SupplierId");
            int? status = GetNullableInt("Status");  // Nếu không có giá trị mặc định, để nguyên
            int? project = GetNullableInt("ProjectDKC");
            string purchaseContract = collection["PurchaseContract"];
            DateTime? dateOfPurchase = GetNullableDateTime("DateOfPurchase");
            DateTime? guarantee = GetNullableDateTime("Guarantee");
            int? userId = GetNullableInt("UserId");
            
            data.AddDevice(
                deviceCode,
                null,                  // Ví dụ: trường NewCode, nếu chưa sử dụng
                deviceName,
                typeOfDevice,
                null,                  // Ví dụ: trường ParentId, nếu chưa sử dụng
                configuration,
                price,
                purchaseContract,
                dateOfPurchase,
                supplierId,
                project,
                guarantee,
                notes,
                userId,
                status
            );

            // Tìm thiết bị vừa được thêm theo DeviceCode (nếu DeviceCode là duy nhất)
            var device = data.Devices.SingleOrDefault(x => x.DeviceCode == deviceCode);
            if (device == null)
            {
                // Nếu không tìm thấy, có thể hiển thị thông báo lỗi hoặc chuyển hướng về trang Create
                TempData["ErrorMessage"] = "Không thể thêm thiết bị. Vui lòng kiểm tra lại thông tin nhập.";
                return RedirectToAction("Create");
            }

            // Chuyển hướng đến trang EditDevice với Id của thiết bị mới
            return RedirectToAction("EditDevice", "Device", new { Id = device.Id });
        }
        [HttpGet]
        [HasCredential(RoleID = "EDIT_DEVICE")]
        public ActionResult EditDevice(int Id)
        {
            ViewBag.CheckDv = data.DeviceOfProjects.Where(x => x.DeviceId == Id).Count();
            ViewBag.CheckDvDv = data.DeviceDevices.Where(x => x.DeviceCodeChildren == Id || x.DeviceCodeParents == Id).Count();

            // ViewData EditDevice
            var DvType = data.DeviceById(Id).AsEnumerable().Select(x => x.TypeOfDevice).SingleOrDefault();
            int a = Convert.ToInt32(DvType);
            ViewData["TypeOfProject"] = data.DeviceOfProjects.Where(x => x.DeviceId == Id);
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["User"] = data.Users.Where(x => x.IsDeleted == false & x.Status == 0).ToList();
            ViewData["Supplier"] = data.Suppliers.ToList();
            ViewData["Device"] = data.Devices.Where(x => x.IsDeleted == false).ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.Status == 1 & x.TypeProject == 1 & x.IsDeleted == false).ToList();
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).AsEnumerable().ToList();
            ViewData["RepairDetail"] = data.SearchRepairDetails(null, null, Id, null).AsEnumerable().ToList();
            ViewData["DeviceHistory"] = data.DeviceHistory().AsEnumerable().Where(x => x.DeviceId == Id).ToList();
            ViewData["UsageDevice"] = data.SearchUseDevice(Id).AsEnumerable().ToList();
            ViewData["SearchDeviceComponant"] = data.SearchDevice(null, null, null, null, null).AsEnumerable().ToList();
            List<ChildrenOfDevice_Result> numbers = new List<ChildrenOfDevice_Result>();
            var lstTypeDevice = data.TypeComponantOfDevice(a).AsEnumerable().Where(x => x.IsDeleted == false).ToList();

            foreach (var item in lstTypeDevice)
            {
                var lstTag = data.ChildrenOfDevice(Id, item.TypeSymbolChildren).AsEnumerable().ToList();
                Array a2 = lstTag.ToArray();
                numbers.Add(new ChildrenOfDevice_Result { TypeName = item.NameTypeChildren, TypeSymbolChildren = item.TypeSymbolChildren, numbers = a2 });
            }
            ViewData["TypeComponantOfDevice"] = numbers;
            //    public Array numbers { get; set; }   

            // Danh sách thiết bị con theo loại của thiết bị cha 
            var chart = data.DeviceById(Id).AsEnumerable().Single();
            return View(chart);
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_DEVICE")]
        public ActionResult EditDevice(IFormCollection collection)
        {
            int? TypeOfDevice = 0;
            var check = collection["TypeOfDevice"];
            if (check == 0)
            {
                TypeOfDevice = Convert.ToInt32(collection["hiddenTypeId"]);
            }
            else
            {
                TypeOfDevice = collection["TypeOfDevice"].Equals("0") ? (int?)null : Convert.ToInt32(collection["TypeOfDevice"]);

            }
            var b = collection["hiddenParentID"];
            var a = Convert.ToInt32(collection["UserId"]);
            int DeviceId = Convert.ToInt32(collection["hiddenIddv"]);
            //int? TypeOfDevice = collection["TypeOfDevice"].Equals("0") ? (int?)null : Convert.ToInt32(collection["TypeOfDevice"]);
            string DeviceCode = collection["DeviceCode"];
            string NewCode = collection["NewCode"];
            string DeviceName = collection["DeviceName"];
            double Price = collection["Price"].Equals("") ? 0 : Convert.ToDouble(collection["Price"]);
            string Configuration = collection["Configuration"];
            int? SupplierId = collection["SupplierId"].Equals("0") ? (int?)null : Convert.ToInt32(collection["SupplierId"]);
            string PurchaseContract = collection["PurchaseContract"];
            string Notes = collection["Notesdv"];
            DateTime? DateOfPurchase = collection["DateOfPurchase"].Equals("") ? (DateTime?)null : Convert.ToDateTime(collection["DateOfPurchase"]);
            DateTime? Guarantee = collection["Guarantee"].Equals("") ? (DateTime?)null : Convert.ToDateTime(collection["Guarantee"]);
            int? UserId = collection["UserId"].Equals("0") ? (int?)null : Convert.ToInt32(collection["UserId"]);
            int? ParentId = collection["hiddenParentID"].Equals("") ? (int?)null : Convert.ToInt32(collection["hiddenParentID"]);
            int Status = Convert.ToInt32(collection["Status"]);
            DateTime? CreatedDate = collection["CreatedDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(collection["CreatedDate"]);
            data.UpdateDevice(DeviceId, DeviceCode, null, DeviceName, TypeOfDevice, ParentId, Configuration, Price, PurchaseContract, DateOfPurchase, SupplierId, Guarantee, UserId, Notes, CreatedDate, Status);


            var lstComponant = data.DeviceDevices.Where(x => x.DeviceCodeParents == DeviceId & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).ToList();
            foreach (var item in lstComponant)
            {
                // update người dùng của thiết bị con khi thiết bị cha thay đổi ng dùng
                data.UpdateUserDevice(item.Value, UserId);
            }

            return RedirectToAction("EditDevice", "Device", DeviceId);
        }
        [HasCredential(RoleID = "RETURN_DEVICETODEPOT")]
        public ActionResult ReturnDeviceInProject(int Idpr, int Iddv)
        {
            bool result = true;
            var check = 0;
            check += data.DeviceDevices.Where(x => x.DeviceCodeChildren == Iddv & x.IsDeleted == false & x.TypeComponant == 1).Count();
            if (check > 0)
            {
                result = false;
            }
            else
            {

                int checkdele = data.ReturnDeviceOfProject(Idpr, Iddv, "");
                if (checkdele > 0)
                    result = true;
            }
            return Json(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "RETURN_DEVICETODEPOT")]
        public JsonResult ReturnDeviceProject(string Id)
        {

            bool result = true;
            var lstId = Id.Split(',');
            var check = 0;
            for (int i = 0; i < lstId.Length; i++)
            {
                var IdP = Convert.ToInt32(lstId[i]);
                check += data.DeviceDevices.Where(x => x.DeviceCodeChildren == IdP & x.IsDeleted == false & x.TypeComponant == 1).Count();
            }
            if (check > 0)
            {
                result = false;
            }
            else
            {
                for (int i = 0; i < lstId.Length; i++)
                {
                    var IdP = Convert.ToInt32(lstId[i]);
                    var lstComponant = data.DeviceDevices.Where(x => x.DeviceCodeParents == IdP & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).ToList();
                    foreach (var item in lstComponant)
                    {
                        data.ReturnDeviceProject(Convert.ToInt32(item));
                    }
                }
                for (int i = 0; i < lstId.Length; i++)
                {
                    if (!lstId[i].Equals(""))
                        data.ReturnDeviceProject(Convert.ToInt32(lstId[i]));
                }
                result = true;
            }
            return Json(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_SUPPLIER")]
        public JsonResult AddSupplier(string Name, string Email, string PhoneNumber, string Address, string Support)
        {
            data.AddSupplier(Name, Email, PhoneNumber, Address, Support);
            bool result = true;
            return Json(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_EMPLOYEE")]
        public JsonResult AddEmployees(string UserName, string FullName, string Email, string PhoneNumber, string Address, string Department, string Position)
        {
            data.AddUser(UserName, null, FullName, Email, PhoneNumber, Address, Department, Position, null, 0);
            bool result = true;
            return Json(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_REPAIR_DEVICE")]
        public JsonResult AddRepairDevice(int Iddv, int user, string Notesrepair)
        {
            if (user == 0)
            {
                data.AddRepairDetails(Iddv, DateTime.Now, null, null, null, null, null, Notesrepair);
            }
            else { data.AddRepairDetails(Iddv, DateTime.Now, null, null, null, null, user, Notesrepair); }
            bool result = true;
            return Json(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_DEVICE_TYPE")]
        public JsonResult AddDeviceType(string TypeName, string TypeSymbol, string Notes)
        {
            bool result = false;
            var listdvt = data.DeviceTypes.Where(x => x.TypeSymbol.Trim() == TypeSymbol.Trim()).ToList();
            if (listdvt.Count() > 0) { result = false; }
            else
            {
                data.AddDeviceType(TypeName, TypeSymbol, Notes);
                result = true;
            }
            return Json(result);
        }
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public JsonResult AddDeviceProject1(string Id, int PJ)
        {
            bool result = true;
            var IdParent = Id.Split(',');
            var check = 0;
            for (int i = 0; i < IdParent.Length; i++)
            {
                var IdP = Convert.ToInt32(IdParent[i]);
                //Check thiết bị xem có phải là thiết bị con nằm trong
                check += data.DeviceDevices.Where(x => x.DeviceCodeChildren == IdP & x.IsDeleted == false & x.TypeComponant == 1).Count();
            }
            if (check > 0)
            {
                result = false;
            }
            else
            {
                // Lấy danh sách thiết bị con từ danh sách thiết bị cha
                for (int i = 0; i < IdParent.Length; i++)
                {
                    var IdP = Convert.ToInt32(IdParent[i]);
                    var lstComponant = data.DeviceDevices.Where(x => x.DeviceCodeParents == IdP & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).ToList();
                    foreach (var item in lstComponant)
                    {
                        // Thêm thiết bị con vào DeviceOfProject
                        data.AddDeviceOfProject(PJ, Convert.ToInt32(item), "");
                    }
                }
                //Thêm thiết bị cha vào DeviceOfProject
                var lstId = Id.Split(',');
                for (int i = 0; i < lstId.Length; i++)
                {
                    if (!lstId[i].Equals(""))
                        data.AddDeviceOfProject(PJ, Convert.ToInt32(lstId[i]), "");
                }
                result = true;
            }
            return Json(result);
        }
        [HttpGet]
        public JsonResult Searchdv(int TypeOfDevice, int Status, int Guarantee, int Project, string DeviceCode)
        {
            data.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;
            var charts = data.SearchDevice(Status, TypeOfDevice, Guarantee, Project, DeviceCode).AsEnumerable().ToList();
            var model = charts.ToList();
            return Json(new
            {
                data = model,
            });
        }
        [HttpGet]
        public JsonResult AddDeviceCode(int TypeOfDevice)
        {
            data.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            // Lấy mã loại thiết bị
            var type = data.OderCode(TypeOfDevice).AsEnumerable().Single().Code;

            // Lấy TypeSymbol từ bảng DeviceTypes
            var typeSymbol = data.DeviceTypes
                .Where(x => x.Id == TypeOfDevice)
                .SingleOrDefault()?.TypeSymbol.Trim();

            if (typeSymbol != null)
            {
                // Thêm code và padding cho TypeSymbol
                var TypeSymbol = typeSymbol + type.ToString().PadLeft(5, '0');

                // Truy vấn bảng Devices để kiểm tra DeviceCode đã tồn tại chưa
                var charts = data.Devices
                    .Where(x => x.DeviceCode == TypeSymbol)
                    .FirstOrDefault();

                // Nếu thiết bị không tồn tại, trả về mã TypeSymbol
                if (charts == null)
                {
                    return Json(new { data = TypeSymbol });
                }
                else
                {
                    // Nếu đã tồn tại, thực hiện lại truy vấn để cập nhật giá trị mới
                    type = data.OderCode(TypeOfDevice).AsEnumerable().Single().Code;
                    typeSymbol = data.DeviceTypes
                        .Where(x => x.Id == TypeOfDevice)
                        .SingleOrDefault()?.TypeSymbol.Trim();

                    TypeSymbol = typeSymbol + type.ToString().PadLeft(5, '0');

                    return Json(new { data = TypeSymbol });
                }
            }

            return Json(new { data = "Invalid TypeOfDevice" });
        }
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public JsonResult AddDeviceProject(int Iddv, int Idpj)
        {
            bool result = true;
            var Check = data.DeviceDevices.Where(x => x.DeviceCodeChildren == Iddv & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).Count();
            if (Check > 0)
            {
                result = false;
            }
            else
            {
                var lstComponant = data.DeviceDevices.Where(x => x.DeviceCodeParents == Iddv & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).ToList();
                foreach (var item in lstComponant)
                {
                    data.AddDeviceOfProject(Idpj, item.Value, "");
                }
                int checkdele = data.AddDeviceOfProject(Idpj, Iddv, "");
                result = true;
            }
            return Json(result);
        }
        [HasCredential(RoleID = "LIQUIDATION_DEVICE")]
        public JsonResult LiquidationDevice(string Id)
        {
            bool result = true;
            var check = 0;
            var IdParent = Id.Split(',');
            for (int i = 0; i < IdParent.Length; i++)
            {
                // Kiểm tra có tồn tại thiết bị con nằm trong thiết bị cha
                var IdCom = Convert.ToInt32(IdParent[i]);
                check += data.DeviceDevices.Where(x => x.DeviceCodeChildren == IdCom & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).Count();
            }
            if (check > 0)
            {
                result = false;
            }
            else
            {
                // Lấy danh sách thiết bị con trong  từ danh sách thiết bị cha
                for (int i = 0; i < IdParent.Length; i++)
                {
                    var IdP = Convert.ToInt32(IdParent[i]);
                    var lstComponant = data.DeviceDevices.Where(x => x.DeviceCodeParents == IdP & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).ToList();
                    foreach (var item in lstComponant)
                    {
                        string v = "," + item + ",";
                        // Thanh lý thiết bị con trong theo thiết bị cha
                        data.LiquidationDevice(v);
                    }
                    //Khi thanh lý TB cha thì  Gỡ thiết bị con ngoài khỏi thiết bị cha
                    var lstComponant_out = data.DeviceDevices.Where(x => x.DeviceCodeParents == IdP & x.IsDeleted == false & x.TypeComponant == 0).Select(x => x.DeviceCodeChildren).ToList();
                    {
                        foreach (var item in lstComponant_out)
                        {
                            data.DeleteDeviceOfDevice(IdP, item, "Thiết bị cha bị thanh lý");
                        }
                    }
                    // KHi thanh lý tb con ở ngoài thì xóa mối quan hệ cha con của thiết bị vs cha còn hoạt động
                    var lstParent = data.DeviceDevices.Where(x => x.DeviceCodeChildren == IdP & x.IsDeleted == false & x.TypeComponant == 0).Select(x => x.DeviceCodeParents).ToList();
                    {
                        foreach (var item in lstParent)
                        {
                            data.DeleteDeviceOfDevice(item, IdP, "Thiết bị con bị thanh lý");
                        }
                    }
                }
                // Thanh lý thiết bị cha
                string a = "," + Id + ",";
                int checkdele = data.LiquidationDevice(a);
                if (checkdele > 0)
                    result = true;
            }
            return Json(result);
        }
        [HasCredential(RoleID = "DELETE_DEVICE")]
        public JsonResult DeleteDevice(string Id)
        {
            bool result = true;
            var check = 0;
            var IdParent = Id.Split(',');
            for (int i = 0; i < IdParent.Length; i++)
            {
                // Kiểm tra có tồn tại thiết bị con nằm trong thiết bị cha
                var IdCom = Convert.ToInt32(IdParent[i]);
                check += data.DeviceDevices.Where(x => x.DeviceCodeChildren == IdCom & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).Count();
            }
            if (check > 0)
            {
                result = false;
            }
            else
            {
                // Lấy danh sách thiết bị con từ danh sách thiết bị cha
                for (int i = 0; i < IdParent.Length; i++)
                {
                    var IdP = Convert.ToInt32(IdParent[i]);
                    var lstComponant = data.DeviceDevices.Where(x => x.DeviceCodeParents == IdP & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).ToList();
                    foreach (var item in lstComponant)
                    {
                        string v = "," + item + ",";
                        // Thanh lý thiết bị con theo thiết bị cha
                        data.DeleteDevice1(v);
                    }
                    //Khi xóa TB cha thì  Gỡ thiết bị con ngoài khỏi thiết bị cha
                    var lstComponant_out = data.DeviceDevices.Where(x => x.DeviceCodeParents == IdP & x.IsDeleted == false & x.TypeComponant == 0).Select(x => x.DeviceCodeChildren).ToList();
                    {
                        foreach (var item in lstComponant_out)
                        {
                            data.DeleteDeviceOfDevice(IdP, item, "Thiết bị cha bị xóa");
                        }
                    }
                    // KHi xóa tb con ở ngoài thì xóa mối quan hệ cha con của thiết bị cha còn hoạt động
                    var lstParent = data.DeviceDevices.Where(x => x.DeviceCodeChildren == IdP & x.IsDeleted == false & x.TypeComponant == 0).Select(x => x.DeviceCodeParents).ToList();
                    {
                        foreach (var item in lstParent)
                        {
                            data.DeleteDeviceOfDevice(item, IdP, "Thiết bị con bị xóa");
                        }
                    }
                }
                string a = "," + Id + ",";
                int checkdele = data.DeleteDevice1(a);
                if (checkdele > 0)
                    result = true;
            }
            return Json(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "PRINTBARCODE_DEVICE")]
        public JsonResult GenerateBarCode(string barcode)
        {
            string src = "";

            using (Bitmap bitMap = new Bitmap(barcode.Length * 46, 80))
            {
                using (Graphics graphics = Graphics.FromImage(bitMap))
                {
                    // Load font from file
                    PrivateFontCollection pfc = new PrivateFontCollection();
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "App_Data", "IDAutomationHC39XL.ttf"); // Change to correct path
                    pfc.AddFontFile(path);

                    Font oFont = new Font(pfc.Families[0], 14, FontStyle.Regular);
                    PointF point = new PointF(2f, 2f);
                    SolidBrush blackBrush = new SolidBrush(Color.Black);
                    SolidBrush whiteBrush = new SolidBrush(Color.White);

                    graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                    graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    src = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
            }

            return Json(new
            {
                data = src
            });
        }
        private string GenerateQRCode(string code)
        {
            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
                using (var qrCode = new QRCode(qrCodeData))
                using (var bitmap = qrCode.GetGraphic(20))
                using (var ms = new MemoryStream())
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    string result = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    System.Diagnostics.Debug.WriteLine(result);

                    return result;
                }
            }
        }
        [HttpPost]
        [HasCredential(RoleID = "PRINTBARCODE_DEVICE")]
        public JsonResult GenerateBarCodeDevice(string dvcode, string dvid)
        {
            var idList = dvid.Split(',', StringSplitOptions.RemoveEmptyEntries)
                             .Select(int.Parse)
                             .ToList();
            var listdv = data.SearchDevice(null, null, null, null, null).AsEnumerable()
                .Where(t => idList.Contains(t.Id))
                .ToList();
            List<string> barcodeImages = new List<string>();
            var codes = dvcode.Split(',', StringSplitOptions.RemoveEmptyEntries);

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                foreach (var code in codes)
                {
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
                    using (QRCode qrCode = new QRCode(qrCodeData))
                    using (Bitmap bitmap = qrCode.GetGraphic(3))
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, ImageFormat.Png);
                        byte[] byteImage = ms.ToArray();
                        string base64 = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                        barcodeImages.Add(base64);
                    }
                }
            }
            var result = new
            {
                list = barcodeImages,
                Listdv = listdv
            };

            return Json(result);
        }
        public static string HtmlToPlainText(string html)
        {
            const string tagWhiteSpace = @"(>|$)(\W|\n|\r)+<";//matches one or more (white space or line breaks) between '>' and '<'
            const string stripFormatting = @"<[^>]*(>|$)";//match any character between '<' and '>', even when end tag is missing
            const string lineBreak = @"<(br|BR)\s{0,1}\/{0,1}>";//matches: <br>,<br/>,<br />,<BR>,<BR/>,<BR />
            var lineBreakRegex = new Regex(lineBreak, RegexOptions.Multiline);
            var stripFormattingRegex = new Regex(stripFormatting, RegexOptions.Multiline);
            var tagWhiteSpaceRegex = new Regex(tagWhiteSpace, RegexOptions.Multiline);
            var text = html;
            //Decode html specific characters
            text = System.Net.WebUtility.HtmlDecode(text);
            //Remove tag whitespace/line breaks
            text = tagWhiteSpaceRegex.Replace(text, "><");
            //Replace <br /> with line breaks
            text = lineBreakRegex.Replace(text, Environment.NewLine);
            //Strip formatting
            text = stripFormattingRegex.Replace(text, string.Empty);
            return text;
        }
        [HasCredential(RoleID = "EXEL_LIST_DDEVICE")]
        public async Task<IActionResult> ExportToExcel(int? TypeOfDevice, int Status, int Guarantee, int? Project, string DeviceCode)
        {
            // Lấy dữ liệu từ service
            var charts = data.SearchDevice(Status, TypeOfDevice, Guarantee, Project, DeviceCode).AsEnumerable()
                .Where(x => x.Status != 2)
                .ToList();

            // Chuyển dữ liệu thành danh sách NewConfig
            var model = charts.Select(i => new NewConfig
            {
                DeviceCode = i.DeviceCode,
                DeviceName = i.DeviceName,
                TypeName = i.TypeName,
                Configuration = HtmlToPlainText(i.Configuration),
                PriceOne = i.PriceOne,
                FullName = i.FullName,
                Name = i.Name,
                ProjectSymbol = i.ProjectSymbol,
                Status = i.Status
            }).ToList();

            // Tạo file Excel với EPPlus
            var fileName = $"Devices_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "exports");

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            var filePath = Path.Combine(folderPath, fileName);

            // Tạo file Excel
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Devices");

                // Tạo các tiêu đề cột
                ws.Cells[1, 1].Value = "Mã";
                ws.Cells[1, 2].Value = "Tên thiết bị";
                ws.Cells[1, 3].Value = "Loại";
                ws.Cells[1, 4].Value = "Cấu hình";
                ws.Cells[1, 5].Value = "Giá";
                ws.Cells[1, 6].Value = "Họ và tên";
                ws.Cells[1, 7].Value = "Tên dự án";
                ws.Cells[1, 8].Value = "Trạng thái";

                // Thêm dữ liệu vào các ô
                for (int i = 0; i < model.Count; i++)
                {
                    var row = i + 2; // Dữ liệu bắt đầu từ dòng 2 (dòng 1 là tiêu đề)
                    ws.Cells[row, 1].Value = model[i].DeviceCode;
                    ws.Cells[row, 2].Value = model[i].DeviceName;
                    ws.Cells[row, 3].Value = model[i].TypeName;
                    ws.Cells[row, 4].Value = model[i].Configuration;
                    ws.Cells[row, 5].Value = model[i].PriceOne;
                    ws.Cells[row, 6].Value = model[i].FullName;
                    ws.Cells[row, 7].Value = model[i].ProjectSymbol;
                    ws.Cells[row, 8].Value = model[i].Status;
                }

                // Lưu file Excel vào thư mục exports
                await package.SaveAsAsync(new FileInfo(filePath));
            }

            // Trả về URL của tệp vừa tạo
            var fileUrl = Url.Content($"~/exports/{fileName}");

            return Ok(new
            {
                success = true,
                message = "Xuất Excel thành công",
                fileUrl = fileUrl
            });
        }
        [HasCredential(RoleID = "VIEW_STATISTICAL_DEVICE")]
        public ActionResult StatisticalDevice()
        {
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            var lstdv = data.StatisticalDevice().ToList();
            return View(lstdv);
        }
        // [HttpPost]
        public ActionResult SearchStatisticalDevice(IFormCollection collection)
        {
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            int? Status = Convert.ToInt32(collection["Status"]);
            int? TypeOfDevice = Convert.ToInt32(collection["TypeOfDevice"]);
            var charts = data.StatisticalDevice().ToList();
            if (Status == -1 & TypeOfDevice != 0) { charts = data.StatisticalDevice().AsEnumerable().Where(x => x.TypeOfDevice == TypeOfDevice).ToList(); }
            else if (TypeOfDevice == 0 & Status != -1) { charts = data.StatisticalDevice().AsEnumerable().Where(x => x.Status == Status).ToList(); }
            else if (Status != -1 & TypeOfDevice != 0) { charts = data.StatisticalDevice().AsEnumerable().Where(x => x.Status == Status & x.TypeOfDevice == TypeOfDevice).ToList(); }
            ViewBag.status = Status;
            ViewBag.type = TypeOfDevice;
            var model = charts.ToList();
            return View("StatisticalDevice", model);
        }
        [HasCredential(RoleID = "EXPORT_STATISTICAL_DEVICE")]
        public IActionResult ExportStatisticalDevice()
        {
            // Lấy dữ liệu từ cơ sở dữ liệu
            var charts = data.StatisticalDevice()
                .Select(i => new
                {
                    i.DeviceCode,
                    i.DeviceName,
                    i.PriceOne,
                    i.TimeUse,
                    i.TimeRepair,
                    i.SumPrice
                })
                .ToList();

            // Tạo file Excel bằng EPPlus
            using (var package = new ExcelPackage())
            {
                // Tạo worksheet trong file Excel
                var worksheet = package.Workbook.Worksheets.Add("DeviceStatistics");

                // Tạo tiêu đề cột
                worksheet.Cells[1, 1].Value = "Device Code";
                worksheet.Cells[1, 2].Value = "Device Name";
                worksheet.Cells[1, 3].Value = "Price One";
                worksheet.Cells[1, 4].Value = "Time Use";
                worksheet.Cells[1, 5].Value = "Time Repair";
                worksheet.Cells[1, 6].Value = "Sum Price";

                // Điền dữ liệu vào các hàng
                for (int i = 0; i < charts.Count; i++)
                {
                    var item = charts[i];
                    worksheet.Cells[i + 2, 1].Value = item.DeviceCode;
                    worksheet.Cells[i + 2, 2].Value = item.DeviceName;
                    worksheet.Cells[i + 2, 3].Value = item.PriceOne;
                    worksheet.Cells[i + 2, 4].Value = item.TimeUse;
                    worksheet.Cells[i + 2, 5].Value = item.TimeRepair;
                    worksheet.Cells[i + 2, 6].Value = item.SumPrice;
                }

                // Thiết lập kiểu file Excel và trả về file cho người dùng
                var fileName = $"DeviceStatistics_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
                var fileBytes = package.GetAsByteArray();

                return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpGet]
        [HasCredential(RoleID = "VIEW_EMPLOYEE")]
        public JsonResult GetEmployees(int id)
        {
            data.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var Employees = data.Users.Find(id);
            return Json(new
            {
                data = Employees,
            });
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_EMPLOYEE")]
        public JsonResult EditEmployees(int Id, string UserName, string FullName, string PhoneNumber, string Email, string Department, string Position, string Address)
        {
            bool result = true;
            data.UpdateUser(Id, UserName, null, FullName, Email, PhoneNumber, Address, Department, Position, null, 0);
            return Json(result);
        }
        [HasCredential(RoleID = "ADD_TYPE_COMPONAN")]
        public JsonResult AddTypeChidren(int TypeChidren, int TypeParent)
        {
            bool result = false;
            // Check xem đã tồn tại loại Cha-con
            var listdvt = data.DeviceTypeComponantTypes.Where(x => x.TypeSymbolChildren == TypeChidren && x.TypeSymbolParents == TypeParent).Where(x => x.IsDeleted == false).ToList();
            if (listdvt.Count() > 0)
            { result = false; }
            else
            {
                data.AddTypeChidren(TypeChidren, TypeParent, null);
                result = true;
            }
            return Json(result);
        }

        [HttpPost]
        public JsonResult SearchDeviceComponant(int TypeChidren)
        {
            var lst = data.SearchDevice(null, TypeChidren, null, null, null).AsEnumerable().Where(x => (x.Status == 0 || x.Status == 1) & x.StatusRepair != 1 & x.ParentId == null).ToList();
            var result = new { lst };
            return Json(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_DEVICE_DEVICE")]
        public JsonResult AddDeviceOfDevice(string dvChild, int dvParent, int TypeChild, int TypeParent, int Type_TypeCom)
        {
            var lstId = dvChild.Split(',');
            for (int i = 0; i < lstId.Length; i++)
            {
                if (!lstId[i].Equals(""))
                    data.AddDeviceOfDevice(dvParent, Convert.ToInt32(lstId[i]), TypeChild, TypeParent, Type_TypeCom);
            }
            bool result = true;
            return Json(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "DELETE_DEVICE_DEVICE")]
        public JsonResult DeleteDvComponent(int dvChild, int dvParent, string Resons)
        {
            data.DeleteDeviceOfDevice(dvParent, dvChild, Resons);
            bool result = true;
            return Json(result);
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_DEVICE_DEVICE")]
        public JsonResult AddDeviceComponantNew(Com com)
        {
            // Xét null tránh bị lỗi kiểu dữ liệu
            if (com.Config.Trim() == "" || com.Config == null)
            {
                com.Config = "";
            }
            int? Project = com.Project.Equals(0) ? (int?)null : Convert.ToInt32(com.Project);
            int? SupplierId = com.SupplierId.Equals(0) ? (int?)null : Convert.ToInt32(com.SupplierId);
            int? UserId = com.UserId.Equals(0) ? (int?)null : Convert.ToInt32(com.UserId);
            DateTime? DateOfPurchase = (com.PurchaseDate).ToString().Equals("1/1/0001 12:00:00 AM") ? (DateTime?)null : Convert.ToDateTime(com.PurchaseDate);
            DateTime? Guarantee = (com.Guarantee).ToString().Equals("1/1/0001 12:00:00 AM") ? (DateTime?)null : Convert.ToDateTime(com.Guarantee);
            bool result = true;
            //Thêm thiết bị mới
            data.AddDevice((com.DeviceCode).Trim(), null, com.Name, com.TypeComponant, null, com.Config, com.Price, com.PurchaseContract, DateOfPurchase, SupplierId, Project, Guarantee, com.Notes, UserId, com.status);

            //Thêm vào bảng cha con
            var IdChild = data.Devices.Where(x => x.DeviceCode == com.DeviceCode).Single().Id;
            data.AddDeviceOfDevice(com.IdParent, IdChild, com.TypeComponant, com.TypeParent, com.Type_TypeCom);
            return Json(result);
        }
        [HasCredential(RoleID = "MANAGER_TYPE_PR_DV")]
        public ActionResult ManagerTypeParent()
        {
            List<Libs.DTOs.TypeComponantOfDevice_Result> numbers = new List<Libs.DTOs.TypeComponantOfDevice_Result>();
            var lstTypeParent = data.DeviceTypeComponantTypes.Where(x => x.IsDeleted == false).Select(x => x.TypeSymbolParents).Distinct().ToList();

            foreach (var item in lstTypeParent)
            {
                if (item.HasValue)
                {
                    var Name_Parent = data.TypeComponantOfDevice(item.Value).AsEnumerable().Select(i => new { i.TypeSymbolParents, i.NameTypeParents }).FirstOrDefault();

                    if (Name_Parent != null)
                    {
                        var lstChild = data.TypeComponantOfDevice(item.Value).AsEnumerable().Where(x => x.IsDeleted == false).ToList();
                        Array a2 = lstChild.ToArray();
                        numbers.Add(new Libs.DTOs.TypeComponantOfDevice_Result
                        {
                            NameTypeParents = Name_Parent.NameTypeParents,
                            TypeSymbolParents = Name_Parent.TypeSymbolParents,
                            numbers = a2
                        });
                    }
                }
            }
            ViewData["TypeParentTypeChild"] = numbers;
            return View();
            // public Array numbers { get; set; }
        }
        [HasCredential(RoleID = "DELETE_TYPE_PR_DV")]
        public ActionResult DeleteTypeChildOfParent(int idChild, int idParent)
        {
            bool result = true;
            // Check xem còn tồn tại thiết bị con thuộc loại bị xóa không ?
            int check = data.DeviceDevices.Where(x => x.TypeSymbolChildren == idChild & x.TypeSymbolParents == idParent & x.IsDeleted == false).Count();
            if (check > 0)
            {
                result = false;
            }
            else
            {
                data.DeleteTypeParentTypeChild(idChild, idParent);
                result = true;
            }
            return Json(result);
        }
    }
}
