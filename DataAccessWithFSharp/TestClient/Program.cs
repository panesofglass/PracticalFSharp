namespace TestClient
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Threading;

    using TestDataAccess;

    internal class Program
    {
        /// <summary>
        /// Initializes the <see cref="Program"/> class.
        /// </summary>
        static Program()
        {
            SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
        }

        /// <summary>
        /// The main program.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Getting tests from the database synchronously in C# ...");

            var cSharpWatch = Stopwatch.StartNew();
            foreach (var test in GetTestsWithCSharp())
            {
                Console.Write(test.ToString());
                Console.WriteLine("\t{0}", cSharpWatch.ElapsedMilliseconds);
            }
            cSharpWatch.Stop();

            Console.WriteLine();
            Console.WriteLine("Getting tests from the database synchronously in F# ...");

            var syncWatch = Stopwatch.StartNew();
            foreach (var test in GetTests())
            {
                Console.Write(test.ToString());
                Console.WriteLine("\t{0}", syncWatch.ElapsedMilliseconds);
            }
            syncWatch.Stop();

            Console.WriteLine();
            Console.WriteLine("Getting tests from the database asynchronously ...");

            var asyncWatch = Stopwatch.StartNew();
            GetTestsAsync(results =>
            {
                foreach (var test in results)
                {
                    Console.Write(test.ToString());
                    Console.WriteLine("\t{0}", asyncWatch.ElapsedMilliseconds);
                }
            });

            // This is necessary to allow the asynchronous continuation to complete.
            Console.ReadKey();
            asyncWatch.Stop();
        }

        private static IEnumerable<Test> GetTestsWithCSharp()
        {
            int idOrdinal = -1;
            int nameOrdinal = -1;

            const string sql = "select * from Test";

            return Execute.CSharpCommand(
                sql,
                CommandType.Text,
                rdr =>
                {
                    Console.WriteLine("Pre-build");
                    idOrdinal = rdr.GetOrdinal("Id");
                    nameOrdinal = rdr.GetOrdinal("Name");
                },
                rec => new Test
                {
                    Id = rec.GetInt32(idOrdinal),
                    Name = rec.GetString(nameOrdinal)
                });
        }

        /// <summary>
        /// Gets the tests synchronously.
        /// </summary>
        /// <returns>The list of tests.</returns>
        private static IEnumerable<Test> GetTests()
        {
            int idOrdinal = -1;
            int nameOrdinal = -1;

            const string sql = "select * from Test";

            return Execute.Command(
                sql,
                CommandType.Text,
                rdr =>
                {
                    Console.WriteLine("Pre-build");
                    idOrdinal = rdr.GetOrdinal("Id");
                    nameOrdinal = rdr.GetOrdinal("Name");
                },
                rec => new Test
                {
                    Id = rec.GetInt32(idOrdinal),
                    Name = rec.GetString(nameOrdinal)
                });
        }

        /// <summary>
        /// Gets the tests asynchronously.
        /// </summary>
        /// <param name="callback">The callback.</param>
        private static void GetTestsAsync(Action<Test[]> callback)
        {
            int idOrdinal = -1;
            int nameOrdinal = -1;

            const string sql = "select * from Test";

            Execute.CommandAsync(
                sql,
                CommandType.Text,
                rdr =>
                {
                    Console.WriteLine("Pre-build");
                    idOrdinal = rdr.GetOrdinal("Id");
                    nameOrdinal = rdr.GetOrdinal("Name");   
                },
                rec => new Test()
                {
                    Id = rec.GetInt32(idOrdinal),
                    Name = rec.GetString(nameOrdinal)
                },
                callback);
        }
    }
}