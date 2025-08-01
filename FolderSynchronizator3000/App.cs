using FolderSynchronizator3000.Libs;
using FolderSynchronizator3000.Libs.ArgsParser;
using FolderSynchronizator3000.Libs.Logging;
using FolderSynchronizator3000.Libs.Sync;
using FolderSynchronizator3000.Models;

namespace FolderSynchronizator3000;
//["-source", "C:\\installDir", "-replica", "C:\\installDir_replica", "-interval", "16", "-logPath", "\\logsssss\\log.txt"]
internal class App(IParser parser, ISyncker syncker, ILog log)
{
    private readonly IParser _parser = parser ?? throw new ArgumentNullException(nameof(parser));
    private readonly ISyncker _syncker = syncker ?? throw new ArgumentNullException(nameof(syncker));
    private readonly ILog _log = log ?? throw new ArgumentNullException(nameof(log));

    private bool exitProgram = false;

    public void Run(string[] args)
    {
        var isParsed = _parser.ParseArguments(args, out var parsedArgs);

        if (!isParsed)
            Environment.Exit(1);

        Initialize(parsedArgs.LogPath);
        Syncronization(parsedArgs);
        Listening();

        Thread.Sleep(1 * 1000);
    }

    private void Syncronization(Arguments args)
    {
        Task.Run(() =>
        {
            while (!exitProgram)
            {
                _syncker.Sync(args.Source, args.Replica);

                for (int i = args.Interval; i > 0 && !exitProgram; i--)
                {
                    Console.Write($"\rNext sync in {i} second(s)...");
                    Thread.Sleep(1000);
                }

                Console.WriteLine();
            }
        });
    }

    private void Listening()
    {
        while (true)
        {
            var key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.E)
            {
                Console.Write("\n'E' pressed. Exiting...");
                exitProgram = true;
                break;
            }
        }
    }

    private void Initialize(string logPath)
    {
        _log.Init(logPath);
        Greeting.WriteGreeting();
    }
}
