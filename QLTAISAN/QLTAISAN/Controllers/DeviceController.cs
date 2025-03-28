namespace QLTAISAN.Controllers
{
    public class DeviceController : Controller
    {
        QuanLyTaiSanCtyDATNContext data = new QuanLyTaiSanCtyDATNContext();
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
        public ActionResult SearchDevice(FormCollection collection)
        {
            var d = data.DeviceTypes.ToList();
            ViewData["TypeOfDevice"] = data.DeviceTypes.ToList();
            ViewData["ProjectDKC"] = data.ProjectDkcs.Where(x => x.Status != 3 & x.TypeProject == 1 & x.IsDeleted == false).ToList();
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
        public ActionResult SearchTypeDevice(FormCollection collection)
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
        public ActionResult Create(FormCollection collection)
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
                Id = deviceResult.Id,
                DeviceCode = deviceResult.DeviceCode,
                NewCode = deviceResult.NewCode,
                DeviceName = deviceResult.DeviceName,
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
                CreatedDate = deviceResult.CreatedDate
            };

            // Lấy các thông tin bổ sung từ các SP khác và bảng thông thường
            ViewBag.CheckDv = await data.DeviceOfProjects.Where(x => x.DeviceId == id).CountAsync();
            ViewBag.CheckDvDv = await data.DeviceDevices
                .Where(x => x.DeviceCodeChildren == id || x.DeviceCodeParents == id)
                .CountAsync();

            ViewData["TypeOfDevice"] = await data.DeviceTypes.ToListAsync();
            ViewData["User"] = await data.Users.Where(x => x.IsDeleted == false && x.Status == 0).ToListAsync();
            ViewData["Supplier"] = await data.Suppliers.ToListAsync();
            ViewData["Device"] = await data.Devices.Where(x => x.IsDeleted == false).ToListAsync();
            ViewData["ProjectDKC"] = await data.ProjectDkcs
                .Where(x => x.Status == 1 && x.TypeProject == 1 && x.IsDeleted == false)
                .ToListAsync();
            ViewData["sProjectDKC"] = await data.SearchProject(null, 1, 1, null).ToListAsync();
            ViewData["RepairDetail"] = await data.SearchRepairDetails(null, null, id, null).ToListAsync();
            var deviceHistoryData = await data.DeviceHistory().ToListAsync();
            ViewData["DeviceHistory"] = deviceHistoryData.Where(x => x.DeviceId == id).ToList();
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
            if (!ModelState.IsValid)
            {
                ViewData["TypeOfDevice"] = await data.DeviceTypes.ToListAsync();
                ViewData["User"] = await data.Users.Where(x => x.IsDeleted == false && x.Status == 0).ToListAsync();
                ViewData["Supplier"] = await data.Suppliers.ToListAsync();
                ViewData["ProjectDKC"] = await data.ProjectDkcs
                    .Where(x => x.Status == 1 && x.TypeProject == 1 && x.IsDeleted == false)
                    .ToListAsync();
                ViewData["sProjectDKC"] = await data.SearchProject(null, 1, 1, null).ToListAsync();
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
    }
}
