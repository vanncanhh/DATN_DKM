namespace Libs.Service
{
    public interface IUserRoleService
    {
        Task<IEnumerable<InformationUser>> GetAllUsersAsync();
        Task<bool> RegisterUserAsync(string fullName, string role, string username, string password);
        Task<UserLogin?> GetUserByUsernameAsync(string username);
        Task<bool> ChangeRoleByUserIdAsync(int id, string fullName, string username, string role, string password);
        Task<bool> DeleteUserAsync(int userId);
    }
    public class UserRoleService : IUserRoleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<InformationUser>> GetAllUsersAsync()
        {
            var users = await _unitOfWork.UserLoginRepository.GetAllAsync();
            var userGroups = await _unitOfWork.UserGroupRepository.GetAllAsync();

            var userRole = users.Select(item =>
            {
                var group = userGroups.FirstOrDefault(x => x.Id == item.GroupId);
                return new InformationUser
                {
                    Id = item.Id,
                    Username = item.UserName,
                    RoleGroupID = item.GroupId,
                    RoleGroupName = group?.Name ?? "Chưa được phân quyền",
                    FullName = item.FullName
                };
            }).ToList();

            return userRole;
        }

        public async Task<bool> RegisterUserAsync(string fullName, string role, string username, string password)
        {
            var hashedPassword = Encryptor.MD5Hash(password);

            var newUser = new UserLogin
            {
                FullName = fullName,
                GroupId = role,
                UserName = username,
                PassWord = hashedPassword
            };

            await _unitOfWork.UserLoginRepository.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<UserLogin?> GetUserByUsernameAsync(string username)
        {
            return await _unitOfWork.UserLoginRepository.GetAllAsync()
                .ContinueWith(t => t.Result.FirstOrDefault(x => x.UserName == username));
        }

        public async Task<bool> ChangeRoleByUserIdAsync(int id, string fullName, string username, string role, string password)
        {
            var user = await _unitOfWork.UserLoginRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.FullName = fullName;
            user.UserName = username;
            user.GroupId = role;
            user.PassWord = Encryptor.MD5Hash(password);

            _unitOfWork.UserLoginRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _unitOfWork.UserLoginRepository.GetByIdAsync(userId);
            if (user == null) return false;

            _unitOfWork.UserLoginRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
