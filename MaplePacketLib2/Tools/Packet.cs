namespace MaplePacketLib2.Tools
{
    public class Packet
    {
        public byte[] Buffer { get; protected set; }

        public int Length { get; protected set; }

        public Packet(byte[] buffer)
        {
            this.Buffer = buffer;
            this.Length = buffer.Length;
        }

        public PacketReader Reader()
        {
            PacketReader pReader = new PacketReader(Buffer) { Length = Length };
            return pReader;
        }

        public byte[] ToArray()
        {
            byte[] copy = new byte[Length];
            System.Buffer.BlockCopy(Buffer, 0, copy, 0, Length);
            return copy;
        }

        public override string ToString()
        {
            return Buffer.ToHexString(Length, ' ');
        }
    }
}
