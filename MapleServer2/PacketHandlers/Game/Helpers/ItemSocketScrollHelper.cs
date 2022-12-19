using MapleServer2.Tools;
using MoonSharp.Interpreter;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class ItemSocketScrollHelper
{
    public static float GetSuccessRate(int scrollId)
    {
        Script? script = ScriptLoader.GetScript("Functions/ItemSocketScroll/getSuccessRate");
        return (float) (script?.RunFunction("getSuccessRate", scrollId)?.Number ?? 0);
    }

    public static byte GetSocketCount(int scrollId)
    {
        Script? script = ScriptLoader.GetScript("Functions/ItemSocketScroll/getSocketCount");
        return (byte) (script?.RunFunction("getSocketCount", scrollId)?.Number ?? 0);
    }
}
