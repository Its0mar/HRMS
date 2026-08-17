
using Microsoft.Data.SqlClient;
using System.Data;

namespace HRMS.Infrastructure.Persistence
{
    internal static class SqlParams
    {
        public static SqlParameter Int(string name, int value) =>
            new(name, SqlDbType.Int) { Value = value };
        public static SqlParameter NullableInt(string name, int? value) =>
            new(name, SqlDbType.Int) { Value = value.HasValue ? value.Value : DBNull.Value };
        public static SqlParameter VarChar(string name, int size, string? value) =>
            new(name, SqlDbType.VarChar, size) { Value = string.IsNullOrWhiteSpace(value) ? DBNull.Value : value };
        public static SqlParameter Char(string name, int size, string value) =>
            new(name, SqlDbType.Char, size) { Value = value };
        public static SqlParameter DateTime2(string name, DateTime value) =>
            new(name, SqlDbType.DateTime2) { Value = value };
        public static SqlParameter Date(string name, DateOnly value) =>
            new(name, SqlDbType.Date) { Value = value };
        public static SqlParameter Date(string name, DateTime value) =>
            new(name, SqlDbType.Date) { Value = value.Date };
        public static SqlParameter Bit(string name, bool value) =>
            new(name, SqlDbType.Bit) { Value = value };
        public static SqlParameter TokenHash(string name, string value) =>
            new(name, SqlDbType.Char, 64) { Value = value };
        public static SqlParameter NullableVarChar(string name, int size, string? value) =>
        new(name, SqlDbType.VarChar, size) { Value = string.IsNullOrWhiteSpace(value) ? DBNull.Value : value };
        public static SqlParameter NullableDateTime2(string name, DateTime? value) =>
            new(name, SqlDbType.DateTime2) { Value = value.HasValue ? value.Value : DBNull.Value };
    }
}