namespace QLTAISAN.Controllers
{
    public class DeviceTypeController : Controller
    {
        QuanLyTaiSanCtyDATNContext data = new QuanLyTaiSanCtyDATNContext();

        public ActionResult DeviceType()
        {
            return View(data.DeviceTypes.ToList());
        }
        [HttpPost]
        public ActionResult AddDeviceType(FormCollection collection)
        {
            string TypeName = collection["TypeName"];
            string Notes = collection["Notes"];
            string TypeSymbol = collection["TypeSymbol"];
            data.AddDeviceType(TypeName, TypeSymbol, Notes);
            return RedirectToAction("DeviceType", "DeviceType");
        }
        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            data.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var DeviceType = data.DeviceTypes.Find(id);
            return Json(new
            {
                data = DeviceType,
            });
        }
        [HttpPost]
        public JsonResult EditDeviceType(int Id, string TypeName, string TypeSymbol, string Notes)
        {
            bool result = true;
            data.UpdateDeviceType(Id, TypeName, TypeSymbol, Notes);
            return Json(result);
        }
        public JsonResult DeleteDeviceType(int Id)
        {
            bool result = false;
            var charts = data.SearchDevice(null, Id, null, null, null).ToList().Count();
            if (charts == 0)
            {
                int checkdele = data.DeleteDeviceType(Id);
                result = true;
            }
            else result = false;
            return Json(result);
        }
        //  [AuthorizationViewHandler]
        public ActionResult StatisticalDeviceType()
        {
            return View(data.DeviceTypes.ToList());
        }
    }
}
