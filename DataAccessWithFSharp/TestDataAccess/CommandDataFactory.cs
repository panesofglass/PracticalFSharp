namespace TestDataAccess
{
    using System.Configuration;
    using System.Data;

    using FSharp.Data;

    internal static class CommandDataFactory
    {
        /// <summary>
        /// Builds the specified SQL command.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static CommandData Build(string sql, CommandType type)
        {
            return Build(sql, type, new Param[0]);
        }

        /// <summary>
        /// Builds the specified SQL command.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static CommandData Build(string sql, CommandType type, Param[] parameters)
        {
            return new CommandData(sql, parameters, type, GetConnectionString());
        }

        /// <summary>
        /// Gets the connection string.
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        }
    }
}