# GitHub Automated Daily Push

A C# console application that automatically makes a small commit and pushes it to GitHub every day

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

1. **Clone the repository:**
   ```bash
   git clone https://github.com/EvanMcpeak1/AutoCommits
   cd AutoCommits
   ```

2. **Configure authentication**:
   - **Option 1 - GitHub CLI**: Install GitHub CLI and authenticate
     ```bash
     gh auth login
     ```
   - **Option 2 - Personal Access Token**: Use a token in the remote URL (less secure)
     ```bash
     git remote set-url origin https://token@github.com/username/repo.git
     ```

3. **Configure settings** in `appsettings.json`:
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

4. **Build the project**:
   ```bash
   dotnet build
   ```

5. **Test run**:
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

## How It Works

1. The application updates a text file with the current timestamp
2. Stages the file using `git add`
3. Commits the changes with your configured message
4. Pushes to the specified branch on GitHub
