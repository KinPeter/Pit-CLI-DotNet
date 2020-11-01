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

## Usage:
- Build the project
- Add its folder to the PATH
- Run `pit help` to get started

> As platform only Windows is supported at this point and it might be good to have PowerShell 7+ installed. Maybe one day I'll make it work on Linux too...

## Technologies used:
- C#
- .NET Core
- [LibGit2Sharp](https://github.com/libgit2/libgit2sharp)
- JIRA Rest APIs