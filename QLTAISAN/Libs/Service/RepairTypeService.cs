namespace Libs.Service
{
    public interface IRepairTypeService
    {
        Task<IEnumerable<RepairType>> GetAllRepairTypesAsync();
        Task<RepairType?> GetRepairTypeByIdAsync(int id);
        Task<bool> AddRepairTypeAsync(string typeName, string notes);
        Task<bool> EditRepairTypeAsync(int id, string typeName, string notes);
        Task<bool> DeleteRepairTypeAsync(int id);
    }
    public class RepairTypeService : IRepairTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RepairTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RepairType>> GetAllRepairTypesAsync()
        {
            return await _unitOfWork.RepairTypeRepository.GetAllAsync();
        }

        public async Task<RepairType?> GetRepairTypeByIdAsync(int id)
        {
            return await _unitOfWork.RepairTypeRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddRepairTypeAsync(string typeName, string notes)
        {
            var repairType = new RepairType
            {
                TypeName = typeName,
                Notes = notes
            };
            await _unitOfWork.RepairTypeRepository.AddAsync(repairType);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditRepairTypeAsync(int id, string typeName, string notes)
        {
            var repairType = await _unitOfWork.RepairTypeRepository.GetByIdAsync(id);
            if (repairType == null) return false;

            repairType.TypeName = typeName;
            repairType.Notes = notes;

            _unitOfWork.RepairTypeRepository.Update(repairType);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRepairTypeAsync(int id)
        {
            var repairDetails = await _unitOfWork.RepairDetailRepository.GetAllAsync();
            var isReferenced = repairDetails.Any(rd => rd.TypeOfRepair == id);

            if (isReferenced) return false;

            var repairType = await _unitOfWork.RepairTypeRepository.GetByIdAsync(id);
            if (repairType == null) return false;

            _unitOfWork.RepairTypeRepository.Delete(repairType);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
