namespace QLTAISAN.Controllers
{
    public class RepairTypeController : Controller
    {
        private readonly QuanLyTaiSanCtyDATNContext Ql;

        public RepairTypeController(QuanLyTaiSanCtyDATNContext ql)
        {
            Ql = ql;
        }

        public ActionResult RepairType()
        {
            ViewData["RepairTypes"] = Ql.RepairTypes.ToList();
            return View();
        }

        [HttpGet]
        public JsonResult GetDetail(int id)
        {
            Ql.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            var RepairTypes = Ql.RepairTypes.Find(id);
            return Json(new
            {
                data = RepairTypes,
            } );
        }

        public ActionResult AddRepairType()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddRepairType(IFormCollection colection, RepairType RepairType)
        {
            String Notes = colection["Notes"];
            String TypeName = colection["TypeName"];
            Ql.AddRepairType(TypeName, Notes);
            return RedirectToAction("RepairType", "RepairType");
        }

        public ActionResult EditRepairType(int Id)
        {
            return View(Ql.RepairTypes.Find(Id));
        }
        [HttpPost]
        public ActionResult EditRepairType(int Id, String TypeName, String Notes)
        {
            bool result = false;
            int checkdele = Ql.EditRepairType(Id, TypeName, Notes);
            if (checkdele > 0)
                result = true;
            return Json(result );
        }

        public ActionResult DeleteRepairType(int Id)
        {
            bool result = false;
            var dv = Ql.RepairDetails.Where(x => x.TypeOfRepair == Id);
            if (dv.Count() > 0)
            {
                result = false;
            }
            else
            {
                string a = "," + Id + ",";
                int checkdele = Ql.DeleteRepairType(a);
                if (checkdele > 0)
                    result = true;
            }
            return Json(result);
        }
    }
}
