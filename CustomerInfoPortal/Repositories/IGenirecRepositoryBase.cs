using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CustomerInfoPortal.Repositories
{

    public interface IGenirecRepositoryBase<T> where T : class
    {

        void Create(T entity);
        void Update(T entity);
        void Remove(object id);
        IQueryable<T> GetAll();
        IQueryable<T> GetByExpresion(Expression<Func<T, bool>> expression);
        void SaveChanges();

    }

}