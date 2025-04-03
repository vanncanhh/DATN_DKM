namespace QLTAISAN.Controllers
{
    //[HasCredential(RoleID = "")]
    public class ProjectController : Controller
    {
        private readonly QuanLyTaiSanCtyDATNContext Ql;

        public ProjectController(QuanLyTaiSanCtyDATNContext context)
        {
            Ql = context;
        }
        public ActionResult Project()
        {
            ViewBag.ProjectNb = Ql.SearchProject(null, null, 0, null).AsEnumerable().Count();
            ViewData["User"] = Ql.Users.ToList();
            var lstProject = Ql.SearchProject(null, null, 0, null).AsEnumerable().ToList();
            return View(lstProject);
        }
        [HttpPost]
        public ActionResult SeachProject(FormCollection colection, ProjectDkc Project)
        {
            ViewData["User"] = Ql.Users.ToList();
            String ProjectSymbol = colection["ProjectSymbol"].ToString().Trim();
            int? Status = colection["Status"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Status"]);
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            var lstProject = Ql.SearchProject(ManagerProject, Status, 0, ProjectSymbol).AsEnumerable().ToList();
            var ViewProject = lstProject;
            ViewBag.Status = Status;
            ViewBag.ManagerProject = ManagerProject;
            ViewBag.ProjectSymbol = ProjectSymbol;
            ViewBag.ProjectNb = Ql.SearchProject(ManagerProject, Status, 0, ProjectSymbol).AsEnumerable().Count();
            return View("Project", ViewProject);
        }

        public ActionResult AddProject()
        {
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            return View();
        }
        [HttpPost]
        public JsonResult AddRepairType(string TypeName, string Notes)
        {
            Ql.AddRepairType(TypeName, Notes);
            bool result = true;
            return Json(result);
        }

        [HttpPost]
        public ActionResult AddProject(FormCollection colection, ProjectDkc Project)
        {

            String ProjectSymbol = colection["ProjectSymbol"];
            String ProjectName = colection["ProjectName"];
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            String Address = colection["Address"];
            DateTime? FromDate = colection["FromDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["FromDate"]);
            DateTime? ToDate = colection["ToDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["ToDate"]);
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
                ViewBag.FromDate = @String.Format("{0:yyyy-MM-dd}", FromDate);
                ViewBag.ToDate = @String.Format("{0:yyyy-MM-dd}", ToDate);
                ViewBag.Status = Status;
                ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
                return View();
            }
            else
            {
                Ql.AddProject_(ProjectSymbol, ProjectName, ManagerProject, Address, FromDate, ToDate, Status);
                return RedirectToAction("Project", "Project");
            }
        }

        public ActionResult EditProject(int Id)
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
            ViewData["User"] = Ql.Users.Where(x => x.Status != 1 && x.IsDeleted != true).ToList();
            return View(Ql.ProjectDkcs.Find(Id));
        }

        [HttpPost]
        public ActionResult EditProject(FormCollection colection, ProjectDkc Project)
        {
            int? Id = colection["Id"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Id"]);
            String ProjectSymbol = colection["ProjectSymbol"];
            String ProjectName = colection["ProjectName"];
            int? ManagerProject = colection["ManagerProject"].Equals("0") ? (int?)null : Convert.ToInt32(colection["ManagerProject"]);
            String Address = colection["Address"];
            DateTime? FromDate = colection["FromDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["FromDate"]);
            DateTime? ToDate = colection["ToDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["ToDate"]);
            DateTime? CreatedDate = colection["CreatedDate"].Equals("") ? (DateTime?)null : Convert.ToDateTime(colection["CreatedDate"]);
            DateTime ModifiedDate = Convert.ToDateTime(colection["ModifiedDate"]);
            int? Status = colection["Status"].Equals("0") ? (int?)null : Convert.ToInt32(colection["Status"]);
            bool IsDeleted = Convert.ToBoolean(colection["IsDeleted"]);
            Ql.UpdateProject(Id, ProjectSymbol, ProjectName, ManagerProject, Address, FromDate, ToDate, CreatedDate, ModifiedDate, Status, IsDeleted);
            return RedirectToAction("EditProject", "Project");
        }

        public JsonResult DeleteProject(string Id)
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

        public JsonResult DeleteProject1(int Id)
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

        public ActionResult AddDeviceInProject(int Id, int DeviceType)
        {
            ViewData["DeviceType"] = Ql.DeviceTypes.ToList();
            if (DeviceType != 0)
            {
                ViewData["Device"] = Ql.SearchDevice(0, DeviceType, null, null, null).Where(x => x.StatusRepair != 1).ToList();
            }
            else
            {
                ViewData["Device"] = Ql.SearchDevice(0, null, null, null, null).Where(x => x.StatusRepair != 1).ToList();
            }

            ViewData["DeviceOfProjectAll"] = Ql.DeviceOfProjectAll(Id).ToList();
            var lstType = Ql.DeviceTypes.ToList();
            var lstDeviceInProject = Ql.DeviceOfProjectAll(Id).ToList();
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

        public ActionResult AddDeviceInProject1(int Idpr, int Iddv, String Notes)
        {
            bool result = false;
            int checkdele = Ql.AddDeviceOfProject(Idpr, Iddv, Notes);
            if (checkdele > 0)
                result = true;
            return Json(result);
        }

        public ActionResult ReturnDeviceInProject(int Idpr, int Iddv, string notes)
        {
            bool result = false;
            int checkdele = Ql.ReturnDeviceOfProject(Idpr, Iddv, notes);
            if (checkdele > 0)
                result = true;
            return Json(result);
        }

        public JsonResult AddDeviceProjectAll(string Id, int PJ)
        {

            var lstId = Id.Split(',');
            for (int i = 0; i < lstId.Length; i++)
            {
                if (!lstId[i].Equals(""))
                    Ql.AddDeviceOfProject(PJ, Convert.ToInt32(lstId[i]), "");
            }
            bool result = true;
            return Json(result);
        }

        public JsonResult ReturnDeviceProjectAll(string Id, int PJ, string notes)
        {
            var lstId = Id.Split(',');
            for (int i = 0; i < lstId.Length; i++)
            {
                if (!lstId[i].Equals(""))
                    Ql.ReturnDeviceOfProject(PJ, Convert.ToInt32(lstId[i]), notes);
            }
            bool result = true;
            return Json(result);
        }
        public ActionResult StatisticsDevicesInProject()
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
            public string DeviceCode { get; set; }
            public string DeviceName { get; set; }
            public string TypeName { get; set; }
            public string Configuration { get; set; }
            public string Notes { get; set; }
            public string Status { get; set; }
            public string DateOfDelivery { get; set; }
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

        public JsonResult ExportToExcel(int? IdProject)
        {
            Ql.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            var charts = Ql.DeviceOfProjectAll(IdProject)
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
                    Status = statusRepair
                });
            }

            // Đặt tên file và đường dẫn thư mục
            var fileName = $"Devices_{DateTime.Now:ddMMyyyyHHmmss}.xlsx";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "exports");
            var filePath = Path.Combine(folderPath, fileName);

            // Tạo thư mục nếu chưa có
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Sử dụng EPPlus để tạo file Excel
            using (var package = new ExcelPackage())
            {
                var ws = package.Workbook.Worksheets.Add("Devices");

                // Thêm header cho Excel
                ws.Cells[1, 1].Value = "Mã";
                ws.Cells[1, 2].Value = "Tên thiết bị";
                ws.Cells[1, 3].Value = "Loại";
                ws.Cells[1, 4].Value = "Cấu hình";
                ws.Cells[1, 5].Value = "Ngày bàn giao";
                ws.Cells[1, 6].Value = "Trạng thái sửa chữa";
                ws.Cells[1, 7].Value = "Ghi chú";

                // Điền dữ liệu vào các hàng trong Excel
                for (int i = 0; i < model.Count; i++)
                {
                    var row = i + 2; // Bắt đầu từ hàng thứ 2 (hàng đầu tiên là header)
                    ws.Cells[row, 1].Value = model[i].DeviceCode;
                    ws.Cells[row, 2].Value = model[i].DeviceName;
                    ws.Cells[row, 3].Value = model[i].TypeName;
                    ws.Cells[row, 4].Value = model[i].Configuration;
                    ws.Cells[row, 5].Value = model[i].DateOfDelivery;
                    ws.Cells[row, 6].Value = model[i].Status;
                    ws.Cells[row, 7].Value = model[i].Notes;
                }

                // Lưu file Excel vào đĩa
                package.SaveAs(new FileInfo(filePath));
            }

            // Đường dẫn file mà người dùng có thể tải về
            var fileUrl = Url.Content($"~/exports/{fileName}");

            // Trả về JSON chứa URL file
            return Json(new
            {
                success = true,
                message = "Xuất Excel thành công",
                fileUrl = fileUrl
            });
        }

        public ActionResult AddDeviceInProjectMachine(int Id)
        {
            return View(Ql.ProjectDkcs.Find(Id));
        }
        [HttpGet]
        public JsonResult GetDeviceInProjectMachine(string dvc)
        {
            Ql.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            bool Result = false;
            if (Ql.SearchDevice(null, null, null, null, null).Where(x => x.DeviceCode.Trim() == dvc).Count() == 1)
            {
                ViewBag.Scaner = Ql.SearchDevice(null, null, null, null, dvc).First();
                Result = true;
            }
            else Result = false;
            var model = ViewBag.Scaner;
            var result = new { Result, model };
            return Json(result);
        }

        public ActionResult ReturnDeviceInProjectMachine(int Id)
        {
            return View(Ql.ProjectDkcs.Find(Id));
        }
        [HttpGet]
        public JsonResult GetReturnDeviceInProjectMachine(string dvc, int pjId)
        {
            Ql.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            bool Result = false;
            if (Ql.SearchDevice(null, null, null, pjId, null).Where(x => x.DeviceCode.Trim() == dvc).Count() == 1)
            {
                ViewBag.Scaner = Ql.SearchDevice(null, null, null, pjId, dvc).First();
                Result = true;
            }
            else Result = false;
            var model = ViewBag.Scaner;
            var result = new { Result, model };
            return Json(result);
        }

        public ActionResult StatisticProject()
        {
            ViewData["StatisticProject"] = Ql.StatisticProject().ToList();
            return View();

        }
    }
}
