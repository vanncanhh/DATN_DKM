namespace Libs.Service
{
    public interface IHomeService
    {
        Task<Dictionary<string, int>> GetStatisticsAsync();
    }
    public class HomeService : IHomeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public HomeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Dictionary<string, int>> GetStatisticsAsync()
        {
            var statistics = new Dictionary<string, int>();

            int countDevice = (await _unitOfWork.DeviceRepository.GetAllAsync()).Where(x => x.Status != 2).Count();
            statistics.Add("CountDevice", countDevice);

            int deviceLiquidation = (await _unitOfWork.DeviceRepository.GetAllAsync()).Where(x => x.Status == 2).Count();
            statistics.Add("Deviceliquidation", deviceLiquidation);

            int deviceType = (await _unitOfWork.DeviceTypeRepository.GetAllAsync()).Count();
            statistics.Add("DeviceType", deviceType);

            int project = (await _unitOfWork.ProjectDkcRepository.GetAllAsync()).Where(x => x.IsDeleted == false && x.TypeProject == 1).Count();
            statistics.Add("Project", project);

            int user = (await _unitOfWork.UserRepository.GetAllAsync()).Where(x => x.IsDeleted == false).Count();
            statistics.Add("User", user);

            int requestDevice = (await _unitOfWork.RequestDeviceRepository.GetAllAsync()).Count();
            statistics.Add("RequestDevice", requestDevice);

            return statistics;
        }
    }
}
