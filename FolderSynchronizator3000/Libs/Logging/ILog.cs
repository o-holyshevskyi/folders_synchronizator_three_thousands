namespace FolderSynchronizator3000.Libs.Logging;

public interface ILog
{
    void Init(string logPath);
    void LogMessage(string message);
}
