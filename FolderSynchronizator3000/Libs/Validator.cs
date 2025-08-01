namespace FolderSynchronizator3000.Libs;

internal enum CmdArgs
{
    Source = 0,
    Replica = 1,
    Interval = 2,
    Log = 3,
}

internal class Validator
{
    public static bool Validate(Dictionary<string, string> argsMap)
    {
        var source = argsMap["-source"];
        var replica = argsMap["-replica"];
        var interval = argsMap["-interval"];
        var logPath = argsMap["-logPath"];

        bool isSourceValid = ValidatePathes(source, CmdArgs.Source);
        bool isReplicaValid = ValidatePathes(replica, CmdArgs.Replica);
        bool isLogPathValid = ValidatePathes(logPath, CmdArgs.Log);
        bool isIntervalValid = ValidateInterval(interval);

        return isSourceValid &&
            isReplicaValid &&
            isLogPathValid && 
            isIntervalValid;
    }

    private static bool ValidatePathes(string path, CmdArgs arg)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            ConsoleWriter.ErrorMessage(string.Format("Path for '{0}' must not be empty or white space!", arg.ToString().ToLower()));
            return false;
        }
        else if (!Path.IsPathRooted(path))
        {
            ConsoleWriter.ErrorMessage(string.Format("Path for '{0}' must be absolute path!", arg.ToString().ToLower()));
            return false;
        }
        else if (arg != CmdArgs.Log && !Directory.Exists(path))
        {
            ConsoleWriter.ErrorMessage(string.Format("Path for '{0}' does not exists, please provide valid path!", arg.ToString().ToLower()));
            return false;
        }

        return true;
    }

    private static bool ValidateInterval(string value)
    {
        var isParsed = int.TryParse(value, out int interval);

        if (!isParsed)
        {
            ConsoleWriter.ErrorMessage("Interval value must be an integer");
            return false;
        }
        else if (interval <= 15)
        {
            ConsoleWriter.ErrorMessage("Interval value must be grater than 15");
            return false;
        }

        return true;
    }
}
