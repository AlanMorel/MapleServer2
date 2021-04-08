using System.Collections.Generic;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    class InteractObjectPacket
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

        public static Packet AddInteractActors(ICollection<IFieldObject<InteractObject>> objects)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.AddInteractActor);
            pWriter.WriteInt(objects.Count);
            foreach (IFieldObject<InteractObject> interactObject in objects)
            {
                pWriter.WriteMapleString(interactObject.Value.Uuid);
                pWriter.WriteEnum(InteractStatus.Enabled);
                pWriter.WriteEnum(interactObject.Value.Type);
                if (interactObject.Value.Type == InteractObjectType.Gathering)
                {
                    pWriter.WriteInt();
                }
            }

            return pWriter;
        }

        public static Packet UseObject(MapInteractObject interactObject, short result = 0, int numDrops = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.Use);
            pWriter.WriteShort((short) interactObject.Uuid.Length);
            pWriter.WriteString(interactObject.Uuid);
            pWriter.WriteEnum(interactObject.Type);

            if (interactObject.Type == InteractObjectType.Gathering)
            {
                pWriter.WriteShort(result);
                pWriter.WriteInt(numDrops);
            }

            return pWriter;
        }

        // for binoculars this shows "You get a good look at the area"
        // for extractor it does nothing
        public static Packet Extra(MapInteractObject interactObject)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.Extra);
            pWriter.WriteByte();
            pWriter.WriteShort((short) interactObject.Uuid.Length);
            pWriter.WriteString(interactObject.Uuid);
            pWriter.WriteEnum(interactObject.Type);

            return pWriter;
        }
    }
}
