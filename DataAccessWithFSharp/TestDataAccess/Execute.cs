namespace TestDataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Xml;

    using CSharpDataAccess;

    using FSharp.Data;

    using Microsoft.FSharp.Collections;

    using Param=FSharp.Data.Param;

    public static class Execute
    {
        private const string SynchronizationContextNotSetMessage =
            @"
In order for async calls to work, F# needs the SynchronizationContext set.
In WinForms this is taken care of automatically, if you are using a dll/command line,
you can set the context using the following code:

System.Threading.SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
";

        /// <summary>
        /// Asserts the synchronization context is set.
        /// </summary>
        public static void AssertSynchronizationContextIsSet()
        {
            if (null == SynchronizationContext.Current)
                throw new InvalidOperationException(SynchronizationContextNotSetMessage);
        }

        public static IEnumerable<T> CSharpCommand<T>(
            string sql, CommandType type, Action<IDataReader> onPreMap, Func<IDataRecord, T> onMap)
        {
            var data = CommandDataFactory.Build(sql, type);
            var cSharpData = new CSharpDataAccess.CommandData
            {
                sql = data.sql,
                cmdType = data.cmdType,
                connectionString = data.connectionString
            };

            return CSharpFetcher.ExecuteDataReader(cSharpData, onPreMap, onMap);
        }

        /// <summary>
        /// Executes the command synchronously.
        /// </summary>
        /// <typeparam name="T">The type of the data reader value.</typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="type">The type.</param>
        /// <param name="onPreMap">The on pre map.</param>
        /// <param name="onMap">The on map.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static IEnumerable<T> Command<T>(
            string sql, CommandType type, Action<IDataReader> onPreMap, Converter<IDataRecord, T> onMap,
            params Param[] parameters)
        {
            var data = CommandDataFactory.Build(sql, type, parameters);

            return Fetcher.ExecuteDataReader<T>(data, onPreMap.ToConverter(), onMap);
        }

        /// <summary>
        /// Executes the command synchronously.
        /// </summary>
        /// <typeparam name="T">The type of the data reader value.</typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="type">The type.</param>
        /// <param name="onMap">The on map.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static IEnumerable<T> Command<T>(
            string sql, CommandType type, Converter<IDataRecord, T> onMap, params Param[] parameters)
        {
            Action<IDataReader> noAction = rdr => { };

            return Command(sql, type, noAction, onMap, parameters);
        }

        /// <summary>
        /// Executes the non-query command synchronously.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public static int Command(string sql, CommandType type, params Param[] parameters)
        {
            var data = CommandDataFactory.Build(sql, type, parameters);

            return Fetcher.ExecuteNonQuery(data);
        }

        /// <summary>
        /// Executes the command to retrieve the data as XML synchronously
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="type">The type.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The XML document.</returns>
        public static XmlDocument XmlCommand(string sql, CommandType type, params Param[] parameters)
        {
            var data = CommandDataFactory.Build(sql, type, parameters);

            var doc = new XmlDocument();
            doc.LoadXml(Fetcher.GetXml(data));

            return doc;
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the data reader value.</typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="type">The type.</param>
        /// <param name="onPreMap">The on pre map.</param>
        /// <param name="onMap">The on map.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="parameters">The parameters.</param>
        public static void CommandAsync<T>(
            string sql, CommandType type, Action<IDataReader> onPreMap, Converter<IDataRecord, T> onMap,
            Action<T[]> callback, params Param[] parameters)
        {
            AssertSynchronizationContextIsSet();

            var data = CommandDataFactory.Build(sql, type, parameters);

            Fetcher.AsyncExecuteDataReaderWithContinuation<T>(
                data,
                onPreMap.ToConverter(),
                onMap,
                callback.ToConverter());
        }

        /// <summary>
        /// Executes the command asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the data reader value.</typeparam>
        /// <param name="sql">The SQL.</param>
        /// <param name="type">The type.</param>
        /// <param name="onMap">The on map.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="parameters">The parameters.</param>
        public static void CommandAsync<T>(
            string sql, CommandType type, Converter<IDataRecord, T> onMap, Action<T[]> callback, params Param[] parameters)
        {
            Action<IDataReader> noAction = rdr => { };

            CommandAsync(sql, type, noAction, onMap, callback, parameters);
        }

        /// <summary>
        /// Executes the non-query command asynchronously.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="type">The type.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="parameters">The parameters.</param>
        public static void CommandAsync(string sql, CommandType type, Action<int> callback, params Param[] parameters)
        {
            AssertSynchronizationContextIsSet();

            var data = CommandDataFactory.Build(sql, type, parameters);

            Fetcher.AsyncExecuteNonQueryWithContinuation(
                data,
                callback.ToConverter());
        }

        /// <summary>
        /// Executes the XML command asynchronously.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <param name="type">The type.</param>
        /// <param name="callback">The callback.</param>
        /// <param name="parameters">The parameters.</param>
        public static void XmlCommandAsync(
            string sql, CommandType type, Action<XmlDocument> callback, params Param[] parameters)
        {
            AssertSynchronizationContextIsSet();

            var data = CommandDataFactory.Build(sql, type, parameters);

            Action<string> firstCallback = str =>
            {
                var doc = new XmlDocument();
                doc.LoadXml(str);
                callback(doc);
            };

            Fetcher.AsyncGetXmlWithContinuation(data, firstCallback.ToConverter());
        }
    }
}