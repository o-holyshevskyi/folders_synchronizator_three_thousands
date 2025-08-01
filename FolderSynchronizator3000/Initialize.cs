using FolderSynchronizator3000.Libs;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;

namespace FolderSynchronizator3000;

internal class Initialize
{
    public static void Init()
    {
        Greeting.WriteGreeting();
    }

    public static ILogger<Program> InitLogger(string logPath)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog();
        });

        var logger = loggerFactory.CreateLogger<Program>();

        ConsoleWriter.WarningMessage($"Write logs in the: '{logPath}'");

        return logger;
    }
}
