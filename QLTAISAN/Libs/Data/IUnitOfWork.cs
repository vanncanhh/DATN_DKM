using Libs.Models;

namespace Libs.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        void SaveChanges();
        IRepository<Device> DeviceRepository { get; }
    }
}
