using Microsoft.EntityFrameworkCore;
using TestTask.Option1.Data.Interfaces;

namespace TestTask.Option1.Data
{
    // This class provides methods for wrapping code that changes the database

    public abstract class BaseDataService<T>
        where T : DbContext
    {
        private readonly ILogger<BaseDataService<T>> _logger;
        private readonly IDbContextWrapper<T> _dbContextWrapper;

        public BaseDataService(ILogger<BaseDataService<T>> logger, IDbContextWrapper<T> dbContextWrapper)
        {
            _logger = logger;
            _dbContextWrapper = dbContextWrapper;
        }

        protected async Task ExecuteSafeAsync(Func<Task> action)
        {
            await using var transaction = await _dbContextWrapper.BeginTransactionAsync();

            try
            {
                await action();

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "transaction is rollbacked");
            }
        }

        protected async Task<TResult> ExecuteSafeAsync<TResult>(Func<Task<TResult>> action)
        {
            await using var transaction = await _dbContextWrapper.BeginTransactionAsync();

            try
            {
                var result = await action();

                await transaction.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"transaction is rollbacked");
            }

            return default!;
        }
    }
}
