using System;
using System.IO.MemoryMappedFiles;
using System.Text;
using GameDataParser.Crypto.Common;
using GameDataParser.Crypto.Keys;
using GameDataParser.Crypto.Stream;
using Ionic.Zlib;

namespace GameDataParser.Crypto
{
    public static class CryptoManager
    {
        public static byte[] DecryptFileString(IPackStream stream, System.IO.Stream buffer)
        {
            if (stream.CompressedHeaderSize > 0 && stream.EncodedHeaderSize > 0 &&
                stream.HeaderSize > 0)
            {
                byte[] src = new byte[stream.EncodedHeaderSize];

                if ((ulong) buffer.Read(src, 0, (int) stream.EncodedHeaderSize) ==
                    stream.EncodedHeaderSize)
                {
                    return Decrypt(stream.Version, (uint) stream.EncodedHeaderSize,
                        (uint) stream.CompressedHeaderSize, Encryption.Aes | Encryption.Zlib, src);
                }
            }

            throw new Exception("ERROR decrypting file list: the size of the list is invalid.");
        }

        public static byte[] DecryptFileTable(IPackStream stream, System.IO.Stream buffer)
        {
            if (stream.CompressedDataSize > 0 && stream.EncodedDataSize > 0 && stream.DataSize > 0)
            {
                byte[] src = new byte[stream.EncodedDataSize];

                if ((ulong) buffer.Read(src, 0, (int) stream.EncodedDataSize) == stream.EncodedDataSize)
                {
                    return Decrypt(stream.Version, (uint) stream.EncodedDataSize,
                        (uint) stream.CompressedDataSize, Encryption.Aes | Encryption.Zlib, src);
                }
            }

            throw new Exception("ERROR decrypting file table: the size of the table is invalid.");
        }

        public static byte[] DecryptData(IPackFileHeader pHeader, MemoryMappedFile data)
        {
            if (pHeader.CompressedFileSize > 0 && pHeader.EncodedFileSize > 0 && pHeader.FileSize > 0)
            {
                using MemoryMappedViewStream buffer =
                    data.CreateViewStream((long) pHeader.Offset, pHeader.EncodedFileSize);
                byte[] src = new byte[pHeader.EncodedFileSize];

                if (buffer.Read(src, 0, (int) pHeader.EncodedFileSize) == pHeader.EncodedFileSize)
                {
                    return Decrypt(pHeader.Version, pHeader.EncodedFileSize,
                        (uint) pHeader.CompressedFileSize, pHeader.BufferFlag, src);
                }
            }

            throw new Exception("ERROR decrypting data file segment: the size of the block is invalid.");
        }

        // Decryption Routine: Base64 -> AES -> Zlib
        private static byte[] Decrypt(PackVersion version, uint size, uint sizeCompressed, Encryption flag, byte[] src)
        {
            if (flag.HasFlag(Encryption.Aes))
            {
                // Get the AES Key/IV for transformation
                CipherKeys.GetKeyAndIV(version, sizeCompressed, out byte[] key, out byte[] iv);

                // Decode the base64 encoded string
                src = Convert.FromBase64String(Encoding.UTF8.GetString(src));

                // Decrypt the AES encrypted block
                var pCipher = new AESCipher(key, iv);
                pCipher.TransformBlock(src, 0, size, src, 0);
            }
            else if (flag.HasFlag(Encryption.Xor))
            {
                // Decrypt the XOR encrypted block
                src = EncryptXor(version, src, size, sizeCompressed);
            }

            return flag.HasFlag(Encryption.Zlib) ? ZlibStream.UncompressBuffer(src) : src;
        }

        // Encryption Routine: Zlib -> AES -> Base64
        public static byte[] Encrypt(PackVersion version, byte[] src, Encryption flag, out uint size, out uint sizeCompressed, out uint sizeEncoded)
        {
            byte[] pEncrypted;
            if (flag.HasFlag(Encryption.Zlib))
            {
                pEncrypted = ZlibStream.CompressBuffer(src);
            }
            else
            {
                pEncrypted = new byte[src.Length];
                Buffer.BlockCopy(src, 0, pEncrypted, 0, src.Length);
            }

            size = (uint) src.Length;
            sizeCompressed = (uint) pEncrypted.Length;

            if (flag.HasFlag(Encryption.Aes))
            {
                // Get the AES Key/IV for transformation
                CipherKeys.GetKeyAndIV(version, sizeCompressed, out byte[] key, out byte[] iv);

                // Perform AES block encryption
                var pCipher = new AESCipher(key, iv);
                pCipher.TransformBlock(pEncrypted, 0, size, pEncrypted, 0);

                // Encode the encrypted data into a base64 encoded string
                pEncrypted = Encoding.UTF8.GetBytes(Convert.ToBase64String(pEncrypted));
            }
            else if (flag.HasFlag(Encryption.Xor))
            {
                // Perform XOR block encryption
                pEncrypted = EncryptXor(version, pEncrypted, size, sizeCompressed);
            }

            sizeEncoded = (uint) pEncrypted.Length;

            return pEncrypted;
        }

        private static byte[] EncryptXor(PackVersion version, byte[] src, uint size, uint sizeCompressed)
        {
            CipherKeys.GetXorKey(version, out byte[] key);

            uint uBlock = size >> 2;
            uint uBlockOffset = 0;
            int nKeyOffset = 0;

            if (uBlock != 0)
            {
                while (uBlockOffset < uBlock)
                {
                    uint pBlockData = BitConverter.ToUInt32(src, (int) (4 * uBlockOffset)) ^
                                      BitConverter.ToUInt32(key, 4 * nKeyOffset);
                    Buffer.BlockCopy(BitConverter.GetBytes(pBlockData), 0, src, (int) (4 * uBlockOffset),
                        sizeof(uint));

                    nKeyOffset = ((ushort) nKeyOffset + 1) & 0x1FF;
                    uBlockOffset++;
                }
            }

            uBlock = (size & 3);
            if (uBlock != 0)
            {
                int nStart = (int) (4 * uBlockOffset);

                uBlockOffset = 0;
                nKeyOffset = 0;

                while (uBlockOffset < uBlock)
                {
                    src[nStart + uBlockOffset++] ^= (byte) (key[nKeyOffset]);

                    nKeyOffset = ((ushort) nKeyOffset + 1) & 0x7FF;
                }
            }

            return src;
        }
    }
}
