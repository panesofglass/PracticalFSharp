namespace Client
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var urls = new List<string>
            {
                "http://www.live.com/",
                "http://www.google.com",
                "http://www.yahoo.com",
                "http://www.twine.com/user/panesofglass",
                "http://msdn.microsoft.com/en-us/library/86wf6409(VS.71).aspx",
                "http://tomasp.net/blog/fsharp-webcast-functional.aspx"
            };

            //var urls = new List<string>
            //{
            //    "http://localhost/twine.html",
            //    "http://localhost/fsharp-workflows.html",
            //    "http://localhost/fsharp-webcast-functional.aspx.html",
            //    "http://localhost/async-computation-expressions-resource-and-exception-management.aspx.html",
            //    "http://localhost/introducing-f-asynchronous-workflows.aspx.html",
            //    "http://localhost/wf_Fsharp_ccr.aspx.html",
            //};

            ThreadPool.SetMinThreads(10, 1000);
            TestSynchronousCSharpWebRequests(urls);
            TestSynchronousFSharpWebRequests(urls);
            TestAsynchronousCSharpWebRequests(urls);
            TestAsynchronousFSharpWebRequests(urls);
        }

        private static void TestAsynchronousCSharpWebRequests(IEnumerable<string> urls)
        {
            Console.WriteLine("Testing asynchronous C# web requests ...");

            var stopwatch = Stopwatch.StartNew();
            var pages = WebRequestsWithCSharp.Fetcher.AsyncExecuteWebRequests(urls);

            Console.WriteLine("{0} pages were requested; {1} were returned", urls.Count(), pages.Count(p => p.Length > 0));

            Console.WriteLine("Elapsed time was {0}", stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();

            Console.WriteLine();
        }

        private static void TestAsynchronousFSharpWebRequests(IEnumerable<string> urls)
        {
            Console.WriteLine("Testing asynchronous F# web requests ...");

            var stopwatch = Stopwatch.StartNew();
            var pages = FSharpFetcher.AsyncExecuteWebRequests(urls);

            Console.WriteLine("{0} pages were requested; {1} were returned", urls.Count(), pages.Count(p => p.Length > 0));

            Console.WriteLine("Elapsed time was {0}", stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            
            Console.WriteLine();
        }

        private static void TestSynchronousCSharpWebRequests(IEnumerable<string> urls)
        {
            Console.WriteLine("Testing synchronous C# web requests ...");

            var stopwatch = Stopwatch.StartNew();
            var pages = WebRequestsWithCSharp.Fetcher.ExecuteWebRequests(urls);

            Console.WriteLine("{0} pages were requested; {1} were returned", urls.Count(), pages.Count(p => p.Length > 0));

            Console.WriteLine("Elapsed time was {0}", stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();

            Console.WriteLine();
        }

        private static void TestSynchronousFSharpWebRequests(IEnumerable<string> urls)
        {
            Console.WriteLine("Testing synchronous F# web requests ...");

            var stopwatch = Stopwatch.StartNew();
            var pages = FSharpFetcher.ExecuteWebRequests(urls);

            Console.WriteLine("{0} pages were requested; {1} were returned", urls.Count(), pages.Count(p => p.Length > 0));

            Console.WriteLine("Elapsed time was {0}", stopwatch.ElapsedMilliseconds);
            stopwatch.Stop();
            
            Console.WriteLine();
        }
    }
}