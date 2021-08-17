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
            Activate = 0x06, //could also just be lever object
            AddInteractObject = 0x08,
            AddAdBalloons = 0x09,
            Extra = 0x0D
        }

        private enum InteractStatus : byte
        {
            Disabled = 0x00,
            Enabled = 0x01
        }

        public static Packet ActivateInteractObject(int interactObjectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.WriteEnum(InteractObjectMode.Activate);
            pWriter.WriteInt(interactObjectId);
            pWriter.WriteEnum(InteractStatus.Enabled);
            return pWriter;
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

        public static Packet AddAdBallons(IFieldObject<InteractObject> balloon)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteEnum(InteractObjectMode.AddAdBalloons);
            pWriter.WriteMapleString(balloon.Value.Name);
            pWriter.WriteByte(1);
            pWriter.WriteEnum(balloon.Value.Type);
            pWriter.WriteInt(balloon.Value.Balloon.InteractId);
            pWriter.Write(balloon.Coord);
            pWriter.Write(balloon.Rotation);
            pWriter.WriteUnicodeString(balloon.Value.Balloon.Model);
            pWriter.WriteUnicodeString(balloon.Value.Balloon.Asset);
            pWriter.WriteUnicodeString(balloon.Value.Balloon.NormalState);
            pWriter.WriteUnicodeString(balloon.Value.Balloon.Reactable);
            pWriter.WriteFloat(balloon.Value.Balloon.Scale);
            pWriter.WriteByte();
            pWriter.WriteLong(balloon.Value.Balloon.Owner.CharacterId);
            pWriter.WriteUnicodeString(balloon.Value.Balloon.Owner.Name);
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
