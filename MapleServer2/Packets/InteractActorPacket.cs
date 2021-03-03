using System.Collections.Generic;
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

        public static Packet AddInteractActors(ICollection<IFieldObject<InteractActor>> actors)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.AddInteractActor);
            pWriter.WriteInt(actors.Count);
            foreach (IFieldObject<InteractActor> actor in actors)
            {
                pWriter.WriteShort((short) actor.Value.Uuid.Length);
                pWriter.WriteString(actor.Value.Uuid);
                pWriter.WriteByte(1);
                pWriter.WriteEnum(actor.Value.Type);
            }

            return pWriter;
        }

        // TODO: if extractor, 0x002B is sent first; figure out why
        public static Packet UseObject(MapInteractActor actor)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.Use);
            pWriter.WriteShort((short) actor.Uuid.Length);
            pWriter.WriteString(actor.Uuid);
            pWriter.WriteEnum(actor.Type);

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