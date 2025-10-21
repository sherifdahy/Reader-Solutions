
using Reader.Entities.Entities;

namespace NOTE.Solutions.Entities.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IRepository<Client> Clients { get; }
    public IRepository<Group> Groups { get; }
    public IRepository<GroupClient> GroupClients { get; }

    Task<int> SaveAsync(CancellationToken cancellationToken = default);
}
