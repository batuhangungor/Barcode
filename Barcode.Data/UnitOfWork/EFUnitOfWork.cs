using Barcode.Data.Context;
using Barcode.Data.Repository;
using Barcode.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Barcode.Data.UnitOfWork
{
    /// <summary>
    /// EntityFramework için oluşturmuş olduğumuz UnitOfWork.
    /// EFRepository'de olduğu gibi bu şekilde tasarlamamızın ana sebebi ise veritabanına independent(bağımsız) bir durumda kalabilmek. Örneğin MongoDB için ise ilgili provider'ı aracılığı ile MongoDBOfWork tasarlayabiliriz.
    /// </summary>
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public EFUnitOfWork(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException("dbContext can not be null.");
        }

        List<RepositoryListItem> RepositoryList = new List<RepositoryListItem>();
        #region IUnitOfWork Members

        public IRepository<T> GetRepository<T>() where T : class
        {
            var repositoryItem = RepositoryList.Where(q => q.Name == typeof(T).Name).FirstOrDefault();
            if (repositoryItem == null)
            {
                repositoryItem = new RepositoryListItem()
                {
                    Name = typeof(T).Name,
                    Repository = new EFRepository<T>(_context)
                };
                RepositoryList.Add(repositoryItem);
            }
            return repositoryItem.Repository;
        }

        public int Commit()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region IDisposable Members
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}