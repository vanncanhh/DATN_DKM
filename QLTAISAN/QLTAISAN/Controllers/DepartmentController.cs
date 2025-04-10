namespace QLTAISAN.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly QuanLyTaiSanCtyDATNContext Ql;
        public DepartmentController(QuanLyTaiSanCtyDATNContext ql)
        {
            Ql = ql;
        }
        [HasCredential(RoleID = "VIEW_DEPARTMENT")] //phân quyền
        public ActionResult Department()
        {
            ViewBag.ProjectNb = Ql.SearchProject(null, null, 1, null)
                        .AsEnumerable()
                        .Count();
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
            var lstProject = Ql.SearchProject(null, null, 1, null)
                                    .AsEnumerable()
                                    .ToList();
            return View(lstProject);
        }

        [HttpPost]
        public ActionResult SeachDepartment(IFormCollection colection, ProjectDkc Project)
        {
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
            String ProjectSymbol = colection["ProjectSymbol"].ToString().Trim();
            //   int? Status = colection["Status"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Status"]);
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            var lstProject = Ql.SearchProject(ManagerProject, null, 1, ProjectSymbol).AsEnumerable().ToList();
            var ViewProject = lstProject;
            // ViewBag.Status = Status;
            ViewBag.ManagerProject = ManagerProject;
            ViewBag.ProjectSymbol = ProjectSymbol;
            ViewBag.ProjectNb = Ql.SearchProject(ManagerProject, null, 1, ProjectSymbol).AsEnumerable().Count();
            return View("Department", ViewProject);
        }

        [HasCredential(RoleID = "ADD_DEPARTMENT")]
        public ActionResult AddDepartment()
        {
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
            return View();
        }
        [HttpPost]
        [HasCredential(RoleID = "ADD_REPAIR_DEVICE")]
        public JsonResult AddRepairType(string TypeName, string Notes)
        {
            Ql.AddRepairType(TypeName, Notes);
            bool result = true;
            return Json(result);
        }

        [HttpPost]
        [HasCredential(RoleID = "ADD_DEPARTMENT")]
        public ActionResult AddDepartment(IFormCollection colection, ProjectDkc Project)
        {

            String ProjectSymbol = colection["ProjectSymbol"];
            String ProjectName = colection["ProjectName"];
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            String Address = colection["Address"];
            int? Status = colection["Status"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Status"]);
            var lstProjectSymbol = Ql.ProjectDkcs.Where(x => x.ProjectSymbol.Trim() == ProjectSymbol.Trim()).ToList();
            var CountPr = lstProjectSymbol.Count();
            if (CountPr > 0)
            {
                ViewBag.Tb = "Mã bị trùng";
                ViewBag.ProjectSymbol = ProjectSymbol;
                ViewBag.ProjectName = ProjectName;
                ViewBag.ManagerProject = ManagerProject;
                ViewBag.Address = Address;
                ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
                return View();
            }
            else
            {
                Ql.AddProject_(ProjectSymbol, ProjectName, ManagerProject, Address, null, null, Status);
                return RedirectToAction("Department", "Department");
            }
        }
        [HasCredential(RoleID = "EDIT_DEPARTMENT")]
        public ActionResult EditDepartment(int Id)
        {
            var lstType = Ql.DeviceTypes.ToList();
            ViewData["DeviceOfProjectAll"] = Ql.DeviceOfProjectAll(Id).ToList();
            var lstDeviceInProject = Ql.DeviceOfProjectAll(Id).ToList();
            ViewData["DeviceOfProjects"] = Ql.DeviceOfProjects.Where(x => x.ProjectId == Id).ToList();
            var tempList = lstDeviceInProject.GroupBy(k => k.TypeOfDevice).ToList();
            var map = new Dictionary<string, int>();
            foreach (var i in tempList)
            {
                var typeName = lstType.FirstOrDefault(k => k.Id == i.Key).TypeName;
                map.Add(typeName, i.Count());
            }
            ViewData["CountingDeviceType"] = map;
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted == false).ToList();
            return View(Ql.ProjectDkcs.Find(Id));
        }

        [HttpPost]
        [HasCredential(RoleID = "EDIT_DEPARTMENT")]
        public ActionResult EditDepartment(IFormCollection colection, ProjectDkc Project)
        {
            int? Id = colection["Id"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Id"]);
            String ProjectSymbol = colection["ProjectSymbol"];
            String ProjectName = colection["ProjectName"];
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            String Address = colection["Address"];
            //   DateTime? FromDate = colection["FromDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["FromDate"]);
            //  DateTime? ToDate = colection["ToDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["ToDate"]);
            DateTime? CreatedDate = colection["CreatedDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["CreatedDate"]);
            DateTime ModifiedDate = Convert.ToDateTime(colection["ModifiedDate"]);
            int? Status = colection["Status"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Status"]);
            bool IsDeleted = Convert.ToBoolean(colection["IsDeleted"]);
            Ql.UpdateProject(Id, ProjectSymbol, ProjectName, ManagerProject, Address, null, null, CreatedDate, ModifiedDate, Status, IsDeleted);
            return RedirectToAction("EditDepartment", "Department");
        }

        [HasCredential(RoleID = "DELETE_DEPARTMENT")]
        public JsonResult DeleteDepartment(string Id)
        {
            bool checkIsset = true;
            bool result = false;
            string[] arrId = Id.Split(',');
            foreach (string i in arrId)
            {
                int prjid = Convert.ToInt32(i);
                var dv = Ql.DeviceOfProjects.Where(x => x.ProjectId == prjid && x.Status == 1).ToList();
                if (dv.Count() > 0)
                {
                    checkIsset = false;
                    break;
                }
            }
            if (checkIsset)
            {
                string a = "," + Id + ",";
                int checkdele = Ql.DeleteProject1(a);
                if (checkdele > 0)
                    result = true;
            }
            else
            {
                result = false;
            }
            return Json(result);
        }
        [HasCredential(RoleID = "DELETE_DEPARTMENT")]
        public JsonResult DeleteDepartment1(int Id)
        {
            bool result = false;
            var dv = Ql.DeviceOfProjects.Where(x => x.ProjectId == Id && x.Status == 1);
            if (dv.Count() == 0)
            {
                result = true;
                Ql.DeleteProject(Id);
            }
            else
            {
                result = false;
            }

            return Json(result);
        }
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public ActionResult AddDeviceInDepartment(int Id, int DeviceType)
        {
            ViewData["DeviceType"] = Ql.DeviceTypes.ToList();
            if (DeviceType != 0)
            {
                ViewData["Device"] = Ql.SearchDevice(0, DeviceType, null, null, null).AsEnumerable().Where(x => x.StatusRepair != 1).ToList();
            }
            else
            {
                ViewData["Device"] = Ql.SearchDevice(0, null, null, null, null).AsEnumerable().Where(x => x.StatusRepair != 1).ToList();
            }

            ViewData["DeviceOfProjectAll"] = Ql.DeviceOfProjectAll(Id).AsEnumerable().ToList();
            var lstType = Ql.DeviceTypes.ToList();
            var lstDeviceInProject = Ql.DeviceOfProjectAll(Id).AsEnumerable().ToList();
            var tempList = lstDeviceInProject.GroupBy(k => k.TypeOfDevice).ToList();
            var map = new Dictionary<string, int>();
            foreach (var i in tempList)
            {
                var typeName = lstType.FirstOrDefault(k => k.Id == i.Key).TypeName;
                map.Add(typeName, i.Count());
            }
            ViewData["CountingDeviceType"] = map;
            return View(Ql.ProjectDkcs.Find(Id));
        }
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public ActionResult AddDeviceInDepartment1(int Idpr, int Iddv, String Notes)
        {
            bool result = true;
            //Check xem thiết bị có phải là thiết bị con nằm trong
            var Check = Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == Iddv & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).Count();
            if (Check > 0)
            {
                result = false;
            }
            else
            {
                int checkdele = Ql.AddDeviceOfProject(Idpr, Iddv, Notes);
                if (checkdele > 0)
                    result = true;
            }
            return Json(result);
        }
        [HasCredential(RoleID = "RETURN_DEVICETODEPOT")]
        public ActionResult ReturnDeviceInDepartment(int Idpr, int Iddv, String notes)
        {
            bool result = true;
            var check = 0;
            //Check xem có phải là thiết bị con nằm trong 
            check += Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == Iddv & x.IsDeleted == false & x.TypeComponant == 1).Count();
            if (check > 0)
            {
                result = false;
            }
            else
            {
                int checkdele = Ql.ReturnDeviceOfProject(Idpr, Iddv, notes);
                if (checkdele > 0)
                    result = true;
            }
            return Json(result);
        }
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public JsonResult AddDeviceDepartmentAll(string Id, int PJ, String Nt)
        {
            bool result = true;
            var IdParent = Id.Split(',');
            var check = 0;
            for (int i = 0; i < IdParent.Length; i++)
            {
                var IdP = Convert.ToInt32(IdParent[i]);
                //Check thiết bị xem có phải là thiết bị con nằm trong
                check += Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == IdP & x.IsDeleted == false & x.TypeComponant == 1).Count();
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
                    var lstComponant = Ql.DeviceDevices.Where(x => x.DeviceCodeParents == IdP & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).ToList();
                    foreach (var item in lstComponant)
                    {
                        // Thêm thiết bị con vào DeviceOfProject
                        Ql.AddDeviceOfProject(PJ, Convert.ToInt32(item), "");
                    }
                }
                //Thêm thiết bị cha vào DeviceOfProject
                var lstId = Id.Split(',');
                for (int i = 0; i < lstId.Length; i++)
                {
                    if (!lstId[i].Equals(""))
                        Ql.AddDeviceOfProject(PJ, Convert.ToInt32(lstId[i]), Nt);
                }
                result = true;
            }
            return Json(result);
        }
        [HasCredential(RoleID = "RETURN_DEVICETODEPOT")]
        public JsonResult ReturnDeviceDepartmentAll(string Id, int PJ, string notes)
        {
            bool result = true;
            var lstId = Id.Split(',');
            var check = 0;
            for (int i = 0; i < lstId.Length; i++)
            {
                var IdP = Convert.ToInt32(lstId[i]);
                //Check thiết bị xem có phải là thiết bị con nằm trong
                check += Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == IdP & x.IsDeleted == false & x.TypeComponant == 1).Count();
            }
            if (check > 0)
            {
                result = false;
            }
            else
            {
                // Lấy danh sách thiết bị con từ danh sách thiết bị cha
                for (int i = 0; i < lstId.Length; i++)
                {
                    var IdP = Convert.ToInt32(lstId[i]);
                    var lstComponant = Ql.DeviceDevices.Where(x => x.DeviceCodeParents == IdP & x.IsDeleted == false & x.TypeComponant == 1).Select(x => x.DeviceCodeChildren).ToList();
                    foreach (var item in lstComponant)
                    {
                        // Trả thiết bị con về kho                      
                        Ql.ReturnDeviceOfProject(PJ, Convert.ToInt32(item), notes);
                    }
                }
                // var lstId = Id.Split(',');
                for (int i = 0; i < lstId.Length; i++)
                {
                    if (!lstId[i].Equals(""))
                        Ql.ReturnDeviceOfProject(PJ, Convert.ToInt32(lstId[i]), notes);
                }
                result = true;
            }
            return Json(result);
        }
        public ActionResult StatisticsDevicesInDepartment()
        {
            return View();
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


        public class NewConfig
        {
            // Auto-Initialized properties  
            public string DeviceCode { get; set; }
            public string DeviceName { get; set; }
            public string TypeName { get; set; }
            public string Configuration { get; set; }
            public string DateOfDelivery { get; set; }
            public string Notes { get; set; }
            public string StatusRepair { get; set; }
        }

        public string Get_Status(int? status)
        {
            var st = "";
            if (status == 1)
            {
                st = "Đang sửa";
            }
            return st;
        }
        [HasCredential(RoleID = "EXPORT_DV_IN_DEPARTMENT")]
        public JsonResult ExportToExcel(int? IdProject)
        {
            Ql.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var charts = Ql.DeviceOfProjectAll(IdProject)
                .AsEnumerable()
                .Select(i => new
                {
                    i.DeviceCode,
                    i.DeviceName,
                    i.TypeName,
                    i.Configuration,
                    i.DateOfDelivery,
                    i.StatusRepair,
                    i.Notes
                })
                .ToList();

            var model = new List<NewConfig>();
            foreach (var item in charts)
            {
                var plainConfig = HtmlToPlainText(item.Configuration);
                var statusRepair = Get_Status(item.StatusRepair);
                var deliveryDate = item.DateOfDelivery?.ToString("dd-MM-yyyy") ?? "";

                model.Add(new NewConfig
                {
                    DeviceCode = item.DeviceCode,
                    DeviceName = item.DeviceName,
                    TypeName = item.TypeName,
                    Configuration = plainConfig,
                    DateOfDelivery = deliveryDate,
                    Notes = item.Notes,
                    StatusRepair = statusRepair
                });
            }

            var fileName = $"Devices_{DateTime.Now:ddMMyyyyHHmmss}.xlsx";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "exports");
            var filePath = Path.Combine(folderPath, fileName);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Devices");

                ws.Cells[1, 1].Value = "Mã";
                ws.Cells[1, 2].Value = "Tên thiết bị";
                ws.Cells[1, 3].Value = "Loại";
                ws.Cells[1, 4].Value = "Cấu hình";
                ws.Cells[1, 5].Value = "Ngày bàn giao";
                ws.Cells[1, 6].Value = "Trạng thái sửa chữa";
                ws.Cells[1, 7].Value = "Ghi chú";

                for (int i = 0; i < model.Count; i++)
                {
                    var row = i + 2;
                    ws.Cells[row, 1].Value = model[i].DeviceCode;
                    ws.Cells[row, 2].Value = model[i].DeviceName;
                    ws.Cells[row, 3].Value = model[i].TypeName;
                    ws.Cells[row, 4].Value = model[i].Configuration;
                    ws.Cells[row, 5].Value = model[i].DateOfDelivery;
                    ws.Cells[row, 6].Value = model[i].StatusRepair;
                    ws.Cells[row, 7].Value = model[i].Notes;
                }

                package.SaveAs(new FileInfo(filePath));
            }

            var fileUrl = Url.Content($"~/exports/{fileName}");

            return Json(new
            {
                success = true,
                message = "Xuất Excel thành công",
                fileUrl = fileUrl
            });
        }

        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public ActionResult AddDeviceInDepartmentMachine(int Id)
        {
            return View(Ql.ProjectDkcs.Find(Id));
        }
        [HttpGet]
        [HasCredential(RoleID = "ADD_DEVICEINPROJECT")]
        public JsonResult GetDeviceInDepartmentMachine(string dvc)
        {
            Ql.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            int? Result = 0;
            var stt = Ql.Devices.Where(x => x.DeviceCode.Trim() == dvc).Select(x => x.Status).First();
            if (stt == 0)
            {
                var findDvCode = Ql.Devices.Where(x => x.DeviceCode.Trim() == dvc).Select(x => x.Id).First();
                var check = Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == findDvCode & x.IsDeleted == false).Count();
                if (check > 0)
                {
                    Result = 4;
                }
                else
                {
                    ViewBag.Scaner = Ql.SearchDevice(null, null, null, null, dvc).AsEnumerable().First();
                }
            }
            else
            { Result = stt; }
            var model = ViewBag.Scaner;
            var result = new { Result, model };
            return Json(result);
        }

        [HasCredential(RoleID = "RETURN_DEVICETODEPOT")]
        public ActionResult ReturnDeviceInDepartmentMachine(int Id)
        {
            return View(Ql.ProjectDkcs.Find(Id));
        }
        [HttpGet]
        [HasCredential(RoleID = "RETURN_DEVICETODEPOT")]
        public JsonResult GetReturnDeviceInDepartmentMachine(string dvc, int pjId)
        {
            Ql.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            int Result = 0;
            if (Ql.SearchDevice(null, null, null, pjId, null).AsEnumerable().Where(x => x.DeviceCode.Trim() == dvc).Count() == 1)
            {
                var findDvCode = Ql.Devices.Where(x => x.DeviceCode.Trim() == dvc).Select(x => x.Id).First();
                var check = Ql.DeviceDevices.Where(x => x.DeviceCodeChildren == findDvCode & x.IsDeleted == false).Count();
                if (check > 0)
                {
                    Result = 2;
                }
                else
                {
                    ViewBag.Scaner = Ql.SearchDevice(null, null, null, pjId, dvc).AsEnumerable().First();
                    Result = 3;
                }
            }
            else
            {
                Result = 1;
            }
            var model = ViewBag.Scaner;
            var result = new { Result, model };
            return Json(result);
        }

        [HasCredential(RoleID = "VIEW_STATISTICAL_DEPARTMENT")]
        public ActionResult StatisticDepartment()
        {
            ViewData["StatisticProject"] = Ql.StatisticProject().AsEnumerable().ToList();
            return View();
        }
    }
}
