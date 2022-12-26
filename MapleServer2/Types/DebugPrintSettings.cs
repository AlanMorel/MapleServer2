namespace MapleServer2.Types;

public class DebugPrintSettings
{
    public int TargetsToPrint = 0;
    public bool PrintOwnEffects = false;
    public bool PrintCastedEffects = false;
    public bool PrintEffectsFromOthers = false;
    public bool PrintEffectEvents = false;
    public bool IncludeEffectTickEvent = false;
    public List<int> EffectWatchList = new();
    public List<int> EffectIgnoreList = new();
}
