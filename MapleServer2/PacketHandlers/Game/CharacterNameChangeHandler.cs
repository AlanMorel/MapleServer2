using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game
{
    public class CharacterNameChangeHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.CHECK_CHAR_NAME;

        public CharacterNameChangeHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            string characterName = packet.ReadUnicodeString();
            long itemUid = packet.ReadLong();

            if (DatabaseManager.Characters.NameExists(characterName))
            {
                session.FieldManager.BroadcastPacket(CharacterNameChangePacket.NameResult(true, characterName, 0));
                return;
            }
            session.FieldManager.BroadcastPacket(CharacterNameChangePacket.NameResult(false, characterName, itemUid));
        }
    }
}
