using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    internal class InteractObjectPacket
    {
        private enum InteractObjectMode : byte
        {
            Use = 0x05,
            SetInteractObject = 0x06,
            LoadInteractObject = 0x08,
            AddAdBalloons = 0x09,
            Interact = 0x0D
        }

        public static PacketWriter Use(InteractObject interactObject, short result = 0, int numDrops = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.Write(InteractObjectMode.Use);
            pWriter.WriteString(interactObject.Id);
            pWriter.Write(interactObject.Type);

            if (interactObject.Type == InteractObjectType.Gathering)
            {
                pWriter.WriteShort(result);
                pWriter.WriteInt(numDrops);
            }
            return pWriter;
        }

        public static PacketWriter SetInteractObject(InteractObject interactObject)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.Write(InteractObjectMode.SetInteractObject);
            pWriter.WriteInt(interactObject.InteractId);
            pWriter.Write(interactObject.State);
            return pWriter;
        }

        public static PacketWriter LoadInteractObject(List<InteractObject> interactObjects)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.Write(InteractObjectMode.LoadInteractObject);
            pWriter.WriteInt(interactObjects.Count);
            foreach (InteractObject interactObject in interactObjects)
            {
                WriteInteractObject(pWriter, interactObject);
            }

            return pWriter;
        }

        public static void WriteInteractObject(PacketWriter pWriter, InteractObject interactObject)
        {
            pWriter.WriteString(interactObject.Id);
            pWriter.Write(interactObject.State);
            pWriter.Write(interactObject.Type);
            if (interactObject.Type == InteractObjectType.Gathering)
            {
                pWriter.WriteInt();
            }
        }

        public static PacketWriter LoadAdBallon(AdBalloon balloon)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.Write(InteractObjectMode.AddAdBalloons);
            pWriter.WriteString(balloon.Id);
            pWriter.Write(balloon.State);
            pWriter.Write(balloon.Type);
            pWriter.WriteInt(balloon.InteractId);
            pWriter.Write(balloon.Position);
            pWriter.Write(balloon.Rotation);
            pWriter.WriteUnicodeString(balloon.Model);
            pWriter.WriteUnicodeString(balloon.Asset);
            pWriter.WriteUnicodeString(balloon.NormalState);
            pWriter.WriteUnicodeString(balloon.Reactable);
            pWriter.WriteFloat(balloon.Scale);
            pWriter.WriteByte();
            pWriter.WriteLong(balloon.Owner.CharacterId);
            pWriter.WriteUnicodeString(balloon.Owner.Name);
            return pWriter;
        }

        public static PacketWriter Interact(InteractObject interactObject)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.Write(InteractObjectMode.Interact);
            pWriter.WriteByte();
            pWriter.WriteString(interactObject.Id);
            pWriter.Write(interactObject.Type);
            return pWriter;
        }
    }
}
