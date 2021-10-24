using System.Net;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Login;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Login
{
    public class CharacterManagementHandler : LoginPacketHandler
    {
        public override RecvOp OpCode => RecvOp.CHARACTER_MANAGEMENT;

        public CharacterManagementHandler() : base() { }

        private enum CharacterManagementMode : byte
        {
            Login = 0x0,
            Create = 0x1,
            Delete = 0x2
        }

        public override void Handle(LoginSession session, PacketReader packet)
        {
            CharacterManagementMode mode = (CharacterManagementMode) packet.ReadByte();
            switch (mode)
            {
                case CharacterManagementMode.Login:
                    HandleSelect(session, packet);
                    break;
                case CharacterManagementMode.Create:
                    HandleCreate(session, packet);
                    break;
                case CharacterManagementMode.Delete:
                    HandleDelete(session, packet);
                    break;
                default:
                    IPacketHandler<LoginSession>.LogUnknownMode(mode);
                    break;
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
            Logger.Info("Character id {characterId} deleted!", characterId);
        }

        public void HandleSelect(LoginSession session, PacketReader packet)
        {
            long charId = packet.ReadLong();
            packet.ReadShort(); // 01 00
            Logger.Info("Logging in to game with char id: {charId}", charId);

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
            session.CharacterId = newCharacter.CharacterId;

            byte equipCount = packet.ReadByte();
            for (int i = 0; i < equipCount; i++)
            {
                int id = packet.ReadInt();
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
                        float backLength = packet.ReadFloat();
                        CoordF backPositionCoord = packet.Read<CoordF>();
                        CoordF backPositionRotation = packet.Read<CoordF>();
                        float frontLength = packet.ReadFloat();
                        CoordF frontPositionCoord = packet.Read<CoordF>();
                        CoordF frontPositionRotation = packet.Read<CoordF>();
                        if (!DefaultItemsMetadataStorage.IsValid((int) job, id))
                        {
                            continue;
                        }

                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.HR, new Item(id)
                        {
                            Color = equipColor,
                            HairData = new HairData(backLength, frontLength, backPositionCoord, backPositionRotation, frontPositionCoord, frontPositionRotation),
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.FA: // Face
                        if (!DefaultItemsMetadataStorage.IsValid((int) job, id))
                        {
                            continue;
                        }

                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.FA, new Item(id)
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.FD: // Face Decoration
                        byte[] faceDecoration = packet.Read(16); // Face decoration position

                        if (!DefaultItemsMetadataStorage.IsValid((int) job, id))
                        {
                            continue;
                        }

                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.FD, new Item(id)
                        {
                            Color = equipColor,
                            FaceDecorationData = faceDecoration,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.CL: // Clothes
                        if (!DefaultItemsMetadataStorage.IsValid((int) job, id))
                        {
                            continue;
                        }

                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.CL, new Item(id)
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.PA: // Pants
                        if (!DefaultItemsMetadataStorage.IsValid((int) job, id))
                        {
                            continue;
                        }

                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.PA, new Item(id)
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.SH: // Shoes
                        if (!DefaultItemsMetadataStorage.IsValid((int) job, id))
                        {
                            continue;
                        }

                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.SH, new Item(id)
                        {
                            Color = equipColor,
                            IsTemplate = false,
                            IsEquipped = true
                        });
                        break;
                    case ItemSlot.ER: // Ear
                        if (!DefaultItemsMetadataStorage.IsValid((int) job, id))
                        {
                            continue;
                        }

                        newCharacter.Inventory.Cosmetics.Add(ItemSlot.ER, new Item(id)
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
