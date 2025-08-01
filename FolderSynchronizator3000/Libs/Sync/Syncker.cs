using FolderSynchronizator3000.Libs.Helpers;
using FolderSynchronizator3000.Libs.Logging;

namespace FolderSynchronizator3000.Libs.Sync;

internal class Syncker(ILog log, IFileHelper fileHelper) : ISyncker
{
    public readonly ILog _log = log ?? throw new ArgumentNullException(nameof(log));
    public readonly IFileHelper _fileHelper = fileHelper ?? throw new ArgumentNullException(nameof(fileHelper));

    public void Sync(string sourcePath, string replicaPath)
    {
        if (StartSyncFiles(sourcePath, replicaPath))
        {
            _log.LogMessage("All files are equal. Sync will not be performed!");
        }
        else
        {
            _log.LogMessage($"Detected outdated files in: '{replicaPath}'. Performing sync...");
            PerformSync(sourcePath, replicaPath);
            _log.LogMessage("All files are synced successfully!");
        }
    }

    private static bool StartSyncFiles(string sourcePath, string replicaPath) =>
        Comparer.CompareDirectories(sourcePath, replicaPath) &&
        Comparer.CompareFiles(sourcePath, replicaPath);

    private void PerformSync(string source, string destination)
    {
        var sDirInfo = new DirectoryInfo(source);
        var dDirInfo = new DirectoryInfo(destination);

        // Sync dirs
        var sDirs = sDirInfo.GetDirectories();
        var dDirs = dDirInfo.GetDirectories();
        var dDirsDict = dDirs.ToDictionary(f => f.Name, StringComparer.OrdinalIgnoreCase);

        if (dDirs.Length > sDirs.Length)
        {
            var redundantDirs = dDirs
                .Where(d => !sDirs.Any(s => s.Name.Equals(d.Name, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            _fileHelper.RemoveDirs(redundantDirs);
        }

        foreach (var sDir in sDirs)
        {
            var newDestPath = Path.Combine(dDirInfo.FullName, sDir.Name);

            if (!dDirsDict.TryGetValue(sDir.Name, out var dDir) ||
                !Comparer.DirsAreEqual(sDir.FullName, dDir.FullName))
            {
                _fileHelper.CreateDir(sDir, dDirInfo.FullName);
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
            _fileHelper.RemoveFiles(redundantFiles);
        }

        foreach (var sFile in sFiles)
        {
            if (!dFilesDict.TryGetValue(sFile.Name, out var dFile) ||
                !Comparer.FilesAreEqual(sFile.FullName, dFile.FullName))
            {
                _fileHelper.MoveFiles(sFile, dDirInfo.FullName);
            }
        }
    }
}
