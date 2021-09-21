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

        public static Packet Use(InteractObject interactObject)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.WriteEnum(InteractObjectMode.Use);
            pWriter.WriteMapleString(interactObject.Id);
            pWriter.WriteEnum(interactObject.Type);
            return pWriter;
        }

        public static Packet SetInteractObject(InteractObject interactObject)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.WriteEnum(InteractObjectMode.SetInteractObject);
            pWriter.WriteInt(interactObject.InteractId);
            pWriter.WriteEnum(interactObject.State);
            return pWriter;
        }

        public static Packet LoadInteractObject(List<InteractObject> interactObjects)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.WriteEnum(InteractObjectMode.LoadInteractObject);
            pWriter.WriteInt(interactObjects.Count);
            foreach (InteractObject interactObject in interactObjects)
            {
                WriteInteractObject(pWriter, interactObject);
            }

            return pWriter;
        }

        public static void WriteInteractObject(PacketWriter pWriter, InteractObject interactObject)
        {
            pWriter.WriteMapleString(interactObject.Id);
            pWriter.WriteEnum(interactObject.State);
            pWriter.WriteEnum(interactObject.Type);
            if (interactObject.Type == InteractObjectType.Gathering)
            {
                pWriter.WriteInt();
            }
        }

        public static Packet LoadAdBallon(AdBalloon balloon)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.WriteEnum(InteractObjectMode.AddAdBalloons);
            pWriter.WriteMapleString(balloon.Id);
            pWriter.WriteEnum(balloon.State);
            pWriter.WriteEnum(balloon.Type);
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

        public static Packet Interact(InteractObject interactObject)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);
            pWriter.WriteEnum(InteractObjectMode.Interact);
            pWriter.WriteByte();
            pWriter.WriteMapleString(interactObject.Id);
            pWriter.WriteEnum(interactObject.Type);
            return pWriter;
        }
    }
}
