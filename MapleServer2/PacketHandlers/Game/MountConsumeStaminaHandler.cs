using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

internal class MountConsumeStaminaHandler : GamePacketHandler<MountConsumeStaminaHandler>
{
    public override RecvOp OpCode => RecvOp.MountConsumeStamina;

    public override void Handle(GameSession session, PacketReader packet)
    {
        MountMetadata metadata = MountMetadataStorage.GetMountMetadata(session.Player.Mount.Value.Id);
        if (metadata is null)
        {
            return;
        }

        session.Player.FieldPlayer.ConsumeStamina(metadata.RunConsumeEp, noRegen: true);
        session.Send(StatPacket.UpdateStats(session.Player.FieldPlayer, StatAttribute.Stamina));
    }
}
