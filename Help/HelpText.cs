namespace Pit.Help
{
    public class HelpText
    {
        public const string Main = @"
Pit has diferent modules which can be started by running 'pit <module> [<parameters>]'. 
You can choose from the below modules to run:

  review, rv              Reviewer - Checks out a remote branch for review.
  clean, cl               Branch Cleaner - Deletes local branches in a Git repository.
  
  help                    Displays this help page.
  debug                   Debug mode, manages a dummy repository.

Run 'pit <command> --help' or 'pit <command> -h' to get more info about each module.
";
        
        public const string Reviewer = @"
Checks out a remote branch for review.

Without parameters: displays a select menu to choose which remote branches to check out. 
Press <Enter> after selecting the branch, or <ESC> to cancel and quit. 

Optional parameters: 
  [string]                Looks for the given string in branch names and checks out the first one
                          it finds.
";
        
        public const string BranchCleaner = @"
Deletes local branches in a Git repository.

Without parameters: displays a multiselect menu to choose which local branches to delete. Use <Space> to 
select branches, press <Enter> when you're done with the selection, or <ESC> to cancel and quit. 

Optional parameters: 
  -a, --auto              Deletes all branches except for the current HEAD and master/develop branches.
  -p, --protect [string]  For auto mode you can set which branches to protect. The passed string can be
                          a comma separated list of identifers, eg. issue numbers. 
                          Example: 'pit clean -a -p 123,124,125'
";

        public const string RepoDebugger = @"
Manages a dummy repository to debug Pit functionalities. Setting an action as parameter is required.

Parameters:
  create                  Creates a directory 'PitDebug' with a repository under the current working
                          directory.
  delete                  Deletes the existing debug repository directory.
  reset                   Deletes the current debug directory and creates a new one.
";
    }
}