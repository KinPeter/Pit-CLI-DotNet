# Pit CLI

A command line "toolbox" supporting Git and Jira workflows by some added functionality. *This was my first experiment with C# and the .NET Core framework.*

## Features:
- Config file handling - set your user info and Jira project credentials in a central config file
- Git:
  - Clean unused branches using a multi select list menu or with auto mode
  - Check out a branch by issue number or by a select menu
  - Check out the latest commit of a remote branch for Code Review - also by issue number or a select menu
  - Shortcuts to check out main branches and also refresh them with one short command
  - Set pre-defined user info for a repository or globally
- Jira:
  - Fetch base information about Jira issues by issue number
  - Create a branch parsing and using the Jira issue title as branch name

## Usage (Windows):
- Build the project
- Add its folder to the PATH
- Run `pit help` to get started

> It might be good to have PowerShell 7+ installed. I haven't tested with lower versions.

## Usage (Linux):
- Build the project
- Create an alias in `.bashrc` that will run the app dll with `dotnet`:
  ```bash
  alias pit='dotnet ~/path/to/pit.dll'
  ```
- Run `pit help` to get started

## Technologies used:
- C#
- .NET Core
- [LibGit2Sharp](https://github.com/libgit2/libgit2sharp)
- JIRA Rest APIs