using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MenuMb.Classes
{
    //public enum HttpMethodEnum
    //{
    //    GET = 0, POST = 1, PUT = 2, DELETE = 3
    //}

    internal static class HttpRequestHelper
    {
        private static HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(ConnectionServerSetings.ServerIp) };

        public async static Task<T?> GetAsync<T>(string link, string? param)
        {
            string full_link = param != null ? link + param : link;
            try
            {
                var response = await httpClient.GetFromJsonAsync<T>(full_link);
                return response;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async static Task<string?> PostAsync(string link, object? data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json,Encoding.UTF8,"application/json");

            try
            {
                var response = await httpClient.PostAsync(link,content);
                var text = await response.Content.ReadAsStringAsync();
                return text;
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
