using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace MaplePacketLib2.Tools;

// From https://github.com/kOchirasu/Maple2/blob/master/Maple2.Tools/Extensions/PacketExtensions.cs thanks kOchirasu
public static class PacketExtensions
{
    // Allows writing packet generically from class
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void WriteClass<T>(this PacketWriter pWriter, T type) where T : IPacketSerializable
    {
        type.WriteTo(pWriter);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ReadClass<T>(this PacketReader packet) where T : IPacketDeserializable
    {
        T type = (T) FormatterServices.GetSafeUninitializedObject(typeof(T));
        type.ReadFrom(packet);
        return type;
    }
}
