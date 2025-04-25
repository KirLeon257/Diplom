using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MenuMb.Classes
{
    public enum HttpMethodEnum
    {
        GET = 0, POST = 1, PUT = 2, DELETE= 3
    }

    internal static class HttpRequestHelper
    {
        private static HttpClient httpClient = new HttpClient();
        //public static object? SendRequest(HttpMethodEnum method,string, object? data)
        //{
            
        //}
    }
}
