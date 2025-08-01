namespace FolderSynchronizator3000.Libs.ArgsParser;

internal class Parser : IParser
{
    private readonly string[] requiredCommandLineArgs = [ "-source", "-replica", "-interval", "-logPath" ];

    public bool ParseArguments(string[] args, out Dictionary<string, string> result)
    {
        result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (args.Length % 2 != 0)
        {
            ConsoleWriter.ErrorMessage("Each argument must have a corresponding value.");
            return false;
        }

        for (int i = 0; i < args.Length; i += 2)
        {
            string key = args[i];
            string value = args[i + 1];

            if (!key.StartsWith('-'))
            {
                ConsoleWriter.ErrorMessage($"Invalid argument format: '{key}'. Expected a flag starting with '-'.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(value) || value.StartsWith("-"))
            {
                ConsoleWriter.ErrorMessage($"Missing or invalid value for the '{key}' argument.");
                return false;
            }

            result[key] = value;
        }

        var parsedArgs = result;
        var missingArgs = requiredCommandLineArgs.Where(arg => !parsedArgs.ContainsKey(arg)).ToList();
        if (missingArgs.Count > 0)
        {
            ConsoleWriter.ErrorMessage($"Missing required arguments: {string.Join(", ", missingArgs)}");
            return false;
        }

        if (!Validator.Validate(result))
           return false;

        return true;
    }
}
