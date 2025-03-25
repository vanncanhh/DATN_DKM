namespace Libs.Service
{
    public interface IDeviceTypeService
    {
        Task<IEnumerable<DeviceType>> GetAllDeviceTypesAsync();
        Task<DeviceType?> GetDeviceTypeByIdAsync(int id);
        Task<bool> AddDeviceTypeAsync(string typeName, string typeSymbol, string notes);
        Task<bool> UpdateDeviceTypeAsync(int id, string typeName, string typeSymbol, string notes);
        Task<bool> DeleteDeviceTypeAsync(int id);
    }
    public class DeviceTypeService : IDeviceTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeviceTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DeviceType>> GetAllDeviceTypesAsync()
        {
            return await _unitOfWork.DeviceTypeRepository.GetAllAsync();
        }

        public async Task<DeviceType?> GetDeviceTypeByIdAsync(int id)
        {
            return await _unitOfWork.DeviceTypeRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddDeviceTypeAsync(string typeName, string typeSymbol, string notes)
        {
            var deviceType = new DeviceType
            {
                TypeName = typeName,
                TypeSymbol = typeSymbol,
                Notes = notes
            };
            await _unitOfWork.DeviceTypeRepository.AddAsync(deviceType);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateDeviceTypeAsync(int id, string typeName, string typeSymbol, string notes)
        {
            var deviceType = await _unitOfWork.DeviceTypeRepository.GetByIdAsync(id);
            if (deviceType == null) return false;

            deviceType.TypeName = typeName;
            deviceType.TypeSymbol = typeSymbol;
            deviceType.Notes = notes;

            _unitOfWork.DeviceTypeRepository.Update(deviceType);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDeviceTypeAsync(int id)
        {
            var deviceType = await _unitOfWork.DeviceTypeRepository.GetByIdAsync(id);
            if (deviceType == null) return false;

            var linkedDevices = await _unitOfWork.DeviceRepository.GetAllAsync();
            if (linkedDevices.Any(d => d.TypeOfDevice == id))
                return false;

            _unitOfWork.DeviceTypeRepository.Delete(deviceType);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
