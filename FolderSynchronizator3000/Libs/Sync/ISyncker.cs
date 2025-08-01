namespace FolderSynchronizator3000.Libs.Sync;

public interface ISyncker
{
    public void Sync(string sourcePath, string replicaPath);
}
