namespace MaplePacketLib2.Tools;

public interface IPacketSerializable
{
    public void WriteTo(PacketWriter writer);
}

public interface IPacketDeserializable
{
    public void ReadFrom(PacketReader reader);
}
