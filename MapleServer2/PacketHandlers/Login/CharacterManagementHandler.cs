using System.Net;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Extensions;
using MapleServer2.Packets;
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
            long characterId = packet.ReadLong();
            if (!DatabaseManager.Characters.SetCharacterDeleted(characterId))
            {
                Logger.Error("Could not delete character");
                return;
            }
            session.Send(CharacterListPacket.DeleteCharacter(characterId));
            Logger.Info($"Character id {characterId} deleted!");
        }

        public void HandleSelect(LoginSession session, PacketReader packet)
        {
            long charId = packet.ReadLong();
            packet.ReadShort(); // 01 00
            Logger.Info($"Logging in to game with char id: {charId}");

            string ipAddress = Environment.GetEnvironmentVariable("IP");
            int port = int.Parse(Environment.GetEnvironmentVariable("GAME_PORT"));
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
            AuthData authData = new AuthData
            {
                TokenA = LoginSession.GetToken(),
                TokenB = LoginSession.GetToken(),
                CharacterId = charId,
            };
            // Write AuthData to storage shared with GameServer
            AuthStorage.SetData(session.AccountId, authData);

            session.SendFinal(MigrationPacket.LoginToGame(endpoint, authData));

            // LoginPacket.LoginError("message?");
        }

        public static void HandleCreate(LoginSession session, PacketReader packet)
        {
            byte gender = packet.ReadByte();
            Job job = (Job) packet.ReadShort();
            string name = packet.ReadUnicodeString();
            SkinColor skinColor = packet.Read<SkinColor>();
            packet.Skip(2);

            if (DatabaseManager.Characters.NameExists(name))
            {
                session.Send(ResponseCharCreatePacket.NameTaken());
                return;
            }

            Account account = DatabaseManager.Accounts.FindById(session.AccountId);
            Player newCharacter = new Player(account, name, gender, job, skinColor);

            int equipCount = packet.ReadByte();
            for (int i = 0; i < equipCount; i++)
            {
                uint id = packet.ReadUInt();
                int id2 = Convert.ToInt32(id);
                string typeStr = packet.ReadUnicodeString();
                if (!Enum.TryParse(typeStr, out ItemSlot type))
                {
                    throw new ArgumentException($"Unknown equip type: {typeStr}");
                }
                EquipColor equipColor = packet.Read<EquipColor>();
                //if (!DefaultItemsMetadataStorage.IsValid((int) job, id2))
                //{
                //    Console.WriteLine("INVALID ITEM");
                //    continue;
                //}

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

                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.HR, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            HairData = new HairData(backLength, frontLength, backPositionCoord, backPositionRotation, frontPositionCoord, frontPositionRotation),
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.FA: // Face
                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.FA, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.FD: // Face Decoration
                        byte[] faceDecoration = packet.Read(16); // Face decoration position
                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.FD, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            FaceDecorationData = faceDecoration,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.CL: // Clothes
                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.CL, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.PA: // Pants
                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.PA, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.SH: // Shoes
                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.SH, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.ER: // Ear
                        // Assign ER
                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.ER, new Item(Convert.ToInt32(id))
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                }
            }
            packet.ReadInt(); // const? (4)

            DatabaseManager.Inventories.Update(newCharacter.Inventory);

            // Send updated CHAR_MAX_COUNT
            session.Send(CharacterListPacket.SetMax(account.CharacterSlots));

            // Send CHARACTER_LIST for new character only (append)
            session.Send(CharacterListPacket.AppendEntry(newCharacter));
        }
    }
}
