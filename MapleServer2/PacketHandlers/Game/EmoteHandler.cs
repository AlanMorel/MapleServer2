using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class EmoteHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.EMOTION;

    private enum EmoteMode : byte
    {
        LearnEmote = 0x1,
        UseEmote = 0x2
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        EmoteMode mode = (EmoteMode) packet.ReadByte();

        switch (mode)
        {
            case EmoteMode.LearnEmote:
                HandleLearnEmote(session, packet);
                break;
            case EmoteMode.UseEmote:
                HandleUseEmote(packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleLearnEmote(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        if (!session.Player.Inventory.HasItem(itemUid))
        {
            return;
        }

        Item item = session.Player.Inventory.GetByUid(itemUid);

        if (session.Player.Emotes.Contains(item.SkillId))
        {
            return;
        }

        session.Player.Emotes.Add(item.SkillId);

        session.Send(EmotePacket.LearnEmote(item.SkillId));

        session.Player.Inventory.ConsumeItem(session, item.Uid, 1);
    }

    private static void HandleUseEmote(PacketReader packet)
    {
        int emoteId = packet.ReadInt();
        string animationName = packet.ReadUnicodeString();
        // animationName is the name in /Xml/anikeytext.xml
    }
}
