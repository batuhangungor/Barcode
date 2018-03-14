using Barcode.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Barcode.Data.Repository
{
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext db;

        public EFRepository(ApplicationDbContext _db)
        {
            db = _db;
        }
        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> filter)
        {
            return db.Set<T>().Where(filter);
        }

        public IQueryable<T> GetAll()
        {
            return db.Set<T>();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
