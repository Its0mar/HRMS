using HRMS.Application.Authentication.Dtos;
using HRMS.Application.Authentication.Interfaces;
using HRMS.Domain.Entities;
using HRMS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using System.Data;

namespace HRMS.Infrastructure.Repositories
{
    internal sealed class RegistrationRepository : IRegistrationRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public RegistrationRepository(
            IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Task<bool> OrganizationCodeExistsAsync(
            string code,
            CancellationToken cancellationToken)
        {
            return ExistsAsync(
                procedure: "dbo.Organization_CodeExists",
                parameter: new SqlParameter(
                    "@Code",
                    SqlDbType.VarChar,
                    10)
                {
                    Value = code
                },
                cancellationToken);
        }

        public Task<bool> OrganizationEmailExistsAsync(
            string email,
            CancellationToken cancellationToken)
        {
            return ExistsAsync(
                procedure: "dbo.Organization_EmailExists",
                parameter: new SqlParameter(
                    "@Email",
                    SqlDbType.VarChar,
                    40)
                {
                    Value = email
                },
                cancellationToken);
        }

        public Task<bool> UserEmailExistsAsync(
            string email,
            CancellationToken cancellationToken)
        {
            return ExistsAsync(
                procedure: "dbo.User_EmailExists",
                parameter: new SqlParameter(
                    "@Email",
                    SqlDbType.VarChar,
                    40)
                {
                    Value = email
                },
                cancellationToken);
        }

        public Task<bool> UsernameExistsAsync(
            string username,
            CancellationToken cancellationToken)
        {
            return ExistsAsync(
                procedure: "dbo.User_UsernameExists",
                parameter: new SqlParameter(
                    "@Username",
                    SqlDbType.VarChar,
                    20)
                {
                    Value = username
                },
                cancellationToken);
        }

        public async Task<RegisterResponse> RegisterAsync(
            Organization organization,
            Func<int, User> createOwner,
            CancellationToken cancellationToken)
        {
            await using var connection =
                _connectionFactory.CreateConnection();

            await connection.OpenAsync(cancellationToken);

            await using var transaction =
                (SqlTransaction)await connection.BeginTransactionAsync(
                    cancellationToken);

            try
            {
                var organizationId =
                    await CreateOrganizationAsync(
                        connection,
                        transaction,
                        organization,
                        cancellationToken);

                var owner = createOwner(organizationId);

                var userId =
                    await CreateUserAsync(
                        connection,
                        transaction,
                        owner,
                        cancellationToken);

                var ownerRoleId =
                    await CreateRoleAsync(
                        connection,
                        transaction,
                        organizationId,
                        roleName: "OrganizationOwner",
                        cancellationToken);

                await AssignRoleAsync(
                    connection,
                    transaction,
                    userId,
                    ownerRoleId,
                    cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new RegisterResponse(
                    OrganizationId: organizationId,
                    UserId: userId);
            }
            catch
            {
                await transaction.RollbackAsync(
                    CancellationToken.None);

                throw;
            }
        }

        private async Task<bool> ExistsAsync(
            string procedure,
            SqlParameter parameter,
            CancellationToken cancellationToken)
        {
            await using var connection =
                _connectionFactory.CreateConnection();

            await connection.OpenAsync(cancellationToken);

            await using var command =
                CreateCommand(
                    connection,
                    transaction: null,
                    procedure);

            command.Parameters.Add(parameter);

            var result =
                await command.ExecuteScalarAsync(
                    cancellationToken);

            return result is not null &&
                   result != DBNull.Value &&
                   Convert.ToBoolean(result);
        }

        private static async Task<int> CreateOrganizationAsync(
            SqlConnection connection,
            SqlTransaction transaction,
            Organization organization,
            CancellationToken cancellationToken)
        {
            await using var command =
                CreateCommand(
                    connection,
                    transaction,
                    "dbo.Organization_Create");

            command.Parameters.Add(
                new SqlParameter("@Name", SqlDbType.VarChar, 30)
                {
                    Value = organization.Name
                });

            command.Parameters.Add(
                new SqlParameter("@Code", SqlDbType.VarChar, 10)
                {
                    Value = organization.Code
                });

            command.Parameters.Add(
                new SqlParameter("@Email", SqlDbType.VarChar, 40)
                {
                    Value = organization.Email
                });

            command.Parameters.Add(
                CreateNullableVarcharParameter(
                    "@Address",
                    100,
                    organization.Address));

            command.Parameters.Add(
                CreateNullableVarcharParameter(
                    "@Website",
                    100,
                    organization.Website));

            command.Parameters.Add(
                CreateNullableVarcharParameter(
                    "@LogoUrl",
                    100,
                    organization.LogoUrl));

            command.Parameters.Add(
                new SqlParameter("@IsActive", SqlDbType.Bit)
                {
                    Value = organization.IsActive
                });

            command.Parameters.Add(
                new SqlParameter("@IsDeleted", SqlDbType.Bit)
                {
                    Value = organization.IsDeleted
                });

            command.Parameters.Add(
                new SqlParameter("@CreatedAt", SqlDbType.DateTime2)
                {
                    Value = organization.CreatedAt
                });

            var result =
                await command.ExecuteScalarAsync(
                    cancellationToken);

            return Convert.ToInt32(result);
        }

        private static async Task<int> CreateUserAsync(
            SqlConnection connection,
            SqlTransaction transaction,
            User user,
            CancellationToken cancellationToken)
        {
            await using var command =
                CreateCommand(
                    connection,
                    transaction,
                    "dbo.User_Create");

            command.Parameters.Add(
                new SqlParameter("@OrganizationId", SqlDbType.Int)
                {
                    Value = user.OrganizationId
                });

            command.Parameters.Add(
                new SqlParameter("@Username", SqlDbType.VarChar, 20)
                {
                    Value = user.Username
                });

            command.Parameters.Add(
                new SqlParameter("@Email", SqlDbType.VarChar, 40)
                {
                    Value = user.Email
                });

            command.Parameters.Add(
                new SqlParameter("@PasswordHash", SqlDbType.VarChar, -1)
                {
                    Value = user.PasswordHash
                });

            command.Parameters.Add(
                new SqlParameter("@FirstName", SqlDbType.VarChar, 20)
                {
                    Value = user.FirstName
                });

            command.Parameters.Add(
                new SqlParameter("@LastName", SqlDbType.VarChar, 20)
                {
                    Value = user.LastName
                });

            command.Parameters.Add(
                new SqlParameter("@IsActive", SqlDbType.Bit)
                {
                    Value = user.IsActive
                });

            command.Parameters.Add(
                new SqlParameter("@IsDeleted", SqlDbType.Bit)
                {
                    Value = user.IsDeleted
                });

            command.Parameters.Add(
                new SqlParameter("@CreatedAt", SqlDbType.DateTime2)
                {
                    Value = user.CreatedAt
                });

            var result =
                await command.ExecuteScalarAsync(
                    cancellationToken);

            return Convert.ToInt32(result);
        }

        private static async Task<int> CreateRoleAsync(
            SqlConnection connection,
            SqlTransaction transaction,
            int organizationId,
            string roleName,
            CancellationToken cancellationToken)
        {
            await using var command =
                CreateCommand(
                    connection,
                    transaction,
                    "dbo.Role_Create");

            command.Parameters.Add(
                new SqlParameter("@OrganizationId", SqlDbType.Int)
                {
                    Value = organizationId
                });

            command.Parameters.Add(
                new SqlParameter("@Name", SqlDbType.VarChar, 30)
                {
                    Value = roleName
                });

            var result =
                await command.ExecuteScalarAsync(
                    cancellationToken);

            return Convert.ToInt32(result);
        }

        private static async Task AssignRoleAsync(
            SqlConnection connection,
            SqlTransaction transaction,
            int userId,
            int roleId,
            CancellationToken cancellationToken)
        {
            await using var command =
                CreateCommand(
                    connection,
                    transaction,
                    "dbo.UserRole_Create");

            command.Parameters.Add(
                new SqlParameter("@UserId", SqlDbType.Int)
                {
                    Value = userId
                });

            command.Parameters.Add(
                new SqlParameter("@RoleId", SqlDbType.Int)
                {
                    Value = roleId
                });

            command.Parameters.Add(
                new SqlParameter("@CreatedAt", SqlDbType.DateTime2)
                {
                    Value = DateTime.UtcNow
                });

            await command.ExecuteNonQueryAsync(
                cancellationToken);
        }

        private static SqlCommand CreateCommand(
            SqlConnection connection,
            SqlTransaction? transaction,
            string procedure)
        {
            var command = connection.CreateCommand();

            command.CommandText = procedure;
            command.CommandType = CommandType.StoredProcedure;
            command.Transaction = transaction;

            return command;
        }

        private static SqlParameter CreateNullableVarcharParameter(
            string name,
            int size,
            string? value)
        {
            return new SqlParameter(
                name,
                SqlDbType.VarChar,
                size)
            {
                Value = string.IsNullOrWhiteSpace(value)
                    ? DBNull.Value
                    : value
            };
        }
    }
}
