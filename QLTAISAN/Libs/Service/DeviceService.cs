namespace Libs.Service
{
    public interface IDeviceService
    {

    }
    public class DeviceService : IDeviceService
    {
        private readonly UnitOfWork _unitOfWork;

        public DeviceService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<SearchDevice_Result>> SearchDeviceAsync(int? status, int? typeOfDevice, int? guarantee, int? project, string deviceCode)
        {
            // Giả sử trong context đã có method SearchDevice như bạn đã định nghĩa
            return await _unitOfWork.Context.SearchDevice(status, typeOfDevice, guarantee, project, deviceCode).ToListAsync();
        }
    }
}
