using System;
using LibGit2Sharp;
using Pit.Logs;

namespace Pit.Git
{
    public class GitHelper
    {
        public static void CheckIfRepository()
        {
            try
            {
                using Repository repo = new Repository(Environment.CurrentDirectory);
            }
            catch (Exception e)
            {
                if (e is RepositoryNotFoundException)
                {
                    new Logger("GitHelper").Error(
                        "Not a Git repository!",
                        "Please navigate to a repository folder"
                        );
                    Environment.Exit(1);
                }
            }
        }
    }
}