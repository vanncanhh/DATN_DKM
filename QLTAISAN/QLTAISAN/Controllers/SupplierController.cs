namespace QLTAISAN.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // Lấy danh sách tất cả các nhà cung cấp
        public async Task<IActionResult> Supplier()
        {
            var suppliers = await _supplierService.GetAllSuppliersAsync();
            ViewData["Supplier"] = suppliers;
            return View(suppliers);
        }

        // Hiển thị trang chỉnh sửa nhà cung cấp
        public async Task<IActionResult> EditSupplier(int id)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            // Lấy danh sách thiết bị liên quan
            var devices = await _supplierService.GetAllSuppliersAsync();
            ViewData["sDevice"] = devices.Where(x => x.Id == id).ToList();

            return View(supplier);
        }

        // Cập nhật thông tin nhà cung cấp
        [HttpPost]
        public async Task<IActionResult> EditSupplier(int id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                return BadRequest("ID không khớp.");
            }

            var result = await _supplierService.UpdateSupplierAsync(supplier);
            if (result)
            {
                return RedirectToAction("Supplier");
            }
            return View(supplier);
        }

        // Thêm mới nhà cung cấp
        [HttpPost]
        public async Task<IActionResult> AddSupplier(Supplier supplier)
        {
            var result = await _supplierService.AddSupplierAsync(supplier);
            if (result)
            {
                return RedirectToAction("Supplier");
            }
            return View(supplier);
        }

        // Xóa nhà cung cấp
        public async Task<JsonResult> DeleteSupplier(int id)
        {
            var result = await _supplierService.DeleteSupplierAsync(id);
            return Json(new { success = result });
        }
    }
}
