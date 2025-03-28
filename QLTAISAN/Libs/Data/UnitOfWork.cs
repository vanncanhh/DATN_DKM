namespace Libs.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuanLyTaiSanCtyDATNContext _context;
        private IRepository<Device> _dRepository;
        private IRepository<Credential> _cRepository;
        private IRepository<DestructivelDevice> _dsdRepository;
        private IRepository<DestructiveType> _dtRepository;
        private IRepository<DeviceDevice> _ddRepository;
        private IRepository<DeviceOfProject> _dofRepository;
        private IRepository<DeviceType> _devRepository;
        private IRepository<DeviceTypeComponantType> _dtctRepository;
        private IRepository<ProjectDkc> _pkRepository;
        private IRepository<RepairDetail> _rdRepository;
        private IRepository<RepairType> _rtRepository;
        private IRepository<RequestDevice> _rqdRepository;
        private IRepository<Role> _rRepository;
        private IRepository<ScheduleTest> _stRepository;
        private IRepository<Supplier> _sRepository;
        private IRepository<UsageDevice> _udRepository;
        private IRepository<User> _uRepository;
        private IRepository<UserGroup> _ugRepository;
        private IRepository<UserLogin> _ulRepository;
        public UnitOfWork(QuanLyTaiSanCtyDATNContext context)
        {
            _context = context;
        }
        public IRepository<Device> DeviceRepository => _dRepository ??= new RepositoryBase<Device>(_context);
        public IRepository<Credential> CredentialRepository => _cRepository ??= new RepositoryBase<Credential>(_context);
        public IRepository<DestructivelDevice> DestructivelDeviceRepository => _dsdRepository ??= new RepositoryBase<DestructivelDevice>(_context);
        public IRepository<DestructiveType> DestructiveTypeRepository => _dtRepository ??= new RepositoryBase<DestructiveType>(_context);
        public IRepository<DeviceDevice> DeviceDeviceTypeRepository => _ddRepository ??= new RepositoryBase<DeviceDevice>(_context);
        public IRepository<DeviceOfProject> DeviceOfProjectRepository => _dofRepository ??= new RepositoryBase<DeviceOfProject>(_context);
        public IRepository<DeviceType> DeviceTypeRepository => _devRepository ??= new RepositoryBase<DeviceType>(_context);
        public IRepository<DeviceTypeComponantType> DeviceTypeComponantTypeRepository => _dtctRepository ??= new RepositoryBase<DeviceTypeComponantType>(_context);
        public IRepository<ProjectDkc> ProjectDkcRepository => _pkRepository ??= new RepositoryBase<ProjectDkc>(_context);
        public IRepository<RepairDetail> RepairDetailRepository => _rdRepository ??= new RepositoryBase<RepairDetail>(_context);
        public IRepository<RepairType> RepairTypeRepository => _rtRepository ??= new RepositoryBase<RepairType>(_context);
        public IRepository<RequestDevice> RequestDeviceRepository => _rqdRepository ??= new RepositoryBase<RequestDevice>(_context);
        public IRepository<Role> RoleRepository => _rRepository ??= new RepositoryBase<Role>(_context);
        public IRepository<ScheduleTest> ScheduleTestRepository => _stRepository ??= new RepositoryBase<ScheduleTest>(_context);
        public IRepository<Supplier> SupplierRepository => _sRepository ??= new RepositoryBase<Supplier>(_context);
        public IRepository<UsageDevice> UsageDeviceRepository => _udRepository ??= new RepositoryBase<UsageDevice>(_context);
        public IRepository<User> UserRepository => _uRepository ??= new RepositoryBase<User>(_context);
        public IRepository<UserGroup> UserGroupRepository => _ugRepository ??= new RepositoryBase<UserGroup>(_context);
        public IRepository<UserLogin> UserLoginRepository => _ulRepository ??= new RepositoryBase<UserLogin>(_context);
        public QuanLyTaiSanCtyDATNContext Context => _context;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
