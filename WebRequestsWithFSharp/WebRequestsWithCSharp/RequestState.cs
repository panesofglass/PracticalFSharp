namespace WebRequestsWithCSharp
{
    using System.IO;
    using System.Net;
    using System.Text;

    internal class RequestState
    {
        private const int BufferSize = 1024;
        public byte[] BufferRead;
        public WebRequest Request;
        public StringBuilder RequestData;
        public Stream ResponseStream;
        public Decoder StreamDecode = Encoding.UTF8.GetDecoder();

        public RequestState()
        {
            this.BufferRead = new byte[BufferSize];
            this.RequestData = new StringBuilder(string.Empty);
            this.Request = null;
            this.ResponseStream = null;
        }
    }
}