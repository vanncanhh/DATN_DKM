using Libs.Helper;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace QLTAISAN.Controllers
{
    [Authorize]
    public class AuthenticationController : Controller
    {
        private readonly UserAuthorizationDatabseAction _dbContext;
        private readonly ApplicationDbContext _applicationDbContext;

        // Inject UserAuthorizationContext vào constructor của controller
        public AuthenticationController(UserAuthorizationDatabseAction dbContext, ApplicationDbContext db)
        {
            _dbContext = dbContext; 
             _applicationDbContext = db;  
        }
        // GET: Authentication
        public ActionResult Role()
        {
            ViewData["Role"] = _dbContext.GetAllRole();
            var feature = _dbContext.GetAllFeatureRecords();
            return View(feature);
        }
        [HttpPost]
        public ActionResult Adduserauthorization(FormCollection collection)
        {
            SystemFeature feature = new SystemFeature
            {
                Name = collection["Name"],
                ControllerName = collection["ControllerName"],
                ActionName = collection["ActionName"],
                RoleName = ""
            };
            _dbContext.AddNewFeature(feature);
            return RedirectToAction("Role", "Authentication");
        }
        public JsonResult DeleteAuthentication(List<int> Id)
        {
            _dbContext.DeleteListFeature(Id);
            var result = true;
            return Json(result);
        }

        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            var userauthorization = _dbContext.GetFeatureById(id);
            return Json(new { data = userauthorization });
        }
        [HttpPost]
        public JsonResult Editauthorization(int Id, string Name, string RoleName, string ControllerName, string ActionName)
        {
            var feature = new SystemFeature
            {
                Id = Id,
                Name = Name,
                ControllerName = ControllerName,
                ActionName = ActionName
            };
            var result = _dbContext.UpdateFeature(feature) > 0;
            return Json(result);
        }
        public ActionResult RoleAuthentication(string roleId, int controllerId = -1, string actionText = "")
        {
            var roleName = _dbContext.GetRoleById(roleId).Name;
            ViewData["RoleId"] = roleId;
            ViewData["RoleName"] = roleName;
            ViewData["ActionText"] = actionText;
            ViewData["Controller"] = controllerId;

            var records = _dbContext.GetAllFeatureRecords();

            if (controllerId > 0)
            {
                string controllerName = Enum.GetName(typeof(eStatus.ControllerId), controllerId);
                records = records.Where(k => k.ControllerName.EqualsIgnoringCase(controllerName)).ToList();
            }

            if (!string.IsNullOrEmpty(actionText.Trim()))
            {
                records = records.Where(k => k.ActionName.Contains(actionText) || k.Name.Contains(actionText)).ToList();
            }

            ViewData["FeatureList"] = records;
            var dict = new Dictionary<int, string>();

            foreach (var name in Enum.GetNames(typeof(eStatus.ControllerId)))
            {
                var value = (int)Enum.Parse(typeof(eStatus.ControllerId), name);
                dict.Add(value, value.GetEnumDescription(typeof(eStatus.ControllerId)));
            }

            ViewData["ControllerList"] = dict;

            var lstAvailableFeatureId = _dbContext.GetFeaturesByRoleName(roleName).Select(k => k.Id);
            var json = JsonSerializer.Serialize(lstAvailableFeatureId.ToList());
            ViewData["AvailableFeatureId"] = json;

            return View();
        }

        [HttpPost]
        public JsonResult ChangeRole(int[] lstId, string roleName)
        {
            var deleteResult = _dbContext.DeleteUserAuthorizationByRoleName(roleName);
            if (!deleteResult) return Json(new { status = false });

            var result = _dbContext.AddRangeUserAuthorization(lstId, roleName);
            return Json(result ? new { status = true } : new { status = false });
        }

        [HttpPost]
        public JsonResult AddNewRole(string id, string name)
        {
            if (id.Equals("-1"))
            {
                Guid guidId = Guid.NewGuid();
                var sqlInsert = @"INSERT INTO [AspNetRoles] VALUES(@Id, @Name)";
                _applicationDbContext.Database.ExecuteSqlRaw(sqlInsert,
                    new SqlParameter("@Id", guidId.ToString().ToLower()),
                    new SqlParameter("@Name", name));
            }
            else
            {
                var existName = _applicationDbContext.Set<string>()
                                .FromSqlRaw(@"SELECT Name FROM [AspNetRoles] WHERE Id = @Id", new SqlParameter("@Id", id))
                                .ToList();
                if (string.IsNullOrEmpty(existName.FirstOrDefault()))
                {
                    return Json(new { status = false });
                }

                var sqlUpdate = @"UPDATE [AspNetRoles] SET Name = @Name WHERE Id = @Id";
                _applicationDbContext.Database.ExecuteSqlRaw(sqlUpdate,
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Name", name));

                _dbContext.UpdateUserAuthorizationByRoleName(existName.FirstOrDefault(), name);
            }
            return Json(new { status = true });
        }
    }
}
