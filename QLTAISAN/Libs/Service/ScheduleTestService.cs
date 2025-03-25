namespace Libs.Service
{
    public interface IScheduleTestService
    {
        Task<IEnumerable<ScheduleTest>> GetAllScheduleTestsAsync();
        Task<IEnumerable<ScheduleTest>> SearchScheduleTestAsync(int? userId, int? status);
        Task<ScheduleTest?> GetScheduleTestByIdAsync(int id);
        Task<bool> AddScheduleTestAsync(ScheduleTest scheduleTest);
        Task<bool> UpdateScheduleTestAsync(ScheduleTest scheduleTest);
        Task<bool> DeleteScheduleTestAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<Device>> GetAllDevicesAsync();
    }

    public class ScheduleTestService : IScheduleTestService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleTestService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ScheduleTest>> GetAllScheduleTestsAsync()
        {
            return await _unitOfWork.ScheduleTestRepository.GetAllAsync();
        }

        public async Task<IEnumerable<ScheduleTest>> SearchScheduleTestAsync(int? userId, int? status)
        {
            var query = _unitOfWork.ScheduleTestRepository.GetAllAsync().Result.AsQueryable();

            if (userId.HasValue)
                query = query.Where(x => x.UserTest == userId);

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            return await query.ToListAsync();
        }

        public async Task<ScheduleTest?> GetScheduleTestByIdAsync(int id)
        {
            return await _unitOfWork.ScheduleTestRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddScheduleTestAsync(ScheduleTest scheduleTest)
        {
            await _unitOfWork.ScheduleTestRepository.AddAsync(scheduleTest);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateScheduleTestAsync(ScheduleTest scheduleTest)
        {
            var existingSchedule = await _unitOfWork.ScheduleTestRepository.GetByIdAsync(scheduleTest.Id);
            if (existingSchedule == null) return false;

            existingSchedule.DeviceId = scheduleTest.DeviceId;
            existingSchedule.DateOfTest = scheduleTest.DateOfTest;
            existingSchedule.NextDateOfTest = scheduleTest.NextDateOfTest;
            existingSchedule.CategoryTest = scheduleTest.CategoryTest;
            existingSchedule.UserTest = scheduleTest.UserTest;
            existingSchedule.Notes = scheduleTest.Notes;
            existingSchedule.Status = scheduleTest.Status;

            _unitOfWork.ScheduleTestRepository.Update(existingSchedule);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteScheduleTestAsync(int id)
        {
            var scheduleTest = await _unitOfWork.ScheduleTestRepository.GetByIdAsync(id);
            if (scheduleTest == null) return false;

            _unitOfWork.ScheduleTestRepository.Delete(scheduleTest);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.UserRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Device>> GetAllDevicesAsync()
        {
            return await _unitOfWork.DeviceRepository.GetAllAsync();
        }
    }
}
