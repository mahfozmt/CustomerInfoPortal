using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerInfoPortal.Repositories
{
    public class GenericRepository<T> : IRepositoryBase<T> where T : class
    {
        private readonly CIPDbContext _context;

        public GenericRepository(CIPDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> SaveAsync(T entity)
        {
            await _context.SaveChangesAsync();
            return entity;

        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
