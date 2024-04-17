using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace z5.ms.common.infrastructure.db
{
    /// <summary> Helpers for sql operations </summary>
    public static class SqlHelpers
    {
        private static readonly ConcurrentDictionary<Type, string> TableNames = new ConcurrentDictionary<Type, string>();

        /// <summary>Get the table name for an entity type defined in its TableAttribute</summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetTableName<T>() =>
            TableNames.GetOrAdd(typeof(T), t =>
            {
                var tableName = (t.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute)?.Name;
                if (tableName == null) throw new Exception($"Table name not defined for {nameof(T)}");
                return tableName;
            });
    }
}