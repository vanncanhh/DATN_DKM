namespace Libs.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        void SaveChanges();
        IRepository<Device> DeviceRepository { get; }
        IRepository<Credential> CredentialRepository { get; }
        IRepository<DestructivelDevice> DestructivelDeviceRepository { get; }
        IRepository<DestructiveType> DestructiveTypeRepository { get; }
        IRepository<DeviceDevice> DeviceDeviceTypeRepository { get; }
        IRepository<DeviceOfProject> DeviceOfProjectRepository { get; }
        IRepository<DeviceType> DeviceTypeRepository { get; }
        IRepository<DeviceTypeComponantType> DeviceTypeComponantTypeRepository { get; }
        IRepository<ProjectDkc> ProjectDkcRepository { get; }
        IRepository<RepairDetail> RepairDetailRepository { get; }
        IRepository<RepairType> RepairTypeRepository { get; }
        IRepository<RequestDevice> RequestDeviceRepository { get; }
        IRepository<Role> RoleRepository { get; }
        IRepository<ScheduleTest> ScheduleTestRepository { get; }
        IRepository<Supplier> SupplierRepository { get; }
        IRepository<UsageDevice> UsageDeviceRepository { get; }
        IRepository<User> UserRepository { get; }
        IRepository<UserGroup> UserGroupRepository { get; }
        IRepository<UserLogin> UserLoginRepository { get; }
    }
}
