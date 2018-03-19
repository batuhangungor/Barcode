using Barcode.Data.Repository;
using Barcode.Models;
using System;

namespace Barcode.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        int Commit();
    }
}