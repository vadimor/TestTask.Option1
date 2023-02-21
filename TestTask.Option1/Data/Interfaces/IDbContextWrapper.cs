using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace TestTask.Option1.Data.Interfaces
{
    // This interface provides a wrapper for working with transactions 

    public interface IDbContextWrapper<T>
        where T : DbContext
    {
        T DbContext { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
