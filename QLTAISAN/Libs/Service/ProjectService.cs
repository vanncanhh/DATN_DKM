namespace Libs.Service
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDkc>> GetAllProjectsAsync();
        Task<ProjectDkc?> GetProjectByIdAsync(int id);
        Task<bool> AddProjectAsync(ProjectDkc project);
        Task<bool> UpdateProjectAsync(ProjectDkc project);
        Task<bool> DeleteProjectAsync(int id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> AddDeviceToProjectAsync(int projectId, int deviceId, string notes);
        Task<bool> ReturnDeviceFromProjectAsync(int projectId, int deviceId, string notes);
    }
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ProjectDkc>> GetAllProjectsAsync()
        {
            return await _unitOfWork.ProjectDkcRepository.GetAllAsync();
        }

        public async Task<ProjectDkc?> GetProjectByIdAsync(int id)
        {
            return await _unitOfWork.ProjectDkcRepository.GetByIdAsync(id);
        }

        public async Task<bool> AddProjectAsync(ProjectDkc project)
        {
            await _unitOfWork.ProjectDkcRepository.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateProjectAsync(ProjectDkc project)
        {
            var existingProject = await _unitOfWork.ProjectDkcRepository.GetByIdAsync(project.Id);
            if (existingProject == null) return false;

            existingProject.ProjectName = project.ProjectName;
            existingProject.ProjectSymbol = project.ProjectSymbol;
            existingProject.Address = project.Address;
            existingProject.FromDate = project.FromDate;
            existingProject.ToDate = project.ToDate;
            existingProject.ManagerProject = project.ManagerProject;
            existingProject.Status = project.Status;
            existingProject.ModifiedDate = DateTime.Now;

            _unitOfWork.ProjectDkcRepository.Update(existingProject);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _unitOfWork.ProjectDkcRepository.GetByIdAsync(id);
            if (project == null) return false;

            _unitOfWork.ProjectDkcRepository.Delete(project);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.UserRepository.GetAllAsync();
        }

        public async Task<bool> AddDeviceToProjectAsync(int projectId, int deviceId, string notes)
        {
            var deviceProject = new DeviceOfProject
            {
                ProjectId = projectId,
                DeviceId = deviceId,
                Notes = notes
            };
            await _unitOfWork.DeviceOfProjectRepository.AddAsync(deviceProject);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReturnDeviceFromProjectAsync(int projectId, int deviceId, string notes)
        {
            var deviceProject = await _unitOfWork.DeviceOfProjectRepository.GetByIdAsync(deviceId);
            if (deviceProject == null) return false;

            _unitOfWork.DeviceOfProjectRepository.Delete(deviceProject);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
