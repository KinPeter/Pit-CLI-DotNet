namespace Pit.Help
{
    public static class HelpText
    {
        public const string Main = @"
Pit has diferent modules which can be started by running 'pit <module> [<parameters>]'. 
You can choose from the below modules to run:

  checkout, co            Checkout - Checks out a branch.
  review, rv              Reviewer - Checks out a remote branch for review.
  clean, cl               Branch Cleaner - Deletes local branches in a Git repository.
  pulo                    Pulls from origin from the current branch.
  user                    Gets/sets Git user email locally or globally.
  
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
    
        
        public const string Checkout = @"
Checks out a branch.

Without parameters: displays a select menu to choose which branches to check out. 
Press <Enter> after selecting the branch, or <ESC> to cancel and quit. 

Shortcut options:
  com                     Checks out the 'master' or 'main' branch (whichever exists).
  cod                     Checks out the 'develop' branch. 
  -r                      Used with the 'com' or 'cod' command, also performs a pull.

Optional parameters for the 'checkout' or 'co' commands: 
  [string]                Looks for the given string in branch names and checks out the first one
                          it finds.
";
        
        public const string BranchCleaner = @"
Deletes local branches in a Git repository.

Without parameters: displays a multiselect menu to choose which local branches to delete. Use <Space> to 
select branches, press <Enter> when you're done with the selection, or <ESC> to cancel and quit. 

Optional parameters: 
  -a, --auto              Deletes all branches except the current HEAD and master/main/develop branches.
  -p, --protect [string]  For auto mode you can set which branches to protect. The passed string can be
                          a comma separated list of identifers, eg. issue numbers. 
                          Example: 'pit clean -a -p 123,124,125'
";        
        public const string Puller = @"
Pulls from origin from the branch with the same name as pointed at the current HEAD.

No optional parameters.
";

        public const string RepoDebugger = @"
Manages a dummy repository to debug Pit functionalities. Setting an action as parameter is required.

Parameters:
  create                  Creates a directory 'PitDebug' with a repository under the current working
                          directory.
  delete                  Deletes the existing debug repository directory.
  reset                   Deletes the current debug directory and creates a new one.
";

        public const string User = @"
Gets or sets the global or local (for repository) Git user email according to the given parameters.

Without parameters: Gets the local user email set for the current repository.

Parameters:
  -g, --global            Sets the config level to Global
  -p, --personal          Sets the Git user email to the pre-set personal email address
  -w, --work              Sets the Git user email to the pre-set work email address
  
Configuration:
Create a '.pitconfig' file in your user directory. Use the ini file syntax, and set the values as on
the example below:

[git_user]
    personal_email=myemail@gmail.com
    work_email=myname@mycompany.org
";
    }
}