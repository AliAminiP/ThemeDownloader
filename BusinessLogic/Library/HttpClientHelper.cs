using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Library
{
    public interface IHttpClientHelper
    {

        Task DownloadFileAsync(string url, string pathToSave);
        Task<string> GetRequestAsync(string url);
    }
    public class HttpClientHelper : IHttpClientHelper
    {
        public async Task<string> GetRequestAsync(string url)
        {
            string CallResult = string.Empty;
            if (string.IsNullOrEmpty(url))
            {
                return CallResult;
            }

            using (var client = new HttpClient())
            {
                var Result = await client.GetAsync(url);
                if (Result.IsSuccessStatusCode)
                {
                    CallResult = await Result.Content.ReadAsStringAsync();
                }
            }
            return CallResult;
        }
        public async Task DownloadFileAsync(string url, string pathToSave)
        {
            using (var client = new WebClient())
            {
                try
                {
                    await client.DownloadFileTaskAsync(url, pathToSave);
                }
                //if throw web exception means file not found or server error
                catch (WebException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}
