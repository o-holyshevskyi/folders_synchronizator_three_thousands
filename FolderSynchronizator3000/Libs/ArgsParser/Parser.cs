using FolderSynchronizator3000.Models;

namespace FolderSynchronizator3000.Libs.ArgsParser;

internal class Parser : IParser
{
    private readonly string[] requiredCommandLineArgs = [ "-source", "-replica", "-interval", "-logPath" ];

    public bool ParseArguments(string[] args, out Arguments result)
    {
        result = new Arguments();

        var argsDictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (args.Length % 2 != 0)
        {
            Console.Write("Each argument must have a corresponding value.");
            return false;
        }

        for (int i = 0; i < args.Length; i += 2)
        {
            string key = args[i];
            string value = args[i + 1];

            if (!key.StartsWith('-'))
            {
                Console.Write($"Invalid argument format: '{key}'. Expected a flag starting with '-'.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(value) || value.StartsWith("-"))
            {
                Console.Write($"Missing or invalid value for the '{key}' argument.");
                return false;
            }

            argsDictionary[key] = value;
        }

        var parsedArgs = argsDictionary;
        var missingArgs = requiredCommandLineArgs.Where(arg => !parsedArgs.ContainsKey(arg)).ToList();
        if (missingArgs.Count > 0)
        {
            Console.Write($"Missing required arguments: {string.Join(", ", missingArgs)}");
            return false;
        }

        if (!Validator.Validate(argsDictionary))
           return false;

        result = GetArguments(argsDictionary);

        return true;
    }

    private Arguments GetArguments(Dictionary<string, string> argsDictionary)
    {
        return new Arguments
        {
            Source = argsDictionary["-source"],
            Replica = argsDictionary["-replica"],
            Interval = int.Parse(argsDictionary["-interval"]),
            LogPath = argsDictionary["-logPath"]
        };
    }
}
