using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using smarthome.IDataAccess;

namespace smarthome.DataAccess.Repositories;

public class GenericRepository<T> : IGenericRepository<T>
    where T : class
{
    protected DbContext Context { get; set; }

    public GenericRepository(DbContext context)
    {
        Context = context;
    }

    public virtual IEnumerable<T> GetAll()
    {
        return Context.Set<T>().ToList();
    }

    public virtual IEnumerable<T> GetAll(
        Func<T, bool> searchCondition,
        List<string> includes = null
    )
    {
        IQueryable<T> query = Context.Set<T>();
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query.Where(searchCondition).Select(x => x).ToList();
    }

    public virtual void Insert(T entity)
    {
        Context.Set<T>().Add(entity);
        Save();
    }

    public void Update(T entity)
    {
        Context.Entry(entity).State = EntityState.Modified;
        Save();
    }

    public void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
        Save();
    }

    public bool CheckConnection()
    {
        return Context.Database.EnsureCreated();
    }

    public virtual T? Get(Expression<Func<T, bool>> searchCondition, List<string>? includes = null)
    {
        IQueryable<T> query = Context.Set<T>();
        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        return query.Where(searchCondition).Select(x => x).FirstOrDefault();
    }

    private void Save()
    {
        Context.SaveChanges();
    }
}
