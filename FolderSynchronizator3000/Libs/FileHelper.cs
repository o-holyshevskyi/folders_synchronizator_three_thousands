namespace FolderSynchronizator3000.Libs;

internal class FileHelper
{
    public static void MoveFiles(FileInfo fileInfo, string destination)
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
            ConsoleWriter.SuccessMessage(
                $"File '{Path.GetFileName(fileInfo.FullName)}' successfully moved from '{fileInfo.FullName}' to '{copyDestDir}'");
        }
        catch (Exception ex)
        {
            ConsoleWriter.ErrorMessage(ex.Message);
            throw;
        }
    }

    public static void CreateDir(DirectoryInfo dirInfo, string destination)
    {
        try
        {
            var dirName = dirInfo.Name;
            var dirPath = Path.Combine(destination, dirName);

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                ConsoleWriter.SuccessMessage(
                    $"Directory '{dirPath}' successfully created from '{dirInfo.FullName}' to '{dirPath}'");
            }
        }
        catch (Exception ex)
        {
            ConsoleWriter.ErrorMessage(ex.Message);
            throw;
        }
    }

    public static void RemoveDir(DirectoryInfo dirInfo)
    {
        dirInfo.Delete(true);
        ConsoleWriter.SuccessMessage($"Directory '{dirInfo.Name}' successfully removed from '{dirInfo.Parent}'");
    }

    public static void RemoveDirs(List<DirectoryInfo> dirInfoList) => dirInfoList.ForEach(RemoveDir);

    public static void RemoveFile(FileInfo fileInfo)
    {
        fileInfo.Delete();
        ConsoleWriter.SuccessMessage($"File '{fileInfo.Name}' successfully removed from '{fileInfo.DirectoryName}'");
    }

    public static void RemoveFiles(List<FileInfo> dirInfoList) => dirInfoList.ForEach(RemoveFile);
}
