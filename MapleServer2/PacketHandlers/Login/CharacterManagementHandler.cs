using System;
using System.Net;
using System.Threading;
using Maple2.Data.Storage;
using Maple2.Data.Types;
using Maple2.Data.Types.Items;
using Maple2.Sql.Context;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Extensions;
using MapleServer2.GameData;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Tools;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Login {
    public class CharacterManagementHandler : LoginPacketHandler {
        public override ushort OpCode => RecvOp.CHARACTER_MANAGEMENT;

        

        public CharacterManagementHandler(ILogger<CharacterManagementHandler> logger)
                : base(logger) {
           
        }

        public override void Handle(LoginSession session, PacketReader packet) {
            byte mode = packet.ReadByte();
            switch (mode) {
                case 0:
                    HandleSelect(session, packet);
                    break;
                case 1:
                    //HandleCreate(session, packet);
                    break;
                case 2:
                    //HandleDelete(session, packet);
                    break;
                case 3:
                    HandleCancelDelete(session, packet);
                    break;
                default:
                    throw new ArgumentException($"Invalid Char select mode {mode}");
            }
        }

        private void HandleSelect(LoginSession session, PacketReader packet) {
            long characterId = packet.ReadLong();
            packet.ReadShort(); // 01 00
            logger.Info($"Logging in to game with charId:{characterId}");

            var endpoint = new IPEndPoint(IPAddress.Loopback, GameServer.PORT);
            var authData = new AuthData {
                TokenA = session.GetToken(),
                TokenB = session.GetToken(),
                CharacterId = characterId,
            };

            // Write AuthData to storage shared with GameServer
            AuthStorage.SetData(session.AccountId, authData);
            session.Send(MigrationPacket.LoginToGame(endpoint, authData));

            //This is for closing Socket
            session.Disconnect();

            //LoginPacket.LoginError("message?");
        }
        /*
        private void HandleCreate(LoginSession session, PacketReader packet) {
            byte gender = packet.ReadByte();
            var jobCode = (JobCode) packet.ReadShort();
            var jobType = (JobType) ((int) jobCode * 10);
            string name = packet.ReadUnicodeString();
            long accountId = session.AccountId;
            // Temporary fake validation for testing
            if (name.Length < 4) {
                session.Send(ResponseCharCreatePacket.NameTaken());
                return;
            }

            var skinColor = packet.Read<SkinColor>();
            packet.Skip(2); // Unknown
            
            logger.Info($"Creating character:{name}, gender:{gender}, skinColor:{skinColor}");
            var newCharacter = Character.NewCharacter(gender, jobType, name);

            using UserStorage.Request request = userStorage.Context();
            newCharacter.Id = request.CreateCharacter(newCharacter);
            newCharacter.SkinColor = skinColor;

            int equipCount = packet.ReadByte();
            for (int i = 0; i < equipCount; i++) {
                int id = packet.ReadInt();
                string typeStr = packet.ReadUnicodeString();
                if (!Enum.TryParse(typeStr, out EquipSlot slot)) {
                    throw new ArgumentException($"Unknown equip type: {typeStr}");
                }

                Item equip = ItemFactory.Create(id);
                equip.Slot = (short) slot;
                //equip.Appearance.ReadFrom(packet);
                //InventoryType type = equip.InventoryType == InventoryType.Outfit
                //    ? InventoryType.OutfitEquip
                //    : InventoryType.GearEquip;
                //long ownerId = InventoryState.GetOwnerId(newCharacter.Id, type);
                //long itemId = request.CreateItem(ownerId, equip);
                //bool result = player.GearEquip.TryPutSlot(equip, (short) slot);
                //logger.Info($" > {slot} - id:{id}, color:{equip.Appearance.Color} | {itemId}");
                
                logger.Info($">{slot} - id:{id}, color:not define");
            }

            packet.Skip(4); // Unknown

            session.Send(CharacterListPacket.SetMax(4, 6));

            Character player = request.GetCharacter(newCharacter.Id);
            //session.Send(CharacterListPacket.AppendEntry(player));
            // There is a bug in the client at causes a lag spike here CxxException [STATUS_ACCESS_VIOLATION] 6F465F6C
        }

        private void HandleDelete(LoginSession session, PacketReader packet) {
            long characterId = packet.ReadLong();

            using (UserStorage.Request request = userStorage.Context()) {
                Character character = request.GetCharacter(characterId);
                if (character.Level < 20) {
                    request.DeleteCharacter(characterId);
                    request.Commit();

                    session.Send(CharacterListPacket.DeleteEntry(characterId));
                } else {
                    session.Send(CharacterListPacket.DeletePending(characterId,
                        DateTimeOffset.UtcNow.AddDays(1).ToUnixTimeSeconds()));
                }
            }
        }
        */
        private void HandleCancelDelete(LoginSession session, PacketReader packet) {
            long characterId = packet.ReadLong();

            session.Send(CharacterListPacket.CancelDelete(characterId));
        }
    }
}