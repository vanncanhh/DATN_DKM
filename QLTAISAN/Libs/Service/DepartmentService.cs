namespace Libs.Service
{
    public interface IDepartmentService
    {
        void Save();
        Task<List<ProjectDkc>> GetDepartmentsAsync();
        Task<List<ProjectDkc>> SearchDepartmentAsync(string projectSymbol, int? managerProject);
        Task<bool> AddDepartmentAsync(ProjectDkc project);
        Task<ProjectDkc?> GetDepartmentByIdAsync(int id);
        Task<bool> EditDepartmentAsync(ProjectDkc project);
        Task<bool> DeleteDepartmentAsync(int id);
        Task<bool> AddDeviceToDepartmentAsync(int departmentId, int deviceId, string notes);
        Task<bool> RemoveDeviceFromDepartmentAsync(int departmentId, int deviceId, string notes);
    }
    public class DepartmentService : IDepartmentService
    {
        private QuanLyTaiSanCtyDATNContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentService(QuanLyTaiSanCtyDATNContext dbContext, IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        public void Save()
        {
            _unitOfWork.SaveChanges();
        }
        public async Task<List<ProjectDkc>> GetDepartmentsAsync()
        {
            return await _dbContext.ProjectDkcs.Where(d => d.IsDeleted == false).ToListAsync();
        }

        public async Task<List<ProjectDkc>> SearchDepartmentAsync(string projectSymbol, int? managerProject)
        {
            return await _dbContext.ProjectDkcs
                .Where(x => (string.IsNullOrEmpty(projectSymbol) || x.ProjectSymbol.Contains(projectSymbol))
                            && (!managerProject.HasValue || x.ManagerProject == managerProject))
                .ToListAsync();
        }

        public async Task<bool> AddDepartmentAsync(ProjectDkc project)
        {
            var exists = await _dbContext.ProjectDkcs.AnyAsync(x => x.ProjectSymbol.Trim() == project.ProjectSymbol.Trim());
            if (exists)
                return false; // Mã đã tồn tại

            _dbContext.ProjectDkcs.Add(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<ProjectDkc?> GetDepartmentByIdAsync(int id)
        {
            return await _dbContext.ProjectDkcs.FindAsync(id);
        }

        public async Task<bool> EditDepartmentAsync(ProjectDkc project)
        {
            var existingProject = await _dbContext.ProjectDkcs.FindAsync(project.Id);
            if (existingProject == null)
                return false;

            existingProject.ProjectSymbol = project.ProjectSymbol;
            existingProject.ProjectName = project.ProjectName;
            existingProject.ManagerProject = project.ManagerProject;
            existingProject.Address = project.Address;
            existingProject.CreatedDate = project.CreatedDate;
            existingProject.ModifiedDate = project.ModifiedDate;
            existingProject.Status = project.Status;
            existingProject.IsDeleted = project.IsDeleted;

            _dbContext.ProjectDkcs.Update(existingProject);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteDepartmentAsync(int id)
        {
            var project = await _dbContext.ProjectDkcs.FindAsync(id);
            if (project == null)
                return false;

            _dbContext.ProjectDkcs.Remove(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddDeviceToDepartmentAsync(int departmentId, int deviceId, string notes)
        {
            var deviceExists = await _dbContext.DeviceDevices.AnyAsync(x => x.DeviceCodeChildren == deviceId && x.IsDeleted == false && x.TypeComponant == 1);
            if (deviceExists)
                return false;

            var newDevice = new DeviceOfProject
            {
                ProjectId = departmentId,
                DeviceId = deviceId,
                Notes = notes
            };

            _dbContext.DeviceOfProjects.Add(newDevice);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveDeviceFromDepartmentAsync(int departmentId, int deviceId, string notes)
        {
            var deviceExists = await _dbContext.DeviceDevices.AnyAsync(x => x.DeviceCodeChildren == deviceId && x.IsDeleted == false && x.TypeComponant == 1);
            if (deviceExists)
                return false;

            var device = await _dbContext.DeviceOfProjects.FirstOrDefaultAsync(x => x.ProjectId == departmentId && x.DeviceId == deviceId);
            if (device == null)
                return false;

            _dbContext.DeviceOfProjects.Remove(device);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
