
using Microsoft.Data.SqlClient;

namespace HRMS.Infrastructure.Persistence
{
    public interface IDbConnectionFactory
    {
        SqlConnection CreateConnection();
    }
}
