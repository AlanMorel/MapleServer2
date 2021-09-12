using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class UgcPacket
    {
        public enum UgcMode : byte
        {
            ProfilePicture = 0x0B
        }

        public static Packet SetEndpoint(string unknownEndpoint, string resourceEndpoint, string locale = "na")
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x11); // Function
            pWriter.WriteUnicodeString(unknownEndpoint); // Serves some random irrq.aspx
            pWriter.WriteUnicodeString(resourceEndpoint); // Serves resources
            pWriter.WriteUnicodeString(locale); // locale?
            pWriter.WriteByte(2);

            return pWriter;
        }

        public static Packet Unknown0()
        {
            // SO MANY CASES...
            return null;
        }

        public static Packet Unknown4()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x04);
            pWriter.WriteByte();
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("");
            // ???

            return pWriter;
        }

        public static Packet Unknown7()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x07);
            pWriter.WriteLong();
            pWriter.WriteByte(); // condition
            // If byte == 1
            SharedSubUGC(pWriter);
            // EndIf
            // ???

            return pWriter;
        }

        public static Packet Unknown8()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x08);
            SharedSubUGC2(pWriter);

            return pWriter;
        }

        public static Packet Unknown9()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x09);
            pWriter.WriteLong();
            pWriter.WriteInt(); // counter for loop
            for (int i = 0; i < 0; i++)
            {
                pWriter.WriteLong();
            }

            return pWriter;
        }

        public static Packet SetProfilePictureURL(int objectId, long characterId, string url)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteEnum(UgcMode.ProfilePicture);
            pWriter.WriteInt(objectId);
            pWriter.WriteLong(characterId);
            pWriter.WriteUnicodeString(url);

            return pWriter;
        }

        public static Packet Unknown13To15()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x0D); // Also 0x0E, 0x0F
            pWriter.WriteInt();
            // sub1
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteUnicodeString("StrW");
            pWriter.WriteByte();
            pWriter.WriteLong();
            pWriter.WriteBool(false);
            // sub2
            SharedSub661B00(pWriter);

            return pWriter;
        }

        public static Packet Unknown16()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x10);
            pWriter.WriteInt();
            // sub1
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteUnicodeString("StrW");
            // sub2
            SharedSub661B00(pWriter);

            return pWriter;
        }

        public static Packet Unknown17()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x11);
            pWriter.WriteUnicodeString("WstrA");
            pWriter.WriteUnicodeString("WstrA");
            pWriter.WriteUnicodeString("WstrA");
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet Unknown18()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x12);
            // sub1
            pWriter.WriteInt();
            pWriter.WriteByte(); // condition
            // If condition
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("StrW");
            // unknown call to invalid memory using packet
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("WstrA");
            // unknown call to invalid memory using packet
            pWriter.WriteUnicodeString("StrW");
            // unknown call to invalid memory using packet
            pWriter.WriteUnicodeString("StrW");
            // EndIf

            // One some random condition jump to this block
            pWriter.WriteInt(); // counter for loop
            for (int i = 0; i < 0; i++)
            {
                pWriter.WriteLong();
                pWriter.WriteByte();
                // If some condition (can't read)
                SharedSubUGC(pWriter);
                // EndIf
            }
            pWriter.WriteInt(); // counter for loop
            for (int i = 0; i < 0; i++)
            {
                SharedSubUGC2(pWriter);
            }

            return pWriter;
        }

        public static Packet Unknown20()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x14);
            pWriter.WriteLong();
            pWriter.WriteInt(); // some count for loop
            for (int i = 0; i < 0; i++)
            {
                pWriter.WriteLong();
                pWriter.WriteInt();
                pWriter.WriteLong();
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteLong();
            }

            return pWriter;
        }

        public static Packet Unknown21()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x15);
            pWriter.WriteInt(); // some count for loop
            for (int i = 0; i < 0; i++)
            {
                pWriter.WriteLong();
                pWriter.WriteInt();
            }

            return pWriter;
        }

        public static Packet Unknown22()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.UGC);
            pWriter.WriteByte(0x16);
            pWriter.WriteInt();

            return pWriter;
        }

        private static void SharedSub661B00(PacketWriter pWriter)
        {
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("WstrA");
            pWriter.WriteUnicodeString("StrW");
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("StrW");
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("WstrA");
            pWriter.WriteByte();
        }

        private static void SharedSubUGC(PacketWriter pWriter)
        {
            pWriter.WriteByte();
            pWriter.WriteUnicodeString("WstrA");
            // unknown call to invalid memory using packet
        }

        private static void SharedSubUGC2(PacketWriter pWriter)
        {
            pWriter.WriteLong();
            pWriter.WriteInt(); // counter for loop
            for (int i = 0; i < 0; i++)
            {
                pWriter.WriteLong();
                pWriter.WriteUnicodeString("StrW");
                // unknown call to invalid memory using packet
                pWriter.WriteByte();
            }
        }
    }
}
