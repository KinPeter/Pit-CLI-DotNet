using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pit.Config;
using Pit.Git;
using Pit.Help;
using Pit.Http;
using Pit.Jira.Types;
using Pit.Types;

namespace Pit.Jira
{
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
            if (Args.Length == 1 && (Args[0] == "-h" || Args[0] == "--help"))
            {
                ShowHelp();
                return;
            }

            if (Args.Length == 0)
            {
                HandleMissingParams();
                return;
            }

            if (Args.Length == 1)
            {
                await GetIssueByKey(Args[0]);
                return;
            }

            if (Args.Length == 2 && (Args.Contains("-c") || Args.Contains("--create")))
            {
                await GetIssueAndCreateBranch(Args);
                return;
            }

            HandleUnknownParam();
        }

        private async Task GetIssueAndCreateBranch(string[] args)
        {
            try
            {
                var issueKey = args.First(a => !a.Contains('c')).ToString();
                Issue issue = await GetIssueByKey(issueKey);
                GitUtils.CreateBranch(GetSuggestedBranchName(issue));
            }
            catch (Exception)
            {
                HandleUnknownParam();
            }
        }

        private async Task<Issue> GetIssueByKey(string key)
        {
            var url = $"{baseUrl}/issue/{config.Prefix}-{key}";
            PitHttp http = new PitHttp("Jira");
            
            Issue issue = await http.Get<Issue>(url, JiraUtils.ConstructAuthHeader(config));

            WriteIssueToConsole(issue);
            return issue;
        }

        private void WriteIssueToConsole(Issue issue)
        {
            string[] componentNames = issue.Fields.Components.Select(c => c.Name).ToArray();
            string components = componentNames.Length == 0
                ? "<none>"
                : string.Join(", ", componentNames);

            Console.WriteLine();
            Log.Blue($"{issue.Key}: {issue.Fields.Summary}");
            WriteWithColors("Type:       ", issue.Fields.IssueType.Name);
            WriteWithColors("Components: ", components);

            WriteWithColors("\nStatus:     ", issue.Fields.Status.Name);

            if (issue.Fields.Resolution != null)
            {
                WriteWithColors("Resolution: ", issue.Fields.Resolution.Name);
            }

            WriteWithColors("\nReporter:   ", issue.Fields.Reporter.DisplayName);

            if (issue.Fields.Assignee != null)
            {
                WriteWithColors("Assignee:   ", issue.Fields.Assignee.DisplayName);
            }

            WriteWithColors("\nLink:       ", $"{config.Url}/browse/{issue.Key}");

            if (issue.Fields.Parent != null)
            {
                WriteWithColors("\nParent:     ", $"{issue.Fields.Parent.Key}: {issue.Fields.Parent.Fields.Summary}");
                WriteWithColors("Link:       ", $"{config.Url}/browse/{issue.Fields.Parent.Key}");
            }
            
            WriteWithColors("\n\nSuggested:  ", GetSuggestedBranchName(issue));
            Console.WriteLine('\n');
        }

        private void WriteWithColors(string str1, string str2)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"\n{str1}");
            Console.ResetColor();
            Console.Write(str2);
        }

        private string GetSuggestedBranchName(Issue issue)
        {
            const string specialCharactersRegex = @"[$&+,:;=?¿@#|'<>.^*()%!¡~/`\][{}\\_-]";
            string branchType = issue.Fields.IssueType.Name == "Bug"
                ? "bugfix"
                : "feature";
            string lowerCaseString = issue.Fields.Summary.ToLower();
            string withoutSpecials = Regex.Replace(lowerCaseString, specialCharactersRegex, "");
            string withoutSpaces = withoutSpecials.Replace(" ", "-");
            return $"{branchType}/{issue.Key}-{withoutSpaces}";
        }

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
            Log.Info("Usage:");
            Console.WriteLine(HelpText.Jira);
        }
    }
}