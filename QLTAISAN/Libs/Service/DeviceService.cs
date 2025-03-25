namespace Libs.Service
{
    public interface IDeviceService
    {
        void Save();
        IQueryable<Device> getDevicesList();
        Device getDeviceById(int id);
        void insertDevice(Device device);
        void updateDevice(Device device);
        void deleteDevice(int id);
    }
    public class DeviceService : IDeviceService
    {
        private QuanLyTaiSanCtyDATNContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public DeviceService(IUnitOfWork unitOfWork, QuanLyTaiSanCtyDATNContext dbContext) 
        {
            _unitOfWork = unitOfWork;
            _dbContext = dbContext;
        }

        public void Save()
        {
            _unitOfWork.SaveChanges();
        }

        public IQueryable<Device> getDevicesList()
        {
            return _dbContext.Devices.AsQueryable();
        }

        public Device getDeviceById(int id)
        {
            return _unitOfWork.DeviceRepository.GetById(id);
        }

        public void insertDevice(Device device)
        {
            _unitOfWork.DeviceRepository.Add(device);
            Save();
        }

        public void updateDevice(Device device)
        {
            _unitOfWork.DeviceRepository.Update(device);
            Save();
        }

        public void deleteDevice(int id)
        {
            Device device = _unitOfWork.DeviceRepository.GetById(id);

            if (device != null)
            {
                _unitOfWork.DeviceRepository.Delete(device);

                Save();
            }
        }
    }
}
