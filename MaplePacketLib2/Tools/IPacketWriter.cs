namespace MaplePacketLib2.Tools;

public interface IPacketWriter : IDisposable
{
    public void Write<T>(in T value) where T : struct;
    public void WriteBytes(byte[] value);
    public void WriteBytes(byte[] value, int offset, int length);
    public void WriteBool(bool value);
    public void WriteByte(byte value = 0);
    public void WriteShort(short value = 0);
    public void WriteInt(int value = 0);
    public void WriteFloat(float value = 0f);
    public void WriteLong(long value = 0);
    public void WriteString(string value = "");
    public void WriteUnicodeString(string value = "");
}
