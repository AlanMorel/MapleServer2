using System;
using System.Collections.Generic;
using System.Net;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Extensions;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Login
{
    public class CharacterManagementHandler : LoginPacketHandler
    {
        public override RecvOp OpCode => RecvOp.CHARACTER_MANAGEMENT;

        public CharacterManagementHandler(ILogger<CharacterManagementHandler> logger) : base(logger) { }

        public override void Handle(LoginSession session, PacketReader packet)
        {
            byte mode = packet.ReadByte();
            switch (mode)
            {
                case 0: // Login
                    HandleSelect(session, packet);
                    break;
                case 1: // Create
                    HandleCreate(session, packet);
                    break;
                case 2: // Delete
                    HandleDelete(session, packet);
                    break;
                default:
                    throw new ArgumentException($"Invalid Char select mode {mode}");
            }
        }

        private void HandleDelete(LoginSession session, PacketReader packet)
        {
            long deleteCharId = packet.ReadLong();
            if (!DatabaseManager.DeleteCharacter(DatabaseManager.GetCharacter(deleteCharId)))
            {
                throw new ArgumentException("Could not delete character");
            }
            session.Send(CharacterListPacket.DeleteCharacter(deleteCharId));
            Logger.Info($"Deleting {deleteCharId}");
        }

        public void HandleSelect(LoginSession session, PacketReader packet)
        {
            long charId = packet.ReadLong();
            packet.ReadShort(); // 01 00
            Logger.Info($"Logging in to game with char id: {charId}");

            IPEndPoint endpoint = new IPEndPoint(IPAddress.Loopback, GameServer.PORT);
            AuthData authData = new AuthData
            {
                TokenA = session.GetToken(),
                TokenB = session.GetToken(),
                CharacterId = charId,
            };
            // Write AuthData to storage shared with GameServer
            AuthStorage.SetData(session.AccountId, authData);

            session.SendFinal(MigrationPacket.LoginToGame(endpoint, authData));

            // LoginPacket.LoginError("message?");
        }

        public void HandleCreate(LoginSession session, PacketReader packet)
        {
            byte gender = packet.ReadByte();
            Job job = (Job) packet.ReadShort();
            string name = packet.ReadUnicodeString();
            SkinColor skinColor = packet.Read<SkinColor>();
            packet.Skip(2);
            Dictionary<ItemSlot, Item> cosmetics = new Dictionary<ItemSlot, Item>();

            Logger.Info($"Creating character: {name}, gender: {gender}, skinColor: {skinColor}, job: {job}");

            int equipCount = packet.ReadByte();
            for (int i = 0; i < equipCount; i++)
            {
                uint id = packet.ReadUInt();
                string typeStr = packet.ReadUnicodeString();
                if (!Enum.TryParse(typeStr, out ItemSlot type))
                {
                    throw new ArgumentException($"Unknown equip type: {typeStr}");
                }
                EquipColor equipColor = packet.Read<EquipColor>();

                switch (type)
                {
                    case ItemSlot.HR: // Hair
                        // Hair Length/Position
                        float backLength = BitConverter.ToSingle(packet.Read(4), 0);
                        CoordF backPositionCoord = packet.Read<CoordF>();
                        CoordF backPositionRotation = packet.Read<CoordF>();
                        float frontLength = BitConverter.ToSingle(packet.Read(4), 0);
                        CoordF frontPositionCoord = packet.Read<CoordF>();
                        CoordF frontPositionRotation = packet.Read<CoordF>();

                        cosmetics.Add(ItemSlot.HR, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            HairData = new HairData(backLength, frontLength, backPositionCoord, backPositionRotation, frontPositionCoord, frontPositionRotation),
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.FA: // Face
                        cosmetics.Add(ItemSlot.FA, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.FD: // Face Decoration
                        byte[] faceDecoration = packet.Read(16); // Face decoration position
                        cosmetics.Add(ItemSlot.FD, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            FaceDecorationData = faceDecoration,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.CL: // Clothes
                        cosmetics.Add(ItemSlot.CL, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.PA: // Pants
                        cosmetics.Add(ItemSlot.PA, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.SH: // Shoes
                        cosmetics.Add(ItemSlot.SH, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.ER: // Ear
                        // Assign ER
                        cosmetics.Add(ItemSlot.ER, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                }
                Logger.Info($" > {type} - id: {id}, color: {equipColor}");
            }
            packet.ReadInt(); // const? (4)

            if (DatabaseManager.NameExists(name))
            {
                session.Send(ResponseCharCreatePacket.NameTaken());
                return;
            }

            Player newCharacter = new Player(session.AccountId, name, gender, job, skinColor);
            foreach (Item item in cosmetics.Values)
            {
                item.Owner = newCharacter;
            }
            newCharacter.Inventory.Cosmetics = cosmetics;
            DatabaseManager.UpdateCharacter(newCharacter);

            // Send updated CHAR_MAX_COUNT
            session.Send(CharacterListPacket.SetMax(4, 6));

            // Send CHARACTER_LIST for new character only (append)
            session.Send(CharacterListPacket.AppendEntry(newCharacter));
        }
    }
}
