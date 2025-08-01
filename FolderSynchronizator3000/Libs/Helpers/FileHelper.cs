using FolderSynchronizator3000.Libs.Logging;

namespace FolderSynchronizator3000.Libs.Helpers;

internal class FileHelper(ILog log) : IFileHelper
{
    public readonly ILog _log = log ?? throw new ArgumentNullException(nameof(log));

    public void MoveFiles(FileInfo fileInfo, string destination)
    {
        try
        {
            var relPath = Path.GetRelativePath(fileInfo.DirectoryName, fileInfo.FullName);
            var destPath = Path.Combine(destination, relPath);
            var destDir = Path.GetDirectoryName(destPath);

            if (!Directory.Exists(destDir))
                Directory.CreateDirectory(destDir);

            string copyDestDir = Path.Combine(destDir, Path.GetFileName(fileInfo.FullName));
            fileInfo.CopyTo(destPath, true);
            _log.LogMessage(
                $"File '{Path.GetFileName(fileInfo.FullName)}' successfully moved from '{fileInfo.FullName}' to '{copyDestDir}'");
        }
        catch (Exception ex)
        {
            _log.LogMessage(ex.Message);
            throw;
        }
    }

    public void CreateDir(DirectoryInfo dirInfo, string destination)
    {
        try
        {
            var dirName = dirInfo.Name;
            var dirPath = Path.Combine(destination, dirName);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                _log.LogMessage(
                    $"Directory '{dirPath}' successfully created from '{dirInfo.FullName}' to '{dirPath}'");
            }
        }
        catch (Exception ex)
        {
            _log.LogMessage(ex.Message);
            throw;
        }
    }

    public void RemoveDir(DirectoryInfo dirInfo)
    {
        dirInfo.Delete(true);
        _log.LogMessage($"Directory '{dirInfo.Name}' successfully removed from '{dirInfo.Parent}'");
    }

    public void RemoveDirs(List<DirectoryInfo> dirInfoList) => dirInfoList.ForEach(RemoveDir);

    public void RemoveFile(FileInfo fileInfo)
    {
        fileInfo.Delete();
        _log.LogMessage($"File '{fileInfo.Name}' successfully removed from '{fileInfo.DirectoryName}'");
    }

    public void RemoveFiles(List<FileInfo> dirInfoList) => dirInfoList.ForEach(RemoveFile);
}
