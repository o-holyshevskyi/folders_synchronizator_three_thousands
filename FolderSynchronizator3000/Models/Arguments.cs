namespace FolderSynchronizator3000.Models;

public record Arguments
{
    public string Source { get; set; }
    public string Replica { get; set; }
    public int Interval { get; set; }
    public string LogPath { get; set; }
}
