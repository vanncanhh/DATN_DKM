using Libs.Models;

namespace Libs.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly QuanLyTaiSanCtyDATNContext _context;
        private IRepository<Device> _dRepository;
        public UnitOfWork(QuanLyTaiSanCtyDATNContext context)
        {
            _context = context;
        }
        public IRepository<Device> DeviceRepository => _dRepository ??= new RepositoryBase<Device>(_context);

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
