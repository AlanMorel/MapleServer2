using System.Security.Cryptography;

namespace GameDataParser.Crypto
{
    public class AESCipher
    {
        private readonly byte[] Iv;
        private readonly SymmetricAlgorithm Algorithm;
        private readonly ICryptoTransform Encryptor;

        public int BlockSize => Algorithm.BlockSize / 8;

        /*
         * Constructs a new AES-CTR cipher.
         *
         * @param userKey A 32-byte User Key
         * @param iv A 16-byte IV Chain
         *
        */
        public AESCipher(byte[] userKey, byte[] iv)
        {
            Iv = iv;
            Algorithm = new AesManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.None
            };
            Encryptor = Algorithm.CreateEncryptor(userKey, new byte[BlockSize]);
        }

        /*
         * Transforms a block, encrypting/decrypting the specified data.
         *
         * @param src The raw buffer (of either encrypted or decrypted data)
         * @param offset The initial offset of the source buffer
         * @param size The length of the block (in bytes) to be transformed
         * @param dst The destination buffer (of now-decrypted or now-encrypted data)
         * @param initialOffset The initial offset of the destination buffer
         *
         * @return The length of the block that was transformed
         *
        */
        public uint TransformBlock(byte[] src, int offset, uint size, byte[] dst, int initialOffset)
        {
            for (int i = 0; i < size; i += BlockSize)
            {
                byte[] xorBlock = new byte[BlockSize];
                Encryptor.TransformBlock(Iv, 0, Iv.Length, xorBlock, 0);
                IncrementCounter();

                for (int j = 0; j < xorBlock.Length; j++)
                {
                    if ((i + j) >= dst.Length)
                    {
                        break;
                    }

                    dst[initialOffset + i + j] = (byte) (src[offset + i + j] ^ xorBlock[j]);
                }
            }

            return size;
        }

        // Increments the XOR block counter.
        private void IncrementCounter()
        {
            for (int i = Iv.Length - 1; i >= 0; i--)
            {
                if (++Iv[i] != 0)
                    break;
            }
        }
    }
}
