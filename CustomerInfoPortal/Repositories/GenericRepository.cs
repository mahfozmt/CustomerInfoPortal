using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomerInfoPortal.Repositories
{
    public class GenericRepository<T> : IGenirecRepositoryBase<T> where T : class
    {
        private readonly CIPDbContext _context;
        private DbSet<T> table = null;
        public GenericRepository(CIPDbContext DbContext)
        {
            _context = DbContext;
            table = _context.Set<T>();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Create(T entity)
        {
            table.Add(entity);
        }

        public void Remove(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public IQueryable<T> GetAll()
        {
            return table.AsNoTracking();
        }

        public IQueryable<T> GetByExpresion(Expression<Func<T, bool>> expression)
        {
            return table.Where(expression).AsNoTracking();
        }

        public void Update(T entity)
        {
            table.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}
