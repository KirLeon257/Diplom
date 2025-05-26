using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace MenuMb.Classes
{
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
                MessageBox.Show("Не удалось выполнить запрос!");
                return default;
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
                MessageBox.Show("Не удалось выполнить запрос!");
                return null;
            }
        }

        public async static Task<string?> DeleteAsync(string link,string? param)
        {
            var fulllink = param != null ? link + param : link;
            try
            {
                var response = await httpClient.DeleteAsync(fulllink);
                var text = await response.Content.ReadAsStringAsync();
                return text;
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось выполнить запрос!");
                return null ;
            }
        }

        internal static async Task<string?> PutAsync(string link, object? data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PutAsync(link, content);
                var text = await response.Content.ReadAsStringAsync();
                return text;
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось выполнить запрос!");
                return null;
            }
        }
    }
}
