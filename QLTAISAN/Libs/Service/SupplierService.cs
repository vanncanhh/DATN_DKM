namespace Libs.Service
{
    public interface ISupplierService
    {
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<Supplier?> GetSupplierByIdAsync(int id);
        Task<bool> UpdateSupplierAsync(Supplier supplier);
        Task<bool> AddSupplierAsync(Supplier supplier);
        Task<bool> DeleteSupplierAsync(int id);
    }
    public class SupplierService : ISupplierService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupplierService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _unitOfWork.SupplierRepository.GetAllAsync();
        }

        public async Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return await _unitOfWork.SupplierRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateSupplierAsync(Supplier supplier)
        {
            var existingSupplier = await _unitOfWork.SupplierRepository.GetByIdAsync(supplier.Id);
            if (existingSupplier == null) return false;

            existingSupplier.Name = supplier.Name;
            existingSupplier.Email = supplier.Email;
            existingSupplier.Phone = supplier.Phone;
            existingSupplier.Address = supplier.Address;
            existingSupplier.Support = supplier.Support;

            _unitOfWork.SupplierRepository.Update(existingSupplier);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSupplierAsync(Supplier supplier)
        {
            await _unitOfWork.SupplierRepository.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            var supplier = await _unitOfWork.SupplierRepository.GetByIdAsync(id);
            if (supplier == null) return false;

            // Kiểm tra xem có thiết bị nào liên quan không
            var relatedDevices = await _unitOfWork.DeviceRepository.GetAllAsync();
            if (relatedDevices.Any(d => d.SupplierId == id))
                return false;

            _unitOfWork.SupplierRepository.Delete(supplier);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
