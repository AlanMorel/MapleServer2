using System.Net;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Login;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Login;

public class CharacterManagementHandler : LoginPacketHandler<CharacterManagementHandler>
{
    public override RecvOp OpCode => RecvOp.CharManagement;

    private enum Mode : byte
    {
        Login = 0x0,
        Create = 0x1,
        Delete = 0x2
    }

    public override void Handle(LoginSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();
        switch (mode)
        {
            case Mode.Login:
                HandleSelect(session, packet);
                break;
            case Mode.Create:
                HandleCreate(session, packet);
                break;
            case Mode.Delete:
                HandleDelete(session, packet);
                break;
            default:
                LogUnknownMode(mode);
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
        Logger.Information("Character id {characterId} deleted!", characterId);
    }

    private void HandleSelect(LoginSession session, PacketReader packet)
    {
        long characterId = packet.ReadLong();
        packet.ReadShort(); // 01 00
        Logger.Information("Logging in to game with char id: {characterId}", characterId);

        string ipAddress = session.IsLocalHost() ? "127.0.0.1" : Environment.GetEnvironmentVariable("IP");
        int port = int.Parse(Environment.GetEnvironmentVariable("GAME_PORT"));
        IPEndPoint endpoint = new(IPAddress.Parse(ipAddress), port);

        AuthData authData = DatabaseManager.AuthData.GetByAccountId(session.AccountId);
        Player player = DatabaseManager.Characters.FindPartialPlayerById(characterId);
        if (player is null)
        {
            Logger.Error("Character not found!");
            return;
        }

        authData.OnlineCharacterId = characterId;

        DatabaseManager.AuthData.UpdateOnlineCharacterId(authData);
        DatabaseManager.Characters.UpdateChannelId(characterId, channelId: 1, instanceId: 1, isMigrating: false);

        session.SendFinal(MigrationPacket.LoginToGame(endpoint, player.MapId, authData), logoutNotice: false);
    }

    private static void HandleCreate(LoginSession session, PacketReader packet)
    {
        Gender gender = (Gender) packet.ReadByte();
        Job job = (Job) packet.ReadShort();
        string name = packet.ReadUnicodeString();
        SkinColor skinColor = packet.Read<SkinColor>();
        packet.Skip(2);

        switch (name.Length)
        {
            case <= 1:
                session.Send(ResponseCharCreatePacket.Error(ResponseCharCreatePacket.Mode.NameNeeds2LettersMinimum));
                return;
            case > 13:
                session.Send(ResponseCharCreatePacket.Error(ResponseCharCreatePacket.Mode.MaxCharactersReached));
                return;
        }

        if (DatabaseManager.Characters.NameExists(name))
        {
            session.Send(ResponseCharCreatePacket.Error(ResponseCharCreatePacket.Mode.NameIsTaken));
            return;
        }

        if (CharacterCreateMetadataStorage.JobIsDisabled((int) job))
        {
            session.Send(ResponseCharCreatePacket.Error(ResponseCharCreatePacket.Mode.JobRestriction));
            return;
        }

        Account account = DatabaseManager.Accounts.FindById(session.AccountId);

        List<Item> equips = new();
        byte equipCount = packet.ReadByte();
        for (int i = 0; i < equipCount; i++)
        {
            int id = packet.ReadInt();
            string typeStr = packet.ReadUnicodeString();
            if (!Enum.TryParse(typeStr, out ItemSlot type) || !DefaultItemsMetadataStorage.IsValid((int) job, id))
            {
                session.Send(ResponseCharCreatePacket.Error(ResponseCharCreatePacket.Mode.IncorrectGear));
                return;
            }

            EquipColor equipColor = packet.Read<EquipColor>();
            HairData hair = new();
            byte[] faceDecoration = new byte[16];

            switch (type)
            {
                case ItemSlot.HR:
                    float backLength = packet.ReadFloat();
                    CoordF backPositionCoord = packet.Read<CoordF>();
                    CoordF backPositionRotation = packet.Read<CoordF>();
                    float frontLength = packet.ReadFloat();
                    CoordF frontPositionCoord = packet.Read<CoordF>();
                    CoordF frontPositionRotation = packet.Read<CoordF>();
                    hair = new(backLength, frontLength, backPositionCoord, backPositionRotation, frontPositionCoord, frontPositionRotation);
                    break;
                case ItemSlot.FD:
                    faceDecoration = packet.ReadBytes(16);
                    break;
            }

            equips.Add(new(id)
            {
                Color = equipColor,
                HairData = hair,
                FaceDecorationData = faceDecoration,
                ItemSlot = type
            });
        }

        // create character
        Player newCharacter = new(account, name, gender, job, skinColor);
        session.CharacterId = newCharacter.CharacterId;

        // equip each item
        foreach (Item item in equips)
        {
            item.IsEquipped = true;
            item.BindItem(newCharacter);
            newCharacter.Inventory.Cosmetics.Add(item.ItemSlot, item);
        }

        packet.ReadInt(); // const? (4)

        DatabaseManager.Inventories.Update(newCharacter.Inventory);

        // Send updated CHAR_MAX_COUNT
        session.Send(CharacterListPacket.SetMax(account.CharacterSlots));

        // Send CHARACTER_LIST for new character only (append)
        session.Send(CharacterListPacket.AppendEntry(newCharacter));
    }
}
