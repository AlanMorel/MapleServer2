using System.Diagnostics;
using System.Text.RegularExpressions;
using MaplePacketLib2.Tools;
using MapleServer2.Network;

namespace MapleServer2.Tools
{
    public class PacketStructureResolver
    {
        private const int HEADER_LENGTH = 6;

        private string DefaultValue;

        private readonly ushort OpCode;
        private readonly PacketWriter Packet;
        private readonly Dictionary<uint, SockHintInfo> Overrides;

        private static readonly Regex infoRegex = new Regex(@"\[type=(\d+)\]\[offset=(\d+)\]\[hint=(\w+)\]");

        private PacketStructureResolver(ushort opCode)
        {
            DefaultValue = "0";
            OpCode = opCode;
            Packet = PacketWriter.Of(opCode);
            Overrides = new Dictionary<uint, SockHintInfo>();
        }

        // resolve opcode (offset,type,value) (offset2,type2,value2) ...
        // Example: resolve 0015 (8,Decode8,100)
        public static PacketStructureResolver Parse(string input)
        {
            Regex overrideRegex = new Regex(@"\((\d+),(\w+),(-?\w+)(?:,(\w+))?\)");
            string[] args = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            // Parse opCode: 81 0081 0x81 0x0081
            ushort opCode;
            if (args[0].ToLower().StartsWith("0x"))
            {
                opCode = Convert.ToUInt16(args[0], 16);
            }
            else
            {
                if (args[0].Length == 2)
                {
                    opCode = args[0].ToByte();
                }
                else if (args[0].Length == 4)
                {
                    // Reverse bytes
                    byte[] bytes = args[0].ToByteArray();
                    Array.Reverse(bytes);

                    opCode = BitConverter.ToUInt16(bytes);
                }
                else
                {
                    Console.WriteLine("Invalid opcode.");
                    return null;
                }
            }

            PacketStructureResolver resolver = new PacketStructureResolver(opCode);
            for (int i = 1; i < args.Length; i++)
            {
                Match match = overrideRegex.Match(args[i]);
                if (!match.Success)
                {
                    Console.WriteLine($"Invalid override:{args[i]} skipped.");
                }
                else
                {
                    uint offset = uint.Parse(match.Groups[1].Value);
                    SockHint type = match.Groups[2].Value.ToSockHint();
                    string value = match.Groups[3].Value;
                    string name = "Override";
                    if (match.Groups.Count > 4 && !string.IsNullOrEmpty(match.Groups[4].Value))
                    {
                        name = match.Groups[4].Value;
                    }
                    resolver.Overrides.Add(offset, new SockHintInfo(type, value, name));
                }
            }

            return resolver;
        }

        public void SetDefault(string value)
        {
            DefaultValue = value;
        }

        public void Start(Session session)
        {
            session.OnError = AppendAndRetry;

            // Start off the feedback loop
            session.Send(Packet);
        }

        private void AppendAndRetry(object session, string err)
        {
            SockExceptionInfo info = ParseError(err);
            Debug.Assert(OpCode == info.Type, $"Error for unexpected op code:{info.Type:X4}");
            Debug.Assert(Packet.Length + HEADER_LENGTH == info.Offset,
                $"Offset:{info.Offset} does not match Packet length:{Packet.Length + HEADER_LENGTH}");

            if (Overrides.ContainsKey(info.Offset))
            {
                SockHintInfo @override = Overrides[info.Offset];
                Debug.Assert(@override.Hint == info.Hint, $"Override does not match expected hint:{info.Hint}");
                @override.Update(Packet);
                Console.WriteLine(info.Hint.GetScript($"{@override.Name}+{info.Offset}"));
            }
            else
            {
                new SockHintInfo(info.Hint, DefaultValue).Update(Packet);
                Console.WriteLine(info.Hint.GetScript($"Unknown+{info.Offset}"));
            }

            //Console.WriteLine($"Updated with hint:{info.Hint}, offset:{info.Offset}");
            (session as Session)?.Send(Packet);
        }

        private static SockExceptionInfo ParseError(string error)
        {
            Match match = infoRegex.Match(error);
            if (match.Groups.Count != 4)
            {
                throw new ArgumentException($"Failed to parse error: {error}");
            }

            SockExceptionInfo info;
            info.Type = ushort.Parse(match.Groups[1].Value);
            info.Offset = uint.Parse(match.Groups[2].Value);
            info.Hint = match.Groups[3].Value.ToSockHint();

            return info;
        }

        private struct SockExceptionInfo
        {
            public ushort Type;
            public uint Offset;
            public SockHint Hint;
        }
    }
}
