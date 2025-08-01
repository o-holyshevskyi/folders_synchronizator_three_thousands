using System.Security.Cryptography;

namespace FolderSynchronizator3000.Libs.Sync;

internal class Comparer
{
    public static bool CompareFiles(string sourcePath, string replicaPath)
    {
        var sFiles = Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories);
        var rFiles = Directory.GetFiles(replicaPath, "*", SearchOption.AllDirectories);

        if (sFiles.Length != rFiles.Length)
            return false;

        for (int i = 0; i < sFiles.Length; i++)
        {
            if (!FilesAreEqual(sFiles[i], rFiles[i]))
                return false;
        }

        return true;
    }

    public static bool CompareDirectories(string sourcePath, string replicaPath)
    {
        var sDirs = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
        var rDirs = Directory.GetDirectories(replicaPath, "*", SearchOption.AllDirectories);

        if (sDirs.Length != rDirs.Length)
            return false;

        for (int i = 0; i < sDirs.Length; i++)
        {
            if (!DirsAreEqual(sDirs[i], rDirs[i]))
                return false;
        }

        return true;
    }

    public static bool FilesAreEqual(string source1, string source2)
    {
        var sFileInfo = new FileInfo(source1);
        var rFileInfo = new FileInfo(source2);

        if (sFileInfo.Length != rFileInfo.Length)
            return false;

        using var hashAlgo = SHA256.Create();
        using var streamS = File.OpenRead(source1);
        using var streamR = File.OpenRead(source2);

        var hashS = hashAlgo.ComputeHash(streamS);
        var hashR = hashAlgo.ComputeHash(streamR);

        return hashS.SequenceEqual(hashR);
    }

    public static bool DirsAreEqual(string source1, string source2)
    {
        var dir1 = new DirectoryInfo(source1);
        var dir2 = new DirectoryInfo(source2);

        if (!dir1.Exists || !dir2.Exists)
            return false;

        var files1 = dir1.GetFiles().OrderBy(f => f.Name).ToArray();
        var files2 = dir2.GetFiles().OrderBy(f => f.Name).ToArray();

        if (files1.Length != files2.Length)
            return false;

        for (int i = 0; i < files1.Length; i++)
        {
            if (files1[i].Name != files2[i].Name || files1[i].Length != files2[i].Length)
                return false;
        }

        var dirs1 = dir1.GetDirectories().OrderBy(d => d.Name).ToArray();
        var dirs2 = dir2.GetDirectories().OrderBy(d => d.Name).ToArray();

        if (dirs1.Length != dirs2.Length)
            return false;

        for (int i = 0; i < dirs1.Length; i++)
        {
            if (dirs1[i].Name != dirs2[i].Name)
                return false;

            // Recursively compare subdirectories
            if (!DirsAreEqual(dirs1[i].FullName, dirs2[i].FullName))
                return false;
        }

        return true;
    }
}
