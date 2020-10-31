using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Pit.Types;

namespace Pit.Jira
{
    public class Data
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }

    public class JiraClient : PitAction, IPitActionAsync
    {
        public JiraClient(string[] args) : base("Jira", args) { }

        public async Task RunAsync()
        {
            HttpClient httpClient = new HttpClient();

            const string url = "https://jsonplaceholder.typicode.com/todos/1";

            Console.WriteLine("Sending request");

            // HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            // HttpResponseMessage res = await httpClient.SendAsync(request);

            HttpResponseMessage res = await httpClient.GetAsync(url);

            Console.WriteLine(res.ToString());

            if (res.IsSuccessStatusCode)
            {
                HttpContent content = res.Content;
                Data data = await content.ReadFromJsonAsync<Data>();
                Console.WriteLine($"userId: {data.UserId}");
                Console.WriteLine($"id: {data.Id}");
                Console.WriteLine($"title: {data.Title}");
                Console.WriteLine($"completed: {data.Completed}");
            }

            Console.WriteLine("Done");
        }

        public override void ShowHelp()
        {
            throw new System.NotImplementedException();
        }
    }
}