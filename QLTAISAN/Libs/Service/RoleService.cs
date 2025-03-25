namespace Libs.Service
{
    public interface IRoleService
    {
        Task<IEnumerable<UserGroup>> GetAllUserGroupsAsync();
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<bool> AddUserGroupAsync(string id, string name);
        Task<bool> DeleteUserGroupAsync(string id);
        Task<List<string>> GetRolesForGroupAsync(string groupId);
        Task<bool> AddRoleForGroupAsync(string roleId, string groupId);
    }
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserGroup>> GetAllUserGroupsAsync()
        {
            return await _unitOfWork.UserGroupRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _unitOfWork.RoleRepository.GetAllAsync();
        }

        public async Task<bool> AddUserGroupAsync(string id, string name)
        {
            var userGroup = new UserGroup
            {
                Id = id,
                Name = name
            };

            await _unitOfWork.UserGroupRepository.AddAsync(userGroup);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserGroupAsync(string id)
        {
            var userGroup = await _unitOfWork.UserGroupRepository.GetByIdAsync(id);
            if (userGroup == null) return false;

            _unitOfWork.UserGroupRepository.Delete(userGroup);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<string>> GetRolesForGroupAsync(string groupId)
        {
            var roles = await _unitOfWork.CredentialRepository.GetAllAsync();
            return roles.Where(x => x.UserGroupId == groupId).Select(x => x.RoleId).ToList();
        }

        public async Task<bool> AddRoleForGroupAsync(string roleId, string groupId)
        {
            // Xóa tất cả các role hiện có của group
            var existingRoles = await _unitOfWork.CredentialRepository.GetAllAsync();
            var groupRoles = existingRoles.Where(x => x.UserGroupId == groupId).ToList();
            foreach (var role in groupRoles)
            {
                _unitOfWork.CredentialRepository.Delete(role);
            }

            // Thêm mới role vào group
            var newCredential = new Credential
            {
                UserGroupId = groupId,
                RoleId = roleId
            };
            await _unitOfWork.CredentialRepository.AddAsync(newCredential);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
