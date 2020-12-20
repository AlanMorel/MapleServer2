using System;
using System.Net;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using Maple2Storage.Types;
using MapleServer2.Extensions;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Login {
    public class CharacterManagementHandler : LoginPacketHandler {
        public override RecvOp OpCode => RecvOp.CHARACTER_MANAGEMENT;

        public CharacterManagementHandler(ILogger<CharacterManagementHandler> logger) : base(logger) {}

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
            // var jobCode = (Job)packet.ReadShort();
            int jobCode = packet.ReadShort();
            string name = packet.ReadUnicodeString();
            var skinColor = packet.Read<SkinColor>();
            //packet.ReadShort(); // const?
            packet.Skip(2);
            var Equips = new Dictionary<ItemSlot, Item>();

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
                        // Hair Length/Position
                        float backLength = BitConverter.ToSingle(packet.Read(4), 0);
                        byte[] backPositionArray = packet.Read(24);
                        float frontLength = BitConverter.ToSingle(packet.Read(4), 0);
                        byte[] frontPositionArray = packet.Read(24);

                        Equips.Add(ItemSlot.HR, new Item(Convert.ToInt32(id)) {
                            CreationTime = 1565575851,
                            Color = equipColor,
                            HairD = HairData.hairData(backLength, frontLength, backPositionArray, frontPositionArray),
                            Stats = new ItemStats(),
                            IsTemplate=false,
                        });
                        break;
                    case ItemSlot.FA: // Face
                        Equips.Add(ItemSlot.FA, new Item(Convert.ToInt32(id)) {
                            CreationTime = 1565575851,
                            Color = equipColor,
                            Stats = new ItemStats(),
                            IsTemplate = false,
                        });
                        break;
                    case ItemSlot.FD: // Face Decoration
                        byte[] faceDecoration = packet.Read(16); // Face decoration position
                        Equips.Add(ItemSlot.FD, new Item(Convert.ToInt32(id)) {
                            CreationTime = 1565575851,
                            Color = equipColor,
                            FaceDecorationD = faceDecoration,
                            Stats = new ItemStats(),
                            IsTemplate = false,
                        });
                        break;
                    case ItemSlot.CL: // Clothes
                        Equips.Add(ItemSlot.CL, new Item(Convert.ToInt32(id)) {
                            CreationTime = 1565575851,
                            Color = equipColor,
                            Stats = new ItemStats(),
                            IsTemplate = false,
                        });
                        break;
                    case ItemSlot.PA: // Pants
                        Equips.Add(ItemSlot.PA, new Item(Convert.ToInt32(id)) {
                            CreationTime = 1565575851,
                            Color = equipColor,
                            Stats = new ItemStats(),
                            IsTemplate = false,
                        });
                        break;
                    case ItemSlot.SH: // Shoes
                        Equips.Add(ItemSlot.SH, new Item(Convert.ToInt32(id)) {
                            CreationTime = 1565575851,
                            Color = equipColor,
                            Stats = new ItemStats(),
                            IsTemplate = false,
                        });
                        break;
                    case ItemSlot.ER: // Ear
                        // Assign ER
                        Equips.Add(ItemSlot.ER, new Item(Convert.ToInt32(id)) {
                            CreationTime = 1565575851,
                            Color = equipColor,
                            Stats = new ItemStats(),
                            IsTemplate = false,
                        });
                        break;
                }
                logger.Info($" > {type} - id: {id}, color: {equipColor}, colorIndex: {colorIndex}");
            }
            packet.ReadInt(); // const? (4)

            // Check if name is in use (currently just on local account)
            bool taken = false;
            
            foreach (var character in AccountStorage.characters.Values) {
                if (character.Name.ToLower().Equals(name.ToLower())) {
                    taken = true;
                }
            }

            if (taken) {
                session.Send(ResponseCharCreatePacket.NameTaken());
                return;
            }

            // Create new player object
            Player newCharacter = Player.NewCharacter(gender, jobCode, name, skinColor, Equips);

            // Add player object to account storage
            AccountStorage.AddCharacter(newCharacter);

            // Send updated CHAR_MAX_COUNT
            session.Send(CharacterListPacket.SetMax(4, 6));

            // Send CHARACTER_LIST for new character only (append)
            session.Send(CharacterListPacket.AppendEntry(newCharacter));
        }
    }
}