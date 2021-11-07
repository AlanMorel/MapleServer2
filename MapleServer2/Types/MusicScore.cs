namespace MapleServer2.Types;

public class MusicScore
{
    public int Length { get; set; }
    public string Title { get; set; }
    public string Composer { get; set; }
    public long ComposerCharacterId { get; set; }
    public int Type { get; set; }
    public bool Locked { get; set; } // 01 = if player != composer
    public string Notes { get; set; }

    public MusicScore()
    {
        Composer = "";
        Title = "";
        Notes = "";
    }
}
