namespace MaplePacketLib2.Crypto
{
    public class XORCrypter : ICrypter
    {
        private const int INDEX = 2;

        private readonly byte[] table;

        public XORCrypter(uint version)
        {
            this.table = new byte[2];

            // Init
            var rand1 = new Rand32(version);
            var rand2 = new Rand32(2 * version);

            table[0] = (byte)(rand1.RandomFloat() * 255.0f);
            table[1] = (byte)(rand2.RandomFloat() * 255.0f);
        }

        public static uint GetIndex(uint version)
        {
            return (version + INDEX) % 3 + 1;
        }

        public void Encrypt(byte[] src)
        {
            for (int i = 0; i < src.Length; i++)
            {
                src[i] ^= table[i & 1];
            }
        }

        public void Decrypt(byte[] src)
        {
            for (int i = 0; i < src.Length; i++)
            {
                src[i] ^= table[i & 1];
            }
        }
    }
}
