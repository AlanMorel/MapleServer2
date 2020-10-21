using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MaplePacketLib2.Tools;
using MapleServer2.Network;

namespace MapleServer2.Tools {
    public class PacketStructureResolver {
        private const int HEADER_LENGTH = 6;

        private string defaultValue;

        private readonly ushort opCode;
        private readonly PacketWriter packet;
        private readonly Dictionary<uint, SockHintInfo> overrides;

        private static readonly Regex infoRegex = new Regex(@"\[type=(\d+)\]\[offset=(\d+)\]\[hint=(\w+)\]");

        private PacketStructureResolver(ushort opCode) {
            this.defaultValue = "0";
            this.opCode = opCode;
            this.packet = PacketWriter.Of(opCode);
            this.overrides = new Dictionary<uint, SockHintInfo>();
        }

        // resolve opcode (offset,type,value) (offset2,type2,value2) ...
        // Example: resolve 1500 (4,Decode8,100)
        public static PacketStructureResolver Parse(string input) {
            var overrideRegex = new Regex(@"\((\d+),(\w+),(-?\w+)(?:,(\w+))?\)");
            string[] args = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            // TODO: fix this opcode parsing (it's backwards for 2 bytes...)
            ushort opCode = args[0].Length == 2 ? args[0].ToByte() : BitConverter.ToUInt16(args[0].ToByteArray());

            var resolver = new PacketStructureResolver(opCode);
            for (int i = 1; i < args.Length; i++) {
                Match match = overrideRegex.Match(args[i]);
                if (!match.Success) {
                    Console.WriteLine($"Invalid override:{args[i]} skipped.");
                } else {
                    uint offset = uint.Parse(match.Groups[1].Value);
                    var type = match.Groups[2].Value.ToSockHint();
                    string value = match.Groups[3].Value;
                    string name = "Override";
                    if (match.Groups.Count > 4 && !string.IsNullOrEmpty(match.Groups[4].Value)) {
                        name = match.Groups[4].Value;
                    }
                    resolver.overrides.Add(offset, new SockHintInfo(type, value, name));
                }
            }

            return resolver;
        }

        public void SetDefault(string value) {
            this.defaultValue = value;
        }

        public void Start(Session session) {
            session.OnError = AppendAndRetry;

            // Start off the feedback loop
            session.Send(packet);
        }

        private void AppendAndRetry(object session, string err) {
            SockExceptionInfo info = ParseError(err);
            Debug.Assert(opCode == info.Type, $"Error for unexpected op code:{info.Type:X4}");
            Debug.Assert(packet.Length + HEADER_LENGTH == info.Offset,
                $"Offset:{info.Offset} does not match packet length:{packet.Length + HEADER_LENGTH}");

            if (overrides.ContainsKey(info.Offset)) {
                SockHintInfo @override = overrides[info.Offset];
                Debug.Assert(@override.Hint == info.Hint, $"Override does not match expected hint:{info.Hint}");
                @override.Update(packet);
                Console.WriteLine(info.Hint.GetScript($"{@override.Name}+{info.Offset}"));
            } else {
                new SockHintInfo(info.Hint, defaultValue).Update(packet);
                Console.WriteLine(info.Hint.GetScript($"Unknown+{info.Offset}"));
            }

            //Console.WriteLine($"Updated with hint:{info.Hint}, offset:{info.Offset}");
            (session as Session)?.Send(packet);
        }

        private static SockExceptionInfo ParseError(string error) {
            Match match = infoRegex.Match(error);
            if (match.Groups.Count != 4) {
                throw new ArgumentException($"Failed to parse error: {error}");
            }

            SockExceptionInfo info;
            info.Type = ushort.Parse(match.Groups[1].Value);
            info.Offset = uint.Parse(match.Groups[2].Value);
            info.Hint = match.Groups[3].Value.ToSockHint();

            return info;
        }

        private struct SockExceptionInfo {
            public ushort Type;
            public uint Offset;
            public SockHint Hint;
        }
    }
}