using System.Security.Cryptography;

namespace GameDataParser.Crypto
{
    public class AESCipher
    {
        private readonly byte[] iv;
        private readonly SymmetricAlgorithm algorithm;
        private readonly ICryptoTransform encryptor;

        public int BlockSize => algorithm.BlockSize / 8;

        /*
         * Constructs a new AES-CTR cipher.
         *
         * @param userKey A 32-byte User Key
         * @param iv A 16-byte IV Chain
         *
        */
        public AESCipher(byte[] userKey, byte[] iv)
        {
            this.iv = iv;
            this.algorithm = new AesManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.None
            };
            this.encryptor = algorithm.CreateEncryptor(userKey, new byte[BlockSize]);
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
                encryptor.TransformBlock(iv, 0, iv.Length, xorBlock, 0);
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
            for (int i = iv.Length - 1; i >= 0; i--)
            {
                if (++iv[i] != 0)
                    break;
            }
        }
    }
}
