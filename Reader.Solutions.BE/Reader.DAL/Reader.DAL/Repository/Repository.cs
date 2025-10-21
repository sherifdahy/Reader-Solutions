using Microsoft.EntityFrameworkCore;
using NOTE.Solutions.Entities.Interfaces;
using Reader.DAL.Data;
using System.Linq.Expressions;

namespace NOTE.Solutions.DAL.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().ToList();
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken);
    }

    public T GetById(int id)
    {
        return _context.Set<T>().Find(id);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().FindAsync([id], cancellationToken);
    }

    public List<string> GetDistinct(Expression<Func<T, string>> col)
    {
        return _context.Set<T>().Select(col).Distinct().ToList();
    }

    public T? Find(Expression<Func<T, bool>> criteria, string[]? includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return query.SingleOrDefault(criteria);
    }

    public async Task<T?> FindAsync(Expression<Func<T, bool>> criteria, string[]? includes = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return await query.SingleOrDefaultAsync(criteria, cancellationToken);
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[]? includes = null)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return query.Where(criteria).ToList();
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int skip, int take)
    {
        return _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToList();
    }

    public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, int? skip, int? take, Expression<Func<T, object>>? orderBy = null, bool isDesc = false)
    {
        IQueryable<T> query = _context.Set<T>().Where(criteria);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (take.HasValue)
            query = query.Take(take.Value);

        if (orderBy != null)
            query = isDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

        return query.ToList();
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[]? includes = null, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _context.Set<T>();

        if (includes != null)
            foreach (var include in includes)
                query = query.Include(include);

        return await query.Where(criteria).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int take, int skip, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().Where(criteria).Skip(skip).Take(take).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, int? take, int? skip, Expression<Func<T, object>>? orderBy = null, bool isDesc = false, CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _context.Set<T>().Where(criteria);

        if (take.HasValue)
            query = query.Take(take.Value);

        if (skip.HasValue)
            query = query.Skip(skip.Value);

        if (orderBy != null)
            query = isDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);

        return await query.ToListAsync(cancellationToken);
    }

    public T Add(T entity)
    {
        _context.Set<T>().Add(entity);
        return entity;
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
        return entity;
    }

    public IEnumerable<T> AddRange(IEnumerable<T> entities)
    {
        _context.Set<T>().AddRange(entities);
        return entities;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
    {
        await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
        return entities;
    }

    public T Update(T entity)
    {
        _context.Update(entity);
        return entity;
    }

    public bool UpdateRange(IEnumerable<T> entities)
    {
        _context.UpdateRange(entities);
        return true;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public int Count()
    {
        return _context.Set<T>().Count();
    }

    public int Count(Expression<Func<T, bool>> criteria)
    {
        return _context.Set<T>().Count(criteria);
    }

    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().CountAsync(cancellationToken);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>> criteria, CancellationToken cancellationToken = default)
    {
        return await _context.Set<T>().CountAsync(criteria, cancellationToken);
    }

    public async Task<long> MaxAsync(Expression<Func<T, object>> column, CancellationToken cancellationToken = default)
    {
        return Convert.ToInt64(await _context.Set<T>().MaxAsync(column, cancellationToken));
    }

    public async Task<long> MaxAsync(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> column, CancellationToken cancellationToken = default)
    {
        return Convert.ToInt64(await _context.Set<T>().Where(criteria).MaxAsync(column, cancellationToken));
    }

    public long Max(Expression<Func<T, object>> column)
    {
        return Convert.ToInt64(_context.Set<T>().Max(column));
    }

    public long Max(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> column)
    {
        return Convert.ToInt64(_context.Set<T>().Where(criteria).Max(column));
    }

    public bool IsExist(Expression<Func<T, bool>> criteria)
    {
        return _context.Set<T>().Any(criteria);
    }

    public T? Last(Expression<Func<T, bool>> criteria, Expression<Func<T, object>> orderBy)
    {
        return _context.Set<T>().OrderByDescending(orderBy).FirstOrDefault(criteria);
    }

    public IEnumerable<T> FindWithFilters(Expression<Func<T, bool>>? criteria = null, string? sortColumn = null, string? sortColumnDirection = null, int? skip = null, int? take = null)
    {
        throw new NotImplementedException();
    }
}
