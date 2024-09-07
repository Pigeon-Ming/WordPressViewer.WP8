using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WordPressViewer.Model
{
    public class HttpClientManager
    {
        public static async Task<string> GetRequest(string URL)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(URL);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsStringAsync();
            else
                return "";
        }

        public static async Task<string> PostRequest(string URL, string body)
        {
            var httpClient = new HttpClient();
            HttpContent content = new StringContent(body);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var response = await httpClient.PostAsync(URL, content);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
