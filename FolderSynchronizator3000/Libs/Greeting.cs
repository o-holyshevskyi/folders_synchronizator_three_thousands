using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FolderSynchronizator3000.Libs;

internal class Greeting
{
    public static void WriteGreeting()
    {
        ConsoleWriter.InfoMessage("Welcome to Folder Synchronizator 3000");
        ConsoleWriter.Message("<<<<<<<<---------->>>>>>>>");
        ConsoleWriter.InfoMessage($"Launched from: \"{Environment.CurrentDirectory}\"");
        ConsoleWriter.InfoMessage($"Physical location: \"{AppDomain.CurrentDomain.BaseDirectory}\"");
        ConsoleWriter.InfoMessage($"AppContext.BaseDir: \"{AppContext.BaseDirectory}\"");
        ConsoleWriter.InfoMessage($"Runtime Call: \"{Path.GetDirectoryName(Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty)}\"");
        ConsoleWriter.InfoMessage("Press 'E' to exit the application");
    }
}
