
using NOTE.Solutions.Entities.Interfaces;
using Reader.DAL.Data;
using Reader.Entities.Entities;

namespace NOTE.Solutions.DAL.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Clients = new Repository<Client>(_context);
        Groups = new Repository<Group>(_context);
        GroupClients = new Repository<GroupClient>(_context);
    }

    public IRepository<Client> Clients { get; }
    public IRepository<Group> Groups { get; }
    public IRepository<GroupClient> GroupClients { get; }


    public void Dispose()
    {
        _context.Dispose();
    }
    public async Task<int> SaveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
