<<<<<<< HEAD
# GitHub Automated Daily Push

A C# console application that automatically makes a small commit and pushes it to GitHub every day, without requiring YAML files or GitHub Actions.

## Features

- Automatically updates a file with a timestamp
- Commits and pushes changes to GitHub
- Configurable via JSON settings file
- Can be scheduled using Windows Task Scheduler

## Prerequisites

- .NET 8.0 SDK or later
- Git installed and configured
- A GitHub repository with proper authentication set up (SSH keys or GitHub CLI)

## Setup

1. **
   ```bash
   git clone <your-repo-url>
   cd <your-repo-name>
   ```

2. **Place this project** inside your repository directory, or update the `RepositoryPath` in `appsettings.json`

3. **Configure authentication**:
   - **Option 1 - SSH (Recommended)**: Set up SSH keys with GitHub
     ```bash
     git remote set-url origin git@github.com:username/repo.git
     ```
   - **Option 2 - GitHub CLI**: Install GitHub CLI and authenticate
     ```bash
     gh auth login
     ```
   - **Option 3 - Personal Access Token**: Use a token in the remote URL (less secure)
     ```bash
     git remote set-url origin https://token@github.com/username/repo.git
     ```

4. **Configure settings** in `appsettings.json`:
   ```json
   {
     "GitSettings": {
       "RepositoryPath": ".",
       "CommitMessage": "Automated daily commit",
       "Branch": "main",
       "FileName": "daily-update.txt",
       "RemoteName": "origin"
     }
   }
   ```

5. **Build the project**:
   ```bash
   dotnet build
   ```

6. **Test run**:
   ```bash
   dotnet run
   ```

## Scheduling (Windows Task Scheduler)

1. Open Task Scheduler (`taskschd.msc`)

2. Create a new task:
   - General tab: Give it a name, select "Run whether user is logged on or not"
   - Triggers tab: Create a new trigger - Daily, set time (e.g., 2:00 AM)
   - Actions tab: Start a program
     - Program: `dotnet`
     - Arguments: `run --project "C:\path\to\GitHubAutoPush\GitHubAutoPush.csproj"`
     - Start in: `C:\path\to\GitHubAutoPush`

3. **Alternative**: Create a batch file for easier scheduling:
   ```batch
   @echo off
   cd /d C:\path\to\GitHubAutoPush
   dotnet run
   ```
   Then schedule this batch file in Task Scheduler.

## Configuration Options

- **RepositoryPath**: Path to your git repository (use "." for current directory)
- **CommitMessage**: Message for the commit (you can use {DATE} placeholder)
- **Branch**: Branch to push to (default: "main")
- **FileName**: Name of the file to update daily (default: "daily-update.txt")
- **RemoteName**: Git remote name (default: "origin")

## How It Works

1. The application updates a text file with the current timestamp
2. Stages the file using `git add`
3. Commits the changes with your configured message
4. Pushes to the specified branch on GitHub


=======
# AutoCommits
Uploads autocommits to show activity
>>>>>>> a2b4ea31ce3aa06530b49fc8d2563052a1182f2f
