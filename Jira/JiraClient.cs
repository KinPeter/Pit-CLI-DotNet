using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Pit.Config;
using Pit.Http;
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
        private readonly JiraProject config;
        private readonly string baseUrl;

        public JiraClient(string[] args) : base("Jira", args)
        {
            config = GetConfigForCurrentFolder();
            baseUrl = $"{config.Url}/rest/api/3";
        }

        public async Task RunAsync()
        {
            const string url = "https://jsonplaceholder.typicode.com/todos/1";
            PitHttp http = new PitHttp("Jira");
            
            Data data = await http.Get<Data>(url, JiraUtils.ConstructAuthHeader(config));

            Console.WriteLine(JiraUtils.ConstructAuthHeader(config).Value);
            
            Console.WriteLine($"userId: {data.UserId}");
            Console.WriteLine($"id: {data.Id}");
            Console.WriteLine($"title: {data.Title}");
            Console.WriteLine($"completed: {data.Completed}");

            Console.WriteLine("Done");
        }

        // private async Task GetIssueByKey(string key)
        // {
        //     var url = $"{baseUrl}/issue/{config.Prefix}-{key}";
        // }

        private JiraProject GetConfigForCurrentFolder()
        {
            try
            {
                PitConfig globalConfig = new ConfigFile().GetConfig();
                string[] path = Environment.CurrentDirectory.Split(Path.DirectorySeparatorChar);
                string currentFolder = path[^1];
                return globalConfig.JiraProjects.First(p => p.Folders.Contains(currentFolder));
            }
            catch (Exception e)
            {
                if (e is InvalidOperationException)
                {
                    Log.Error("Could not find Jira config for the current folder.");
                    Environment.Exit(1);
                }
                else throw;
            }

            return null;
        }

        public override void ShowHelp()
        {
            throw new System.NotImplementedException();
        }
    }
}