using System.Collections.Generic;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    class InteractActorPacket
    {
        private enum InteractObjectMode : byte
        {
            Use = 0x05,
            AddInteractActor = 0x08,
            Extra = 0x0D
        }

        private enum InteractStatus : byte
        {
            Disabled = 0x00,
            Enabled = 0x01
        }

        public static Packet AddInteractActors(ICollection<IFieldObject<InteractActor>> actors)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.AddInteractActor);
            pWriter.WriteInt(actors.Count);
            foreach (IFieldObject<InteractActor> actor in actors)
            {
                pWriter.WriteShort((short) actor.Value.Uuid.Length);
                pWriter.WriteString(actor.Value.Uuid);
                pWriter.WriteEnum(InteractStatus.Enabled);
                pWriter.WriteEnum(actor.Value.Type);
                pWriter.WriteInt(0);
            }

            return pWriter;
        }

        public static Packet UseObject(MapInteractActor actor, short result = 0, int numDrops = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.Use);
            pWriter.WriteShort((short) actor.Uuid.Length);
            pWriter.WriteString(actor.Uuid);
            pWriter.WriteEnum(actor.Type);

            if (actor.Type == InteractActorType.Gathering)
            {
                pWriter.WriteShort(result);
                pWriter.WriteInt(numDrops);
            }

            return pWriter;
        }

        // for binoculars this shows "You get a good look at the area"
        // for extractor it does nothing
        public static Packet Extra(MapInteractActor actor)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.Extra);
            pWriter.WriteByte();
            pWriter.WriteShort((short) actor.Uuid.Length);
            pWriter.WriteString(actor.Uuid);
            pWriter.WriteEnum(actor.Type);

            return pWriter;
        }
    }
}
