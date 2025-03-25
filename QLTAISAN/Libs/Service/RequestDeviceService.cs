namespace Libs.Service
{
    public interface IRequestDeviceService
    {
        Task<IEnumerable<RequestDevice>> GetAllRequestDevicesAsync();
        Task<IEnumerable<RequestDevice>> SearchRequestDevicesAsync(int? status);
        Task<RequestDevice?> GetRequestDeviceByIdAsync(int id);
        Task<bool> AddRequestDeviceAsync(RequestDevice requestDevice);
        Task<bool> UpdateRequestDeviceAsync(RequestDevice requestDevice);
        Task<bool> DeleteRequestDeviceAsync(string id);
        Task<bool> AddDeviceTypeAsync(string typeName, string typeSymbol, string notes);
    }
    public class RequestDeviceService : IRequestDeviceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RequestDeviceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RequestDevice>> GetAllRequestDevicesAsync()
        {
            return await _unitOfWork.RequestDeviceRepository.GetAllAsync();
        }

        public async Task<IEnumerable<RequestDevice>> SearchRequestDevicesAsync(int? status)
        {
            var query = _unitOfWork.RequestDeviceRepository.GetAllAsync().Result.AsQueryable();

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            return await query.ToListAsync();
        }

        public async Task<RequestDevice?> GetRequestDeviceByIdAsync(int id)
        {
            return await _unitOfWork.RequestDeviceRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddRequestDeviceAsync(RequestDevice requestDevice)
        {
            await _unitOfWork.RequestDeviceRepository.AddAsync(requestDevice);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRequestDeviceAsync(RequestDevice requestDevice)
        {
            var existingDevice = await _unitOfWork.RequestDeviceRepository.GetByIdAsync(requestDevice.Id);
            if (existingDevice == null) return false;

            existingDevice.DeviceName = requestDevice.DeviceName;
            existingDevice.DateOfRequest = requestDevice.DateOfRequest;
            existingDevice.DateOfUse = requestDevice.DateOfUse;
            existingDevice.TypeOfDevice = requestDevice.TypeOfDevice;
            existingDevice.Configuration = requestDevice.Configuration;
            existingDevice.Notes = requestDevice.Notes;
            existingDevice.Status = requestDevice.Status;
            existingDevice.Approved = requestDevice.Approved;
            existingDevice.NumDevice = requestDevice.NumDevice;
            existingDevice.NoteProcess = requestDevice.NoteProcess;
            existingDevice.NoteReasonRefuse = requestDevice.NoteReasonRefuse;
            existingDevice.NameUserApproved = requestDevice.NameUserApproved;

            _unitOfWork.RequestDeviceRepository.Update(existingDevice);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRequestDeviceAsync(string id)
        {
            var requestDevice = await _unitOfWork.RequestDeviceRepository.GetByIdAsync(Convert.ToInt32(id));
            if (requestDevice == null) return false;

            _unitOfWork.RequestDeviceRepository.Delete(requestDevice);
            await _unitOfWork.SaveChangesAsync();
            return true;
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
    }
}
