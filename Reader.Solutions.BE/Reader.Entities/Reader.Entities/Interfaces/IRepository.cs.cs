using System.Linq.Expressions;
using System.Threading;

namespace NOTE.Solutions.Entities.Interfaces
{
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);

        List<string> GetDistinct(Expression<Func<T, string>> col);

        T? Find(Expression<Func<T, bool>> criteria, string[]? includes = null);
        Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes = null, CancellationToken cancellationToken = default);

        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[]? includes = null);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int take, int skip);
        IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? take, int? skip,
            Expression<Func<T, object>>? orderBy = null, bool isDesc = false);

        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[]? includes = null, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int skip, int take, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? skip, int? take,
            Expression<Func<T, object>>? orderBy = null, bool isDesc = false, CancellationToken cancellationToken = default);

        IEnumerable<T> FindWithFilters(
            Expression<Func<T, bool>>? criteria = null,
            string? sortColumn = null,
            string? sortColumnDirection = null,
            int? skip = null,
            int? take = null);

        T Add(T entity);
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);

        IEnumerable<T> AddRange(IEnumerable<T> entities);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        T Update(T entity);
        bool UpdateRange(IEnumerable<T> entities);

        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        int Count();
        int Count(Expression<Func<T, bool>> criteria);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default);

        Task<long> MaxAsync(Expression<Func<T, object>> column, CancellationToken cancellationToken = default);
        Task<long> MaxAsync(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> column, CancellationToken cancellationToken = default);

        long Max(Expression<Func<T, object>> column);
        long Max(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> column);

        bool IsExist(Expression<Func<T, bool>> criteria);

        T? Last(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> orderBy);
    }
}
