using Microsoft.Extensions.Logging;
using Serilog;

namespace FolderSynchronizator3000.Libs.Logging;

internal class Log : ILog
{
    private ILogger<Program> _logger;

    public void LogMessage(string message, LogLevel logLevel)
    {
        _logger.Log(logLevel, message);
        Console.WriteLine(message);
    }

    public void Init(string logPath)
    {
        Serilog.Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

        using ILoggerFactory loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog();
        });

        _logger = loggerFactory.CreateLogger<Program>();
    }
}
