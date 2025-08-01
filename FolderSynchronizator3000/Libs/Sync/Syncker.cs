using Microsoft.Extensions.Logging;

namespace FolderSynchronizator3000.Libs.Sync;

internal class Syncker
{
    public static void Sync(string sourcePath, string replicaPath, ILogger<Program> logger)
    {
        if (StartSyncFiles(sourcePath, replicaPath))
        {
            ConsoleWriter.WarningMessage("All files are equal. Sync will not be performed!");
        }
        else
        {
            ConsoleWriter.WarningMessage($"Detected outdated files in: '{replicaPath}'. Performing sync...");
            PerformSync(sourcePath, replicaPath);
            ConsoleWriter.SuccessMessage("All files are synced successfully!");
        }
    }

    private static bool StartSyncFiles(string sourcePath, string replicaPath) =>
        Comparer.CompareDirectories(sourcePath, replicaPath) &&
        Comparer.CompareFiles(sourcePath, replicaPath);

    private static void PerformSync(string source, string destination)
    {
        var sDirInfo = new DirectoryInfo(source);
        var dDirInfo = new DirectoryInfo(destination);

        var sDirs = sDirInfo.GetDirectories();
        var dDirs = dDirInfo.GetDirectories();
        var dDirsDict = dDirs.ToDictionary(f => f.Name, StringComparer.OrdinalIgnoreCase);

        if (dDirs.Length > sDirs.Length)
        {
            var redundantDirs = dDirs
                .Where(d => !sDirs.Any(s => s.Name.Equals(d.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            FileHelper.RemoveDirs(redundantDirs);
        }

        foreach (var sDir in sDirs)
        {
            var newDestPath = Path.Combine(dDirInfo.FullName, sDir.Name);

            if (!dDirsDict.TryGetValue(sDir.Name, out var dDir) ||
                !Comparer.DirsAreEqual(sDir.FullName, dDir.FullName))
            {
                FileHelper.CreateDir(sDir, dDirInfo.FullName);
            }

            PerformSync(sDir.FullName, newDestPath);
        }

        // Sync files
        var sFiles = sDirInfo.GetFiles();
        var dFiles = dDirInfo.GetFiles();
        var dFilesDict = dFiles.ToDictionary(f => f.Name, StringComparer.OrdinalIgnoreCase);

        if (dFiles.Length > sFiles.Length)
        {
            var redundantFiles = dFiles
                .Where(d => !sFiles.Any(s => s.Name.Equals(d.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            FileHelper.RemoveFiles(redundantFiles);
        }

        foreach (var sFile in sFiles)
        {
            if (!dFilesDict.TryGetValue(sFile.Name, out var dFile) ||
                !Comparer.FilesAreEqual(sFile.FullName, dFile.FullName))
            {
                FileHelper.MoveFiles(sFile, dDirInfo.FullName);
            }
        }
    }
}
