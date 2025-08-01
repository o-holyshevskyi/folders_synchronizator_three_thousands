namespace FolderSynchronizator3000.Libs.ArgsParser;

public interface IParser
{
    bool ParseArguments(string[] args, out Dictionary<string, string> result);
}
