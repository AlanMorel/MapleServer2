using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    class InteractActorPacket
    {
        public static Packet AddInteractActors(ICollection<IFieldObject<InteractActor>> actors)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.INTERACT_OBJECT);

            pWriter.WriteByte(0x08);  // There are different types here.
            pWriter.WriteInt(actors.Count);
            foreach (IFieldObject<InteractActor> actor in actors)
            {
                pWriter.WriteShort((short) actor.Value.Uuid.Length);
                pWriter.WriteString(actor.Value.Uuid);
                pWriter.WriteByte(1);
                pWriter.WriteByte(1);
            }

            return pWriter;
        }
    }
}
