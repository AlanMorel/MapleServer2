using System;

namespace MaplePacketLib2.Tools
{
    public interface IByteSerializable
    {
        void WriteTo(IByteWriter writer);
        void ReadFrom(IByteReader reader);
    }
    public interface IByteReader : IDisposable
    {
        int Available { get; }
        unsafe T Read<T>() where T : struct;
        byte ReadByte();
        unsafe long ReadLong();
        unsafe short ReadShort();
        void Skip(int count);
        unsafe int ReadInt();
        string ReadUnicodeString();
        string ReadUnicodeString(int lenght);
        
        string ReadString(int length);
        bool ReadBool();
        unsafe uint ReadUInt();
        string ReadMapleString();
    }
    public interface IByteWriter : IDisposable
    {
        unsafe PacketWriter WriteByte(byte value = 0);
        PacketWriter WriteBool(bool value);
        unsafe PacketWriter WriteInt(int value = 0);
        unsafe PacketWriter WriteLong(long value = 0);
        PacketWriter WriteUnicodeString(string value = "");
        unsafe PacketWriter Write<T>(T value) where T : struct;
        
    }
}
