using System;

namespace MaplePacketLib2.Crypto
{
    public class TableCrypter : ICrypter
    {
        private const int INDEX = 3;

        private const int TABLE_SIZE = 256;

        private readonly byte[] decrypted;
        private readonly byte[] encrypted;

        public TableCrypter(uint version)
        {
            this.decrypted = new byte[TABLE_SIZE];
            this.encrypted = new byte[TABLE_SIZE];

            // Init
            for (int i = 0; i < TABLE_SIZE; i++)
            {
                encrypted[i] = (byte) i;
            }
            Shuffle(encrypted, version);
            for (int i = 0; i < TABLE_SIZE; i++)
            {
                decrypted[encrypted[i]] = (byte) i;
            }
        }

        public static uint GetIndex(uint version)
        {
            return (version + INDEX) % 3 + 1;
        }

        public void Encrypt(byte[] src)
        {
            for (int i = 0; i < src.Length; i++)
            {
                src[i] = encrypted[src[i]];
            }
        }

        public void Decrypt(byte[] src)
        {
            for (int i = 0; i < src.Length; i++)
            {
                src[i] = decrypted[src[i]];
            }
        }

        private static void Shuffle(byte[] data, uint version)
        {
            var rand32 = new Rand32((uint) Math.Pow(version, 2));
            for (int i = TABLE_SIZE - 1; i >= 1; i--)
            {
                byte rand = (byte) (rand32.Random() % (i + 1));

                byte swap = data[i];
                data[i] = data[rand];
                data[rand] = swap;
            }
        }
    }
}
