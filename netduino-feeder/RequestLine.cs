using System;
using Microsoft.SPOT;

namespace NetduinoFeeder
{
    class RequestLine
    {
        public RequestLine(string line) 
        {
            var elements = line.Split(' ');

            Verb = elements[0];
            Query = elements[1];
        }

        public string Verb { get; private set; }

        public string Query { get; private set; }
        
        public static class Verbs 
        {
            public const string GET = "GET";
            public const string POST = "POST";
            public const string PUT = "PUT";
        }
        
    }
}
