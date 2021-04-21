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
            AddInteractObject = 0x08,
            AddAdBalloons = 0x09,
            Extra = 0x0D
        }

        private enum InteractStatus : byte
        {
            Disabled = 0x00,
            Enabled = 0x01
        }

        public static Packet AddInteractObjects(ICollection<IFieldObject<InteractObject>> objects)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.AddInteractObject);
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

        public static Packet AddAdBallons(IFieldObject<InteractAdBalloon> balloon)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.AddAdBalloons);
            pWriter.WriteMapleString(balloon.Value.Name);
            pWriter.WriteByte(1);
            pWriter.WriteEnum(balloon.Value.Type);
            pWriter.WriteInt(balloon.Value.InteractId);
            pWriter.Write(balloon.Coord);
            pWriter.Write(balloon.Rotation);
            pWriter.WriteUnicodeString(balloon.Value.Model);
            pWriter.WriteUnicodeString(balloon.Value.Asset);
            pWriter.WriteUnicodeString(balloon.Value.NormalState);
            pWriter.WriteUnicodeString(balloon.Value.Reactable);
            pWriter.WriteFloat(balloon.Value.Scale);
            pWriter.WriteByte();
            pWriter.WriteLong(balloon.Value.Owner.CharacterId);
            pWriter.WriteUnicodeString(balloon.Value.Owner.Name);
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
