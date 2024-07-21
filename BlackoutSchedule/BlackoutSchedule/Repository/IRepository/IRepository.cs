using System.Linq.Expressions;

namespace BlackoutSchedule.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Find(int id);
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includePropreties = null,
            bool isTracking = true
        );

        T FirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includePropreties = null,
            bool isTracking = true
        );

        void Add(T entity);
        void Save();
    }
}
