using System;
using System.Collections.Generic;
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

    public class PitConfig
    {
        public GitUser GitUser { get; }

        private PitConfig(GitUser gitUser)
        {
            GitUser = gitUser;
        }

        public static PitConfig FromDictionary(Dictionary<string, Dictionary<string, string>> dictionary)
        {
            if (
                !dictionary.ContainsKey("git_user") ||
                !dictionary["git_user"].ContainsKey("personal_email") ||
                !dictionary["git_user"].ContainsKey("work_email")
            )
            {
                new Logger("PitConfig").Error("Config file does not contain the necessary values.");
                Environment.Exit(1);
            }
            return new PitConfig(
                new GitUser(
                    dictionary["git_user"]["personal_email"],
                    dictionary["git_user"]["work_email"]
                )
            );
        }
    }
}