using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSharpDataAccess
{
    using System.Data;
    using System.Data.SqlClient;

    public class Param
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }
    
    public class CommandData
    {
        public string sql { get; set; }
        public Param[] parameters { get; set; }
        public CommandType cmdType { get; set; }
        public string connectionString { get; set; }
    }

    public static class CSharpFetcher
    {
        public static IEnumerable<T> ExecuteDataReader<T>(
            CommandData data, Action<IDataReader> onPreMap, Func<IDataRecord, T> onMap)
        {
            using (var connection = new SqlConnection { ConnectionString = data.connectionString })
            {
                connection.Open();
                var command = new SqlCommand { CommandText = data.sql, CommandType = data.cmdType, Connection = connection };
                using (IDataReader reader = command.ExecuteReader())
                {
                    onPreMap(reader);
                    while (reader.Read())
                    {
                        yield return onMap(reader);
                    }
                }

                connection.Close();
            }
        }
    }
}
