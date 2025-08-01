using FolderSynchronizator3000.Models;

namespace FolderSynchronizator3000.Libs.ArgsParser;

public interface IParser
{
    bool ParseArguments(string[] args, out Arguments result);
}
