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
        public ActionResult Device()
        {
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.TypeProject == 1 & x.IsDeleted == false).ToList();
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).ToList();
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

        public ActionResult ScanerDevice()
        {
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.Status != 3 & x.TypeProject == 1 & x.IsDeleted == false).ToList();
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult SearchDevice(IFormCollection collection)
        {
            var d = data.DeviceTypes.ToList();
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.Status != 3 && x.TypeProject == 1 && x.IsDeleted == false).ToList();
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).ToList();

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
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 0, null).ToList();
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.Status != 3 & x.IsDeleted == false).ToList();
            var lstDevice = data.SearchDevice(null, null, null, null, null)
                              .AsEnumerable()
                              .Where(x => x.Status == 2)
                              .ToList();
            return View(lstDevice);
        }
        public ActionResult TypeDevice(int Id)
        {
            ViewBag.type = Id;
            var deviceType = data.DeviceTypes.SingleOrDefault(x => x.Id == Id);
            ViewBag.Title = deviceType != null ? deviceType.TypeName : "Unknown";

            ViewData["TypeOfDevice"] = data.DeviceTypes.Where(x => x.Id == Id).ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.IsDeleted == false && x.TypeProject == 1).ToList();
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).ToList();

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
            ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).ToList();

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

        //[HasCredential(RoleID = "ADD_DEVICE")]
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
        public async Task<IActionResult> EditDevice(int id)
        {
            // Lấy thông tin thiết bị thông qua stored procedure DeviceById
            var deviceResult = (await data.DeviceById(id).ToListAsync()).SingleOrDefault();
            if (deviceResult == null)
                return NotFound();

            // Map dữ liệu từ deviceResult sang view model
            var viewModel = new DeviceById_Result
            {
                Id = deviceResult.Id,  // Đảm bảo Id là đúng từ DB
                DeviceCode = deviceResult.DeviceCode,  // Đảm bảo DeviceCode được lấy từ DB
                DeviceName = deviceResult.DeviceName,  // Đảm bảo DeviceName đúng
                TypeOfDevice = deviceResult.TypeOfDevice,
                ParentId = deviceResult.ParentId,
                Configuration = deviceResult.Configuration,
                Price = deviceResult.Price,
                PurchaseContract = deviceResult.PurchaseContract,
                DateOfPurchase = deviceResult.DateOfPurchase,
                SupplierId = deviceResult.SupplierId,
                Guarantee = deviceResult.Guarantee,
                Notes = deviceResult.Notes,
                UserId = deviceResult.UserId,
                Status = deviceResult.Status,
                CreatedDate = deviceResult.CreatedDate,
                ModifiedDate = deviceResult.ModifiedDate,
                ProjectName = deviceResult.ProjectName,
                IdProject = deviceResult.IdProject,
                StatusRepair = deviceResult.StatusRepair,
                NewCode = deviceResult.NewCode,
                PriceOne = deviceResult.PriceOne
            };

            // Lấy các thông tin bổ sung từ các SP khác và bảng thông thường
            ViewBag.CheckDv = await data.DeviceOfProjects.Where(x => x.DeviceId == id).CountAsync();
            ViewBag.CheckDvDv = await data.DeviceDevices
                .Where(x => x.DeviceCodeChildren == id || x.DeviceCodeParents == id)
                .CountAsync();

            // Kiểm tra và gán giá trị cho ViewData
            ViewData["TypeOfDevice"] = await data.DeviceTypes.ToListAsync();
            ViewData["User"] = await data.Users.Where(x => x.IsDeleted == false && x.Status == 0).ToListAsync();
            ViewData["Supplier"] = await data.Suppliers.ToListAsync();
            ViewData["Device"] = await data.Devices.Where(x => x.IsDeleted == false).ToListAsync();
            ViewData["ProjectDKC"] = await data.ProjectDkcs
                .Where(x => x.Status == 1 && x.TypeProject == 1 && x.IsDeleted == false)
                .ToListAsync();
            ViewData["sProjectDKC"] = await data.SearchProject(null, 1, 1, null).ToListAsync();

            // Kiểm tra dữ liệu sửa chữa
            var repairDetails = await data.SearchRepairDetails(null, null, id, null).ToListAsync();
            ViewData["RepairDetail"] = repairDetails ?? new List<SearchRepairDetails_Result>();  // Gán danh sách trống nếu không có dữ liệu

            // Lịch sử thiết bị
            var deviceHistoryData = await data.DeviceHistory().ToListAsync();
            ViewData["DeviceHistory"] = deviceHistoryData.Where(x => x.DeviceId == id).ToList();

            // Các dữ liệu khác
            ViewData["UsageDevice"] = await data.SearchUseDevice(id).ToListAsync();
            ViewData["SearchDeviceComponant"] = await data.SearchDevice(null, null, null, null, null).ToListAsync();

            // Lấy danh sách các thành phần con (TypeComponant) dựa vào TypeOfDevice
            List<ChildrenOfDevice_Result> numbers = new List<ChildrenOfDevice_Result>();
            int deviceType = deviceResult.TypeOfDevice ?? 0;
            var typeComponantList = await data.TypeComponantOfDevice(deviceType).ToListAsync();
            var lstTypeDevice = typeComponantList.Where(x => x.IsDeleted == false).ToList();
            foreach (var item in lstTypeDevice)
            {
                var lstTag = await data.ChildrenOfDevice(id, item.TypeSymbolChildren).ToListAsync();
                numbers.Add(new ChildrenOfDevice_Result
                {
                    TypeName = item.NameTypeChildren,
                    TypeSymbolChildren = item.TypeSymbolChildren,
                    numbers = lstTag.ToArray()
                });
            }
            ViewData["TypeComponantOfDevice"] = numbers;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditDevice(DeviceById_Result model)
        {
            // Kiểm tra dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                ViewData["TypeOfDevice"] = await data.DeviceTypes.ToListAsync();
                ViewData["User"] = await data.Users.Where(x => x.IsDeleted == false && x.Status == 0).ToListAsync();
                ViewData["Supplier"] = await data.Suppliers.ToListAsync();
                ViewData["ProjectDKC"] = await data.ProjectDkcs
                    .Where(x => x.Status == 1 && x.TypeProject == 1 && x.IsDeleted == false)
                    .ToListAsync();
                ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).AsEnumerable().ToList();
                return View(model);
            }

            // Kiểm tra xem SupplierId có tồn tại trong bảng Supplier không
            var supplierExists = await data.Suppliers.AnyAsync(x => x.Id == model.SupplierId);
            if (!supplierExists)
            {
                ModelState.AddModelError("", "Supplier ID không hợp lệ.");
                // Reload necessary data if validation fails
                ViewData["TypeOfDevice"] = await data.DeviceTypes.ToListAsync();
                ViewData["User"] = await data.Users.Where(x => x.IsDeleted == false && x.Status == 0).ToListAsync();
                ViewData["Supplier"] = await data.Suppliers.ToListAsync();
                ViewData["ProjectDKC"] = await data.ProjectDkcs
                    .Where(x => x.Status == 1 && x.TypeProject == 1 && x.IsDeleted == false)
                    .ToListAsync();
                ViewData["sProjectDKC"] = data.SearchProject(null, 1, 1, null).AsEnumerable().ToList();
                return View(model);
            }

            // Cập nhật thiết bị thông qua stored procedure UpdateDevice
            await data.Database.ExecuteSqlInterpolatedAsync(
                $"EXEC dbo.UpdateDevice {model.Id}, {model.DeviceCode}, {model.NewCode}, {model.DeviceName}, {model.TypeOfDevice}, {model.ParentId}, {model.Configuration}, {model.Price}, {model.PurchaseContract}, {model.DateOfPurchase}, {model.SupplierId}, {model.Guarantee}, {model.UserId}, {model.Notes}, {model.CreatedDate}, {model.Status}");

            // Cập nhật thông tin người dùng của các thiết bị con (nếu có)
            var lstComponant = await data.DeviceDevices
                .Where(x => x.DeviceCodeParents == model.Id && x.IsDeleted == false && x.TypeComponant == 1)
                .Select(x => x.DeviceCodeChildren)
                .ToListAsync();
            foreach (var childId in lstComponant)
            {
                await data.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC dbo.UpdateUserDevice {childId.Value}, {model.UserId}");
            }

            return RedirectToAction("EditDevice", new { id = model.Id });
        }
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
        public JsonResult AddSupplier(string Name, string Email, string PhoneNumber, string Address, string Support)
        {
            data.AddSupplier(Name, Email, PhoneNumber, Address, Support);
            bool result = true;
            return Json(result);
        }
        [HttpPost]
        public JsonResult AddEmployees(string UserName, string FullName, string Email, string PhoneNumber, string Address, string Department, string Position)
        {
            data.AddUser(UserName, null, FullName, Email, PhoneNumber, Address, Department, Position, null, 0);
            bool result = true;
            return Json(result);
        }
        [HttpPost]
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
            var charts = data.SearchDevice(Status, TypeOfDevice, Guarantee, Project, DeviceCode).ToList();
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
        [HttpPost]
        public JsonResult GenerateBarCodeDevice(string dvcode, string dvid)
        {
            List<int> idList = dvid.Split(',')
                .Where(id => !string.IsNullOrEmpty(id))
                .Select(id => Convert.ToInt32(id))
                .ToList();
            var Listdv = data.SearchDevice(null, null, null, null, null)
                .Where(t => idList.Contains(t.Id))
                .ToList();
            List<string> list = new List<string>();
            var lstdv = dvcode.Split(',');

            foreach (var code in lstdv)
            {
                string barcodeImagePath = SaveBarcodeImage(code);
                list.Add(barcodeImagePath);
            }

            var result = new { list, Listdv };
            return Json(result); // Trả về kết quả dưới dạng JSON
        }

        private string SaveBarcodeImage(string barcode)
        {
            string fileName = $"{barcode}.png";  // Tạo tên tệp hình ảnh
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "barcodes", fileName);
            using (Bitmap bitMap = new Bitmap(barcode.Length * 46, 80))
            {
                using (Graphics graphics = Graphics.FromImage(bitMap))
                {
                    // Tải font từ file
                    PrivateFontCollection pfc = new PrivateFontCollection();
                    string path = Path.Combine(_webHostEnvironment.WebRootPath, "App_Data", "IDAutomationHC39XL.ttf");  // Đảm bảo font có sẵn trong thư mục wwwroot/App_Data
                    pfc.AddFontFile(path);

                    Font oFont = new Font(pfc.Families[0], 14, FontStyle.Regular);
                    PointF point = new PointF(2f, 2f);
                    SolidBrush blackBrush = new SolidBrush(Color.Black);
                    SolidBrush whiteBrush = new SolidBrush(Color.White);

                    graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                    graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                }
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));  // Tạo thư mục nếu chưa có
                bitMap.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            return "/barcodes/" + fileName;
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
        [HttpGet("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(int? TypeOfDevice, int Status, int Guarantee, int? Project, string DeviceCode)
        {
            // Lấy dữ liệu từ service
            var charts = data.SearchDevice(Status, TypeOfDevice, Guarantee, Project, DeviceCode)
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
            if (Status == -1 & TypeOfDevice != 0) { charts = data.StatisticalDevice().Where(x => x.TypeOfDevice == TypeOfDevice).ToList(); }
            else if (TypeOfDevice == 0 & Status != -1) { charts = data.StatisticalDevice().Where(x => x.Status == Status).ToList(); }
            else if (Status != -1 & TypeOfDevice != 0) { charts = data.StatisticalDevice().Where(x => x.Status == Status & x.TypeOfDevice == TypeOfDevice).ToList(); }
            ViewBag.status = Status;
            ViewBag.type = TypeOfDevice;
            var model = charts.ToList();
            return View("StatisticalDevice", model);
        }
        public IActionResult ExportStatisticalDevice()
        {
            data.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.NoTracking;

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
        public JsonResult EditEmployees(int Id, string UserName, string FullName, string PhoneNumber, string Email, string Department, string Position, string Address)
        {
            bool result = true;
            data.UpdateUser(Id, UserName, null, FullName, Email, PhoneNumber, Address, Department, Position, null, 0);
            return Json(result);
        }
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
            var lst = data.SearchDevice(null, TypeChidren, null, null, null).Where(x => (x.Status == 0 || x.Status == 1) & x.StatusRepair != 1 & x.ParentId == null).ToList();
            var result = new { lst };
            return Json(result);
        }
        [HttpPost]
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
        public JsonResult DeleteDvComponent(int dvChild, int dvParent, string Resons)
        {
            data.DeleteDeviceOfDevice(dvParent, dvChild, Resons);
            bool result = true;
            return Json(result);
        }
        [HttpPost]
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

        public ActionResult ManagerTypeParent()
        {
            List<Libs.DTOs.TypeComponantOfDevice_Result> numbers = new List<Libs.DTOs.TypeComponantOfDevice_Result>();
            var lstTypeParent = data.DeviceTypeComponantTypes.Where(x => x.IsDeleted == false).Select(x => x.TypeSymbolParents).Distinct().ToList();

            foreach (var item in lstTypeParent)
            {
                if (item.HasValue)
                {
                    var Name_Parent = data.TypeComponantOfDevice(item.Value).Select(i => new { i.TypeSymbolParents, i.NameTypeParents }).FirstOrDefault();

                    if (Name_Parent != null)
                    {
                        var lstChild = data.TypeComponantOfDevice(item.Value).Where(x => x.IsDeleted == false).ToList();
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
