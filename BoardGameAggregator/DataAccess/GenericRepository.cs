using BoardGameAggregator.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardGameAggregator.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal SystemContext Db;
        internal DbSet<TEntity> DbSet;

        public GenericRepository(SystemContext db)
        {
            this.Db = db;
            this.DbSet = db.Set<TEntity>();
        }

        public bool Exists(object id)
        {
            return this.DbSet.Find(id) != null;
        }

        public IEnumerable<TEntity> Get(
            System.Linq.Expressions.Expression<Func<TEntity, bool>> filter,
            int skip,
            int num,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties
        )
        {
            IQueryable<TEntity> query = this.DbSet;

            if (filter != null)
            {
                query = query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);

                if (skip > -1)
                {
                    query = query.Skip(skip);
                }

                if (num > -1)
                {
                    query = query.Take(num);
                }
            }

            return query.ToList();
        }

        public TEntity Get(object id)
        {
            return this.DbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            this.DbSet.Add(entity);
        }

        public void Update(TEntity entityToUpdate)
        {
            this.Db.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void Delete(TEntity entityToDelete)
        {
            if (this.Db.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.DbSet.Attach(entityToDelete);
            }
            this.DbSet.Remove(entityToDelete);
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = this.DbSet.Find(id);
            if (entityToDelete != null)
            {
                Delete(entityToDelete);
            }
        }
    }
}
