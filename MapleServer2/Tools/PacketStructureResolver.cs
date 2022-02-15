﻿using System.Text.RegularExpressions;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Network;
using NLog;

namespace MapleServer2.Tools;

/*
* This class is a way to resolve packets sent by the server.
* It will try to find the packet structure using the client error logging system.
* It will save the packet structure in the packet structure file located in the MapleServer2\PacketStructure folder.
* You can change each packet value in the file and it'll try to continue resolving from the last value.
* If you want to start from the beginning, you can delete the file.
* The resolver will ignore the lines starting with # and 'PacketWriter'.
* More info in the mapleme.me/docs/tutorials/packet-resolver
*/
public class PacketStructureResolver
{
    private const int HeaderLength = 6;

    private readonly string DefaultValue;
    private readonly ushort OpCode;
    private readonly string PacketName;
    private readonly PacketWriter Packet;
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    private PacketStructureResolver(ushort opCode)
    {
        DefaultValue = "0";
        OpCode = opCode;
        Packet = PacketWriter.Of(opCode);
        PacketName = Enum.GetName(typeof(SendOp), opCode);
    }

    // resolve opcode
    // Example: resolve 81
    public static PacketStructureResolver Parse(string input)
    {
        string[] args = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);

        // Parse opCode: 81 0081 0x81 0x0081
        ushort opCode;
        string firstArg = args[0];
        if (firstArg.ToLower().StartsWith("0x"))
        {
            opCode = Convert.ToUInt16(firstArg, 16);
        }
        else
        {
            switch (firstArg.Length)
            {
                case 2:
                    opCode = firstArg.ToByte();
                    break;
                case 4:
                    // Reverse bytes
                    byte[] bytes = firstArg.ToByteArray();
                    Array.Reverse(bytes);

                    opCode = BitConverter.ToUInt16(bytes);
                    break;
                default:
                    Logger.Info("Invalid opcode.");
                    return null;
            }
        }

        PacketStructureResolver resolver = new(opCode);
        DirectoryInfo dir = Directory.CreateDirectory($"{Paths.SOLUTION_DIR}/MapleServer2/PacketStructures");

        string filePath = $"{dir.FullName}/{resolver.OpCode:X4} - {resolver.PacketName}.txt";
        if (!File.Exists(filePath))
        {
            StreamWriter writer = File.CreateText(filePath);
            writer.WriteLine("# Generated by MapleServer2 PacketStructureResolver");
            writer.WriteLine($"PacketWriter pWriter = PacketWriter.Of(SendOp.{resolver.PacketName});");
            writer.Close();
            return resolver;
        }

        string[] fileLines = File.ReadAllLines(filePath);
        foreach (string line in fileLines)
        {
            if (string.IsNullOrEmpty(line) || line.StartsWith("#") || line.StartsWith("PacketWriter"))
            {
                continue;
            }

            string[] packetLine = line.Split("(");
            string type = packetLine[0][13..];
            string valueAsString = packetLine[1].Split(")")[0];
            valueAsString = string.IsNullOrEmpty(valueAsString) ? "0" : valueAsString;
            try
            {
                switch (type)
                {
                    case "Byte":
                        resolver.Packet.WriteByte(byte.Parse(valueAsString));
                        break;
                    case "Short":
                        resolver.Packet.WriteShort(short.Parse(valueAsString));
                        break;
                    case "Int":
                        resolver.Packet.WriteInt(int.Parse(valueAsString));
                        break;
                    case "Long":
                        resolver.Packet.WriteLong(long.Parse(valueAsString));
                        break;
                    case "Float":
                        resolver.Packet.WriteFloat(float.Parse(valueAsString));
                        break;
                    case "UnicodeString":
                        resolver.Packet.WriteUnicodeString(valueAsString.Replace("\"", ""));
                        break;
                    case "String":
                        resolver.Packet.WriteString(valueAsString.Replace("\"", ""));
                        break;
                    default:
                        Logger.Info($"Unknown type: {type}");
                        break;
                }
            }
            catch
            {
                Logger.Info($"Couldn't parse value on function: {line}");
                return null;
            }
        }

        return resolver;
    }

    public void Start(Session session)
    {
        session.OnError = AppendAndRetry;

        // Start off the feedback loop
        session.Send(Packet);
    }

    private void AppendAndRetry(object session, string err)
    {
        SockExceptionInfo info = ErrorParser.Parse(err);
        if (info.SendOp == 0)
        {
            return;
        }

        if (OpCode != (ushort) info.SendOp)
        {
            Logger.Warn($"Error for unexpected op code:{info.SendOp:X4}");
            return;
        }

        if (Packet.Length + HeaderLength != info.Offset)
        {
            Logger.Warn($"Offset:{info.Offset} does not match Packet length:{Packet.Length + HeaderLength}");
            return;
        }

        new SockHintInfo(info.Hint, DefaultValue).Update(Packet);
        string hint = info.Hint.GetCode() + "\r\n";

        DirectoryInfo dir = Directory.CreateDirectory($"{Paths.SOLUTION_DIR}/MapleServer2/PacketStructures");
        StreamWriter file = File.AppendText($"{dir.FullName}/{OpCode:X4} - {PacketName}.txt");
        file.Write(hint);
        file.Close();

        (session as Session)?.Send(Packet);
    }
}
