﻿namespace Libs.Data
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected QuanLyTaiSanCtyDATNContext _dbContext;
        protected readonly DbSet<T> dbset;

        public RepositoryBase(QuanLyTaiSanCtyDATNContext dbContext)
        {
            _dbContext = dbContext;
            dbset = _dbContext.Set<T>();
        }
        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await dbset.AnyAsync(filter);
        }
        public virtual IQueryable<T> Query(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbset;
            if (filter != null)
                query = query.Where(filter);
            return query;
        }
        public virtual void Add(T entity)
        {
            dbset.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbset.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
        }

        public virtual int Count(Expression<Func<T, bool>> where)
        {
            return dbset.Count<T>(where);
        }

        public virtual IEnumerable<T> GetList(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "",
            int skip = 0,
            int take = 0)
        {
            IQueryable<T> query = dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (skip > 0)
            {
                query = query.Skip(skip);
            }

            if (take > 0)
            {
                query = query.Take(take);
            }

            return query.ToList();
        }

        public virtual T GetById(object id)
        {
            return dbset.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return dbset.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).ToList();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault<T>();
        }

        public virtual bool Any(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).Any<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbset.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(object id)
        {
            return await dbset.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await dbset.AddAsync(entity);
        }
    }
}
