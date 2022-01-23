using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Robot.Linux;

public static class ShellHelper
{
    /// <summary>
    /// Set Bash commands on Linux
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static Task<int> Bash(this string cmd, ILogger logger)
    {
        var source = new TaskCompletionSource<int>();
        var escapedArgs = cmd.Replace("\"", "\\\"");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };
        process.Exited += (sender, args) =>
        {
            logger.LogWarning(process.StandardError.ReadToEnd());
            logger.LogInformation(process.StandardOutput.ReadToEnd());
            if (process.ExitCode == 0)
            {
                source.SetResult(0);
            }
            else
            {
                source.SetException(new Exception($"Command `{cmd}` failed with exit code `{process.ExitCode}`"));
            }

            process.Dispose();
        };

        try
        {
            process.Start();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Command {} failed", cmd);
            source.SetException(e);
        }

        return source.Task;
    }

    /// <summary>
    /// Set Sh commands on Linux
    /// </summary>
    /// <param name="cmd"></param>
    /// <param name="logger"></param>
    /// <returns></returns>
    public static async Task Sh(this string cmd, ILogger logger)
    {
        var source = new TaskCompletionSource<int>();
        var escapedArgs = cmd.Replace("\"", "\\\"");
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "sh",
                Arguments = $"-c \"{escapedArgs}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            },
            EnableRaisingEvents = true
        };
        //process.Exited += (sender, args) =>
        //{
        //    logger.LogWarning(process.StandardError.ReadToEnd());
        //    logger.LogInformation(process.StandardOutput.ReadToEnd());
        //    if (process.ExitCode == 0)
        //    {
        //        source.SetResult(0);
        //    }
        //    else
        //    {
        //        //source.SetException(new Exception($"Command `{cmd}` failed with exit code `{process.ExitCode}`"));
        //    }

        //    process.Dispose();
        //};

        try
        {
            process.Start();
            
        }
        catch (Exception e)
        {
            logger.LogError(e, "Command {} failed", cmd);
            source.SetException(e);
        }

    
    }
}


