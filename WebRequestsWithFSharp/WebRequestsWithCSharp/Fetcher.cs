namespace WebRequestsWithCSharp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Threading;

    public static class Fetcher
    {
        #region synchronous

        public static IEnumerable<string> ExecuteWebRequests(IEnumerable<string> urls)
        {
            var results = new List<string>();

            foreach (var url in urls)
            {
                results.Add(ExecuteWebRequest(url));
            }

            return results;
        }

        private static string ExecuteWebRequest(string url)
        {
            var request = WebRequest.Create(url);
            Console.Write("[{0}]\t", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Created web request for {0}", url);

            var response = request.GetResponse();
            Console.Write("[{0}]\t", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine("Getting the response for {0}", url);

            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                Console.Write("[{0}]\t", Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Reading the response for {0}", url);
                return reader.ReadToEnd();
            }
        }

        #endregion

        #region asynchronous

        private const int BufferSize = 1024;
        private static ManualResetEvent allDone = new ManualResetEvent(false);

        public static IEnumerable<string> AsyncExecuteWebRequests(IEnumerable<string> urls)
        {
            var results = new List<string>();

            Dictionary<WebRequest, IAsyncResult> resultDictionary = new Dictionary<WebRequest, IAsyncResult>();
            foreach (var url in urls)
            {
                var request = WebRequest.Create(url);
                Console.Write("[{0}]\t", Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Created web request for {0}", url);

                Console.Write("[{0}]\t", Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Getting response for {0}", url);
                resultDictionary.Add(
                    request,
                    request.BeginGetResponse(null, new RequestState { Request = request }));
            }

            foreach (var item in resultDictionary)
            {
                item.Value.AsyncWaitHandle.WaitOne();
                results.Add(new StreamReader(item.Key.EndGetResponse(item.Value).GetResponseStream()).ReadToEnd());
                Console.Write("[{0}]\t", Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("Reading response from {0}", item.Key.RequestUri);
            }

            return results;
        }

        #endregion
    }
}