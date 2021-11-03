using System.Diagnostics;
using MaplePacketLib2.Tools;

namespace MapleServer2.Tools
{
    public enum SockHint
    {
        Decode1,
        Decode2,
        Decode4,
        Decodef,
        Decode8,
        DecodeStr,
        DecodeStrA
    }

    public struct SockHintInfo
    {
        public SockHint Hint;
        // Values
        public byte ByteValue;
        public short ShortValue;
        public int IntValue;
        public long LongValue;
        public string StringValue;

        public string Name; // Name of node

        public SockHintInfo(SockHint hint, string value, string name = "Unknown")
        {
            Hint = hint;
            StringValue = value == "0" ? "" : value;
            Name = name;
            ByteValue = 0;
            ShortValue = 0;
            IntValue = 0;
            LongValue = 0;

            if (byte.TryParse(value, out byte byteValue))
            {
                ByteValue = byteValue;
            }
            if (short.TryParse(value, out short shortValue))
            {
                ShortValue = shortValue;
            }
            if (int.TryParse(value, out int intValue))
            {
                IntValue = intValue;
            }
            if (long.TryParse(value, out long longValue))
            {
                LongValue = longValue;
            }
        }

        public void Update(PacketWriter packet)
        {
            switch (Hint)
            {
                case SockHint.Decode1:
                    packet.WriteByte(ByteValue);
                    break;
                case SockHint.Decode2:
                    packet.WriteShort(ShortValue);
                    break;
                case SockHint.Decode4:
                case SockHint.Decodef:
                    packet.WriteInt(IntValue);
                    break;
                case SockHint.Decode8:
                    packet.WriteLong(LongValue);
                    break;
                case SockHint.DecodeStr:
                    packet.WriteUnicodeString(StringValue);
                    break;
                case SockHint.DecodeStrA:
                    packet.WriteString(StringValue);
                    break;
                default:
                    throw new ArgumentException($"Unexpected hint: {Hint}");
            }
        }
    }

    public static class SockHintExtensions
    {
        // MapleShark Script Code
        public static string GetScript(this SockHint hint, string name = "Unknown") =>
            hint switch
            {
                SockHint.Decode1 => $"AddByte(\"{name}\");",
                SockHint.Decode2 => $"AddShort(\"{name}\");",
                SockHint.Decode4 => $"AddInt(\"{name}\");",
                SockHint.Decodef => $"AddFloat(\"{name}\");",
                SockHint.Decode8 => $"AddLong(\"{name}\");",
                SockHint.DecodeStr => $"AddUnicodeString(\"{name.Replace("Unknown", "UnknownStr")}\");",
                SockHint.DecodeStrA => $"AddString(\"{name.Replace("Unknown", "UnknownStr")}\");",
                _ => throw new ArgumentException($"Unexpected hint: {hint}")
            };

        // PacketWriter Code
        public static string GetCode(this SockHint hint) =>
            hint switch
            {
                SockHint.Decode1 => $"pWriter.WriteByte();",
                SockHint.Decode2 => $"pWriter.WriteShort();",
                SockHint.Decode4 => $"pWriter.WriteInt();",
                SockHint.Decodef => $"pWriter.WriteInt(-1);",
                SockHint.Decode8 => $"pWriter.WriteLong();",
                SockHint.DecodeStr => $"pWriter.WriteUnicodeString(\"\");",
                SockHint.DecodeStrA => $"pWriter.WriteString(\"\");",
                _ => throw new ArgumentException($"Unexpected hint: {hint}")
            };

        public static SockHint ToSockHint(this string sockHint)
        {
            sockHint = sockHint switch
            {
                "1" => SockHint.Decode1.ToString(),
                "2" => SockHint.Decode2.ToString(),
                "4" => SockHint.Decode4.ToString(),
                "f" => SockHint.Decodef.ToString(),
                "8" => SockHint.Decode8.ToString(),
                "s" => SockHint.DecodeStr.ToString(),
                "sa" => SockHint.DecodeStrA.ToString(),
                _ => sockHint
            };
            bool result = Enum.TryParse(sockHint, out SockHint hint);
            Debug.Assert(result, $"Failed to parse SockHint:{sockHint}");

            return hint;
        }
    }
}
