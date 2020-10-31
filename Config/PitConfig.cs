using System;
using System.Collections.Generic;
using System.Linq;
using Pit.Logs;

namespace Pit.Config
{
    public class GitUser
    {
        public string PersonalEmail { get; }
        public string WorkEmail { get; }

        public GitUser(string personalEmail, string workEmail)
        {
            PersonalEmail = personalEmail;
            WorkEmail = workEmail;
        }
    }

    public class JiraProject
    {
        public string Name { get; }
        public string Url { get; }
        public string[] Folders { get; }
        public string User { get; }
        public string Password { get; }

        public JiraProject(string name, string url, string[] folders, string user, string password)
        {
            Name = name;
            Url = url;
            Folders = folders;
            User = user;
            Password = password;
        }
    }

    public class PitConfig
    {
        private static readonly Logger Log = new Logger("PitConfig");

        public GitUser GitUser { get; }
        public JiraProject[] JiraProjects { get; }

        private PitConfig(GitUser gitUser, JiraProject[] jiraProjects)
        {
            GitUser = gitUser;
            JiraProjects = jiraProjects;
        }

        public static PitConfig FromDictionary(ConfigDictionary dictionary)
        {
            ValidateDictionary(dictionary);
            return new PitConfig(
                GetGitUser(dictionary),
                GetJiraProjects(dictionary)
            );
        }

        private static void ValidateDictionary(ConfigDictionary dictionary)
        {
            if (
                !dictionary.ContainsKey("git_user") ||
                !dictionary["git_user"].ContainsKey("personal_email") ||
                !dictionary["git_user"].ContainsKey("work_email")
            )
            {
                Log.Error("Config file does not contain the necessary values.");
                Environment.Exit(1);
            }
        }

        private static GitUser GetGitUser(ConfigDictionary dictionary)
        {
            return new GitUser(
                dictionary["git_user"]["personal_email"],
                dictionary["git_user"]["work_email"]
            );
        }

        private static JiraProject[] GetJiraProjects(ConfigDictionary dictionary)
        {
            string[] requiredKeys = {"name", "folders", "url", "user", "password"};
            var projects = new List<JiraProject>();
            string[] jiraKeys = dictionary.Keys.Where(k => k.StartsWith("jira_")).ToArray();

            if (jiraKeys.Length == 0) return null;

            foreach (string key in jiraKeys)
            {
                var jiraDict = dictionary[key];
                if (!jiraDict.Keys.All(k => requiredKeys.Contains(k)))
                {
                    Log.Error($"Required data is missing in config: {jiraDict}");
                    Environment.Exit(1);
                }

                projects.Add(new JiraProject(
                    jiraDict["name"],
                    jiraDict["url"],
                    jiraDict["folders"].Split(','),
                    jiraDict["user"],
                    jiraDict["password"]
                ));
            }

            return projects.ToArray();
        }
    }
}