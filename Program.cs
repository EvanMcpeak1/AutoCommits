using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});

var logger = loggerFactory.CreateLogger<Program>();

try
{
    var repoPath = configuration["GitSettings:RepositoryPath"] ?? Directory.GetCurrentDirectory();
    var commitMessageTemplate = configuration["GitSettings:CommitMessage"] ?? "Automated daily commit";
    var commitMessage = commitMessageTemplate.Replace("{DATE}", DateTime.UtcNow.ToString("yyyy-MM-dd"));
    var branch = configuration["GitSettings:Branch"] ?? "main";
    var fileName = configuration["GitSettings:FileName"] ?? "daily-update.txt";
    var remoteName = configuration["GitSettings:RemoteName"] ?? "origin";

    logger.LogInformation("Starting automated GitHub push...");
    logger.LogInformation($"Repository Path: {repoPath}");
    logger.LogInformation($"Branch: {branch}");

    // Create or update the daily file
    var filePath = Path.Combine(repoPath, fileName);
    var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");
    var content = $"Last automated update: {timestamp}\n";
    
    await File.WriteAllTextAsync(filePath, content);
    logger.LogInformation($"Updated file: {fileName}");

    // Check git status
    if (!IsGitRepository(repoPath))
    {
        logger.LogError("The specified path is not a git repository!");
        return;
    }

    // Stage the file
    var stageResult = RunGitCommand(repoPath, "add", fileName);
    if (!stageResult.Success)
    {
        logger.LogError($"Failed to stage file: {stageResult.Error}");
        return;
    }
    logger.LogInformation("File staged successfully");

    // Check if there are changes to commit
    var statusResult = RunGitCommand(repoPath, "status", "--porcelain");
    if (string.IsNullOrWhiteSpace(statusResult.Output))
    {
        logger.LogInformation("No changes to commit. Exiting.");
        return;
    }

    // Commit the changes
    var commitResult = RunGitCommand(repoPath, "commit", "-m", commitMessage);
    if (!commitResult.Success)
    {
        logger.LogError($"Failed to commit: {commitResult.Error}");
        return;
    }
    logger.LogInformation("Changes committed successfully");

    // Push to GitHub
    var pushResult = RunGitCommand(repoPath, "push", remoteName, branch);
    if (!pushResult.Success)
    {
        logger.LogError($"Failed to push: {pushResult.Error}");
        logger.LogWarning("Make sure you have proper authentication set up (SSH keys or GitHub CLI)");
        return;
    }
    logger.LogInformation($"Successfully pushed to {remoteName}/{branch}");

    logger.LogInformation("Automated GitHub push completed successfully!");
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred during the automated push");
    Environment.Exit(1);
}

static bool IsGitRepository(string path)
{
    var gitDir = Path.Combine(path, ".git");
    return Directory.Exists(gitDir) || File.Exists(gitDir);
}

static GitCommandResult RunGitCommand(string workingDirectory, params string[] arguments)
{
    try
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "git",
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        foreach (var arg in arguments)
        {
            startInfo.ArgumentList.Add(arg);
        }

        using var process = Process.Start(startInfo);
        if (process == null)
        {
            return new GitCommandResult(false, "", "Failed to start git process");
        }

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();
        
        process.WaitForExit();

        return new GitCommandResult(process.ExitCode == 0, output, error);
    }
    catch (Exception ex)
    {
        return new GitCommandResult(false, "", ex.Message);
    }
}

record GitCommandResult(bool Success, string Output, string Error);

