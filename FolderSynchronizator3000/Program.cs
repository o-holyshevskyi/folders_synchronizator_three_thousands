using FolderSynchronizator3000.Libs.DI;

namespace FolderSynchronizator3000;

public class Program
{
    static void Main(string[] args)
    {
        //Run app with registered Dependency Injections
        ServiceHelper.RegisterServices().Run(args);        
    }
}