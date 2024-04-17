using System;
using System.Data;
using System.Data.SQLite;
using Dapper;
using Dapper.FastCrud;

/// <summary>Helpers for working with sqlite as a test database</summary>
public static class SqlLiteHelpers
{
    private static bool _configuredOnce;

    /// <summary>Create an in memory sqlite instance with dapper fast crud compatibility</summary>
    /// <returns></returns>
    public static IDbConnection CreateTestDatabase()
    {
        var dbName = $"{Guid.NewGuid():N}.sqlite";
        SQLiteConnection.CreateFile(dbName);
        var connection = new SQLiteConnection($"Data Source={dbName};");
        connection.Open();
        return connection;
    }

    public static void ConfigureDapper()
    {
        if (_configuredOnce) return;
        _configuredOnce = true;

        OrmConfiguration.DefaultDialect = SqlDialect.SqLite;
        SqlMapper.AddTypeHandler(new SqliteGuidTypeHandler());
    }

    /// <inheritdoc />
    /// <summary>Type handler for dapper to support guids in sqlite</summary>
    /// <remarks>Usage: SqlMapper.AddTypeHandler(new SqliteGuidTypeHandler());</remarks>
    private class SqliteGuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        public override Guid Parse(object value)
        {
            if (value is Guid guid)
                return guid;

            var inVal = (byte[]) value;
            var outVal = BitConverter.IsLittleEndian // Not sure if this is correct. (it works on my machine ;) DAS
                ? new[] {inVal[0], inVal[1], inVal[2], inVal[3], inVal[4], inVal[5], inVal[6], inVal[7], inVal[8], inVal[9], inVal[10], inVal[11], inVal[12], inVal[13], inVal[14], inVal[15]}
                : new[] {inVal[3], inVal[2], inVal[1], inVal[0], inVal[5], inVal[4], inVal[7], inVal[6], inVal[8], inVal[9], inVal[10], inVal[11], inVal[12], inVal[13], inVal[14], inVal[15]};
            return new Guid(outVal);
        }

        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            var inVal = value.ToByteArray();
            var outVal = BitConverter.IsLittleEndian // Not sure if this is correct. (it works on my machine ;) DAS
                ? new[] {inVal[0], inVal[1], inVal[2], inVal[3], inVal[4], inVal[5], inVal[6], inVal[7], inVal[8], inVal[9], inVal[10], inVal[11], inVal[12], inVal[13], inVal[14], inVal[15]}
                : new[] {inVal[3], inVal[2], inVal[1], inVal[0], inVal[5], inVal[4], inVal[7], inVal[6], inVal[8], inVal[9], inVal[10], inVal[11], inVal[12], inVal[13], inVal[14], inVal[15]};
            parameter.Value = outVal;
        }
    }
}