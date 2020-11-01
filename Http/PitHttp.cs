using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Pit.Logs;

namespace Pit.Http
{
    public class PitHttp
    {
        private readonly Logger log;

        public PitHttp(string module)
        {
            log = new Logger($"Http:{module}");
        }

        public async Task<T> Get<T>(string url, List<PitHeader> headers = null)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            AddOptionalHeaders(request, headers);
            return await PerformGet<T>(request);
        }

        public async Task<T> Get<T>(string url, PitHeader header)
        {
            using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add(header.Key, header.Value);
            return await PerformGet<T>(request);
        }

        private async Task<T> PerformGet<T>(HttpRequestMessage request)
        {
            try
            {
                request.Headers.Add(HttpRequestHeader.ContentType.ToString(), "application/json");
                request.Headers.Add(HttpRequestHeader.Accept.ToString(), "application/json");

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException($"{response.StatusCode.GetHashCode()}: {response.ReasonPhrase}");

                HttpContent content = response.Content;

                return await content.ReadFromJsonAsync<T>();
            }
            catch (Exception e)
            {
                log.Error($"An error occured during the request to {request.RequestUri}", e.Message);
                Environment.Exit(1);
            }

            return default(T);
        }

        private void AddOptionalHeaders(HttpRequestMessage request, List<PitHeader> headers)
        {
            if (headers == null || headers.Count <= 0) return;
            foreach (PitHeader header in headers)
            {
                request.Headers.Add(header.Key, header.Value);
            }
        }
    }
}