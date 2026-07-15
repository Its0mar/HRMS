
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Persistence
{
    public interface ISqlExecutor
    {
        Task<List<T>> QueryAsync<T>(
            string procedure,
            Func<SqlDataReader, T> mapper,
            CancellationToken ct,
            params SqlParameter[] parameters);

        Task<T?> QueryFirstOrDefaultAsync<T>(
            string procedure,
            Func<SqlDataReader, T> mapper,
            CancellationToken ct,
            params SqlParameter[] parameters);

        Task<int> ExecuteAsync(
            string procedure,
            CancellationToken ct,
            params SqlParameter[] parameters);
    }
}
