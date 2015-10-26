using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameAggregator.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        bool Exists(object id);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            int skip = -1,
            int num = -1,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""
        );

        TEntity Get(object id);

        void Insert(TEntity entity);

        void Update(TEntity entityToUpdate);

        void Delete(TEntity entityToDelete);

        void Delete(object id);
    }
}
