namespace QLTAISAN.Controllers
{
    public class SupplierController : Controller
    {
        QuanLyTaiSanCtyDATNContext data = new QuanLyTaiSanCtyDATNContext();
        public ActionResult Supplier()
        {
            ViewData["Supplier"] = data.Suppliers.ToList();
            return View(data.Suppliers.ToList());
        }
        //   [AuthorizationViewHandler]
        public ActionResult EditSupplier(int Id)
        {
            ViewData["sDevice"] = data.SearchDevice(null, null, null, null, null).AsEnumerable().Where(x => x.SupplierId == Id).ToList();
            return View(data.Suppliers.Find(Id));
        }
        [HttpPost]
        public ActionResult EditSupplier(IFormCollection collection)
        {
            int Id = Convert.ToInt32(collection["Id"]);
            string Name = collection["Name"];
            string Email = collection["Email"];
            string PhoneNumber = collection["PhoneNumber"];
            string Address = collection["Address"];
            string Support = collection["Support"];
            int Status = Convert.ToInt32(collection["Status"]);
            data.UpdateSupplier(Id, Name, Email, PhoneNumber, Address, Support, Status);
            return RedirectToAction("Supplier", "Supplier");
        }
        [HttpPost]
        public ActionResult AddSupplier(IFormCollection collection)
        {
            string Name = collection["Name"];
            string Email = collection["Email"];
            string PhoneNumber = collection["PhoneNumber"];
            string Address = collection["Address"];
            string Support = collection["Position"];
            data.AddSupplier(Name, Email, PhoneNumber, Address, Support);
            return RedirectToAction("Supplier", "Supplier");
        }
        public JsonResult DeleteSupplier(int Id)
        {
            bool result = false;
            var charts = data.SearchDevice(null, null, null, null, null).AsEnumerable().Where(x => x.SupplierId == Id).ToList();
            if (charts.Count == 0)
            {
                data.DeleteSupplier(Id);
                result = true;
            }
            else result = false;
            return Json(result);
        }
    }
}
