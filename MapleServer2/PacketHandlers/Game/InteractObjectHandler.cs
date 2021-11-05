using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

internal class InteractObjectHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.INTERACT_OBJECT;

    public InteractObjectHandler() : base() { }

    private enum InteractObjectMode : byte
    {
        Cast = 0x0B,
        Interact = 0x0C
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        InteractObjectMode mode = (InteractObjectMode) packet.ReadByte();

        switch (mode)
        {
            case InteractObjectMode.Cast:
                HandleCast(session, packet);
                break;
            case InteractObjectMode.Interact:
                HandleInteract(session, packet);
                break;
        }
    }

    private static void HandleCast(GameSession session, PacketReader packet)
    {
        string id = packet.ReadString();
        InteractObject interactObject = session.FieldManager.State.InteractObjects[id];
        if (interactObject == null)
        {
            return;
        }

        // TODO: Change state of object only if player succeeds in the cast.
    }

    private static void HandleInteract(GameSession session, PacketReader packet)
    {
        string id = packet.ReadString();
        InteractObject interactObject = session.FieldManager.State.InteractObjects[id];
        if (interactObject == null)
        {
            return;
        }

        InteractObjectMetadata metadata = InteractObjectMetadataStorage.GetInteractObjectMetadata(interactObject.InteractId);

        switch (interactObject.Type)
        {
            case InteractObjectType.Binoculars:
                session.Send(InteractObjectPacket.Use(interactObject));
                QuestHelper.UpdateExplorationQuest(session, interactObject.InteractId.ToString(), "interact_object_rep");
                break;
            case InteractObjectType.Ui:
                session.Send(InteractObjectPacket.Use(interactObject));
                break;
            case InteractObjectType.RankBoard:
                session.Send(WebOpenPacket.Open(metadata.Web.Url));
                break;
            case InteractObjectType.AdBalloon:
                session.Send(PlayerHostPacket.AdBalloonWindow((AdBalloon) interactObject));
                break;
            case InteractObjectType.Gathering:
                GatheringHelper.HandleGathering(session, metadata.Gathering.RecipeId, out int numDrop);
                session.Send(InteractObjectPacket.Use(interactObject, (short) (numDrop > 0 ? 0 : 1), numDrop));
                break;
        }

        session.Send(InteractObjectPacket.Interact(interactObject));
    }
}
