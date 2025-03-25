namespace Libs.Service
{
    public interface IEmployeeService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> SearchUsersAsync(int status);
        Task<bool> AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<bool> AddRoleAsync(string roleName, string notes);
        Task<bool> UpdateRoleAsync(int id, string roleName, string notes);
        Task<bool> DeleteRoleAsync(int id);
    }
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _unitOfWork.UserRepository.GetAllAsync();
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(int status)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return users.Where(x => x.Status == status).ToList();
        }

        public async Task<bool> AddUserAsync(User user)
        {
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null) return false;

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _unitOfWork.RoleRepository.GetAllAsync();
        }

        public async Task<bool> AddRoleAsync(string roleName, string notes)
        {
            var role = new Role
            {
                Name = roleName
            };
            await _unitOfWork.RoleRepository.AddAsync(role);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRoleAsync(int id, string roleName, string notes)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null) return false;

            role.Name = roleName;

            _unitOfWork.RoleRepository.Update(role);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(id);
            if (role == null) return false;

            _unitOfWork.RoleRepository.Delete(role);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
