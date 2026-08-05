using Microsoft.Data.SqlClient;


namespace HRMS.Infrastructure.Persistence
{
    public class SqlExecutor : ISqlExecutor
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public SqlExecutor(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> ExecuteAsync(string procedure,
                                            CancellationToken ct,
                                            params SqlParameter[] parameters)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync(ct);
            await using var cmd = CreateCommand(conn, procedure, parameters);

            return await cmd.ExecuteNonQueryAsync(ct);
        }

        public async Task<List<T>> QueryAsync<T>(string procedure,
                                                 Func<SqlDataReader, T> mapper,
                                                 CancellationToken ct,
                                                 params SqlParameter[] parameters)
        {

            var result = new List<T>();

            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync(ct);

            await using var cmd = CreateCommand(conn, procedure, parameters);
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            while (await reader.ReadAsync(ct))
            {
                result.Add(mapper(reader));
            }

            return result;

        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string procedure,
                                                  Func<SqlDataReader, T> mapper,
                                                  CancellationToken ct,
                                                  params SqlParameter[] parameters)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync(ct);

            await using var cmd = CreateCommand(conn, procedure, parameters);
            await using var reader = await cmd.ExecuteReaderAsync(ct);

            if (await reader.ReadAsync(ct))
            {
                return mapper(reader);
            }

            return default;

        }

        public async Task<bool> ExecuteScalarBoolAsync(
            string procedure,
            CancellationToken ct,
            params SqlParameter[] parameters)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync(ct);

            await using var command = CreateCommand(conn, procedure, parameters);

            var result = await command.ExecuteScalarAsync(ct);
            return result is not null && result != DBNull.Value && Convert.ToBoolean(result);
        }

        public async Task<int> ExecuteWithScalarIntAsync(
            string procedure,
            CancellationToken ct,
            params SqlParameter[] parameters)
        {
            await using var conn = _connectionFactory.CreateConnection();
            await conn.OpenAsync(ct);

            await using var command = CreateCommand(conn, procedure, parameters);

            var result = await command.ExecuteScalarAsync(ct);
            return Convert.ToInt32(result);
        }

        private static SqlCommand CreateCommand(SqlConnection conn,
                                                string procedure,
                                                SqlParameter[] parameters)
        {
            var cmd = conn.CreateCommand();

            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = procedure;

            if (parameters.Length > 0)
                cmd.Parameters.AddRange(parameters);

            return cmd;
        }

        public async Task<T?> QueryMultipleAsync<T>(
            string procedure,
            Func<SqlDataReader, CancellationToken, Task<T?>> mapper,
            CancellationToken cancellationToken,
            params SqlParameter[] parameters)
        {
            await using var connection = _connectionFactory.CreateConnection();
            await connection.OpenAsync(cancellationToken);

            await using var command = CreateCommand(
                connection,
                procedure,
                parameters);

            await using var reader =
                await command.ExecuteReaderAsync(cancellationToken);

            return await mapper(reader, cancellationToken);
        }
    }
}
