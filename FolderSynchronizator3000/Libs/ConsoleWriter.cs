using Microsoft.Extensions.Logging;

namespace FolderSynchronizator3000.Libs;

internal class ConsoleWriter
{
    #region Fields

    private readonly static object _messageLock = new();

    #endregion

    #region Main Methods

    public static void InfoMessage(string message) => WriteMessage(message, ConsoleColor.Blue);

    public static void WarningMessage(string message) => WriteMessage($"Warning message: {message}", ConsoleColor.Yellow);
    
    public static void SuccessMessage(string message) => WriteMessage(message, ConsoleColor.Green);

    public static void ErrorMessage(string message) => WriteMessage($"Error: {message}", ConsoleColor.Red);

    public static void Message(string message) => WriteMessage(message, ConsoleColor.White);

    #endregion

    #region Private Helper Methods

    private static void WriteMessage(string message, ConsoleColor backgroundColor)
    {
        lock (_messageLock)
        {
            Console.ForegroundColor = backgroundColor;
            Console.WriteLine(message);
            Console.ResetColor();
        }
    }

    #endregion
}
