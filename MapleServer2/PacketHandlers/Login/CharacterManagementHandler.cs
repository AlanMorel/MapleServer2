using System;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using Maple2Storage.Types;
using MapleServer2.Extensions;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Enums;
using Microsoft.Extensions.Logging;
using MapleServer2.Types;
using System.Collections.Generic;

namespace MapleServer2.PacketHandlers.Login {
    public class CharacterManagementHandler : LoginPacketHandler {
        public override ushort OpCode => RecvOp.CHARACTER_MANAGEMENT;


        public CharacterManagementHandler(ILogger<CharacterManagementHandler> logger) : base(logger) {
        }

        public override void Handle(LoginSession session, PacketReader packet) {
            byte mode = packet.ReadByte();
            switch (mode) {
                case 0: // Login
                    HandleSelect(session, packet);
                    break;
                case 1: // Create
                    HandleCreate(session, packet);
                    break;
                case 2: // Delete
                    long deleteCharId = packet.ReadLong();
                    logger.Info($"Deleting {deleteCharId}");
                    break;
                default:
                    throw new ArgumentException($"Invalid Char select mode {mode}");
            }
        }

        public void HandleSelect(LoginSession session, PacketReader packet)
        {
            long charId = packet.ReadLong();
            packet.ReadShort(); // 01 00
            logger.Info($"Logging in to game with charId:{charId}");

            var endpoint = new IPEndPoint(IPAddress.Loopback, GameServer.PORT);
            var authData = new AuthData
            {
                TokenA = session.GetToken(),
                TokenB = session.GetToken(),
                CharacterId = charId,
            };
            // Write AuthData to storage shared with GameServer
            AuthStorage.SetData(session.AccountId, authData);

            session.Send(MigrationPacket.LoginToGame(endpoint, authData));
            //LoginPacket.LoginError("message?");
        }

        public void HandleCreate(LoginSession session, PacketReader packet)
        {
            byte gender = packet.ReadByte();
            //packet.ReadShort(); // const?
            var jobCode = (Job)packet.ReadShort();
            string name = packet.ReadUnicodeString();
            var skinColor = packet.Read<SkinColor>();
            //packet.ReadShort(); // const?
            packet.Skip(2);
            var equipSlots = new Dictionary<ItemSlot, Item>();

            logger.Info($"Creating character: {name}, gender: {gender}, skinColor: {skinColor}, job: {jobCode}");
            int equipCount = packet.ReadByte();
            for (int i = 0; i < equipCount; i++)
            {
                uint id = packet.ReadUInt();
                string typeStr = packet.ReadUnicodeString();
                if (!Enum.TryParse(typeStr, out ItemSlot type))
                {
                    throw new ArgumentException($"Unknown equip type: {typeStr}");
                }
                var equipColor = packet.Read<EquipColor>();
                int colorIndex = packet.ReadInt();

                switch (type)
                {
                    case ItemSlot.HR: // Hair
                        packet.Skip(56); // Hair Position
                        equipSlots.Add(ItemSlot.HR, new Item(Convert.ToInt32(id)));
                        break;
                    case ItemSlot.FA: // Face
                        equipSlots.Add(ItemSlot.FA, new Item(Convert.ToInt32(id)));
                        break;
                    case ItemSlot.FD: // Face Decoration
                        packet.Skip(16);
                        equipSlots.Add(ItemSlot.FD, new Item(Convert.ToInt32(id)));
                        break;
                    case ItemSlot.CL: // Clothes
                        // Assign CL
                        equipSlots.Add(ItemSlot.CL, new Item(Convert.ToInt32(id)));
                        break;
                    case ItemSlot.PA: // Pants
                        // Assign PA
                        equipSlots.Add(ItemSlot.PA, new Item(Convert.ToInt32(id)));
                        break;
                    case ItemSlot.SH: //Shoes
                        // Assign SH
                        equipSlots.Add(ItemSlot.SH, new Item(Convert.ToInt32(id)));
                        break;
                    case ItemSlot.ER: // Ear
                        // Assign ER
                        equipSlots.Add(ItemSlot.ER, new Item(Convert.ToInt32(id)));
                        break;
                }
                logger.Info($" > {type} - id: {id}, color: {equipColor}, colorIndex: {colorIndex}");
            }
            packet.ReadInt(); // const? (4)
            var player = Player.NewCharacter(gender, jobCode, name, skinColor, equipSlots);
            // OnSuccess
            //SendOp.CHAR_MAX_COUNT;
            session.Send(CharacterListPacket.SetMax(2, 4));
            //SendOp.CHARACTER_LIST //(New char only. This will append)
            session.Send(CharacterListPacket.AppendEntry(player));
            // OnFailure, forcing failure here while debugging
            //session.Send(ResponseCharCreatePacket.NameTaken());


        }
    }
}