using System.Linq.Expressions;

namespace smarthome.IDataAccess;

public interface IGenericRepository<T>
    where T : class
{
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAll(Func<T, bool> predicate, List<string>? includes = null);

    void Insert(T entity);

    void Update(T entity);
    T? Get(Expression<Func<T, bool>> searchCondition, List<string>? includes = null);
    void Delete(T entity);
    bool CheckConnection();
}
