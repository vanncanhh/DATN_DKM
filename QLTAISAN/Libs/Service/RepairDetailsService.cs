namespace Libs.Service
{
    public interface IRepairDetailsService
    {
        Task<IEnumerable<RepairDetail>> GetAllRepairDetailsAsync();
        Task<IEnumerable<RepairDetail>> SearchRepairDetailsAsync(int? repairType, int? userId, int? deviceId, int? status);
        Task<RepairDetail?> GetRepairDetailByIdAsync(int id);
        Task<bool> AddRepairDetailAsync(RepairDetail repairDetail);
        Task<bool> UpdateRepairDetailAsync(RepairDetail repairDetail);
        Task<bool> DeleteRepairDetailAsync(int id);
        Task<bool> DeleteRepairDetailsAsync(string id);
    }
    public class RepairDetailsService : IRepairDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RepairDetailsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RepairDetail>> GetAllRepairDetailsAsync()
        {
            return await _unitOfWork.RepairDetailRepository.GetAllAsync();
        }

        public async Task<IEnumerable<RepairDetail>> SearchRepairDetailsAsync(int? repairType, int? userId, int? deviceId, int? status)
        {
            var query = _unitOfWork.RepairDetailRepository.GetAllAsync().Result.AsQueryable();

            if (repairType.HasValue)
                query = query.Where(x => x.TypeOfRepair == repairType);

            if (userId.HasValue)
                query = query.Where(x => x.UserId == userId);

            if (deviceId.HasValue)
                query = query.Where(x => x.DeviceId == deviceId);

            if (status.HasValue)
                query = query.Where(x => x.Status == status);

            return await query.ToListAsync();
        }

        public async Task<RepairDetail?> GetRepairDetailByIdAsync(int id)
        {
            return await _unitOfWork.RepairDetailRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddRepairDetailAsync(RepairDetail repairDetail)
        {
            await _unitOfWork.RepairDetailRepository.AddAsync(repairDetail);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRepairDetailAsync(RepairDetail repairDetail)
        {
            var existingRepairDetail = await _unitOfWork.RepairDetailRepository.GetByIdAsync(repairDetail.Id);
            if (existingRepairDetail == null) return false;

            existingRepairDetail.DateOfRepair = repairDetail.DateOfRepair;
            existingRepairDetail.NextDateOfRepair = repairDetail.NextDateOfRepair;
            existingRepairDetail.TypeOfRepair = repairDetail.TypeOfRepair;
            existingRepairDetail.AddressOfRepair = repairDetail.AddressOfRepair;
            existingRepairDetail.UserId = repairDetail.UserId;
            existingRepairDetail.Notes = repairDetail.Notes;
            existingRepairDetail.Status = repairDetail.Status;
            existingRepairDetail.Price = repairDetail.Price;

            _unitOfWork.RepairDetailRepository.Update(existingRepairDetail);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRepairDetailAsync(int id)
        {
            var repairDetail = await _unitOfWork.RepairDetailRepository.GetByIdAsync(id);
            if (repairDetail == null) return false;

            _unitOfWork.RepairDetailRepository.Delete(repairDetail);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRepairDetailsAsync(string id)
        {
            var ids = id.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToList();

            foreach (var repairId in ids)
            {
                var repairDetail = await _unitOfWork.RepairDetailRepository.GetByIdAsync(repairId);
                if (repairDetail != null)
                {
                    _unitOfWork.RepairDetailRepository.Delete(repairDetail);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
