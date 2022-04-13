using MapleServer2.Tools;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class ItemSocketScrollHelper
{
    public static float GetSuccessRate(int scrollId)
    {
        ScriptLoader scriptLoader = new("Functions/ItemSocketScroll/getSuccessRate");
        return (float) scriptLoader.Call("getSuccessRate", scrollId).Number;
    }

    public static byte GetSocketCount(int scrollId)
    {
        ScriptLoader scriptLoader = new("Functions/ItemSocketScroll/getSocketCount");
        return (byte) scriptLoader.Call("getSocketCount", scrollId).Number;
    }
}
