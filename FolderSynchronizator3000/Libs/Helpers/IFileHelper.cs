namespace FolderSynchronizator3000.Libs.Helpers;

public interface IFileHelper
{
    void CreateDir(DirectoryInfo dirInfo, string destination);
    void MoveFiles(FileInfo fileInfo, string destination);
    void RemoveDirs(List<DirectoryInfo> dirInfoList);
    void RemoveFiles(List<FileInfo> dirInfoList);
}
