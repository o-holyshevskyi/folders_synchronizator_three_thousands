using FolderSynchronizator3000.Libs.ArgsParser;

namespace FolderSynchronizator3000;
//["-source", "C:\\installDir", "-replica", "C:\\installDir_replica", "-interval", "16", "-logPath", "\\logs\\log.txt"]
internal class App(IParser parser)
{
    private readonly IParser _parser = parser ?? throw new ArgumentNullException(nameof(parser));

    private bool exitProgram = false;

    public void Run(string[] args)
    {
        var isParsed = _parser.ParseArguments(["-source", "C:\\installDir", "-replica", "C:\\installDir_replica", "-interval", "16", "-logPath", "\\logs\\log.txt"], out Dictionary<string, string> parsedArgs);

        if (!isParsed)
            Environment.Exit(1);

        Syncronization(parsedArgs["-interval"]);
        Listening();

        Thread.Sleep(1 * 1000);
    }

    private void Syncronization(string interval)
    {
        int.TryParse(interval, out int period);

        ////initialize greeting and logs
        //Initialize.Init();
        //var logger = Initialize.InitLogger(parsedArgs["-logPath"]);

        //;
        //string sourcePath = parsedArgs["-source"];
        //string replicaPath = parsedArgs["-replica"];

        Task.Run(() =>
        {
            while (!exitProgram)
            {
                //Syncker.Sync(sourcePath, replicaPath, logger);
                //ConsoleWriter.Message("Listen for new content...");

                for (int i = period; i > 0 && !exitProgram; i--)
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
                //ConsoleWriter.Message("\n'E' pressed. Exiting...");
                exitProgram = true;
                break;
            }
        }
    }
}
