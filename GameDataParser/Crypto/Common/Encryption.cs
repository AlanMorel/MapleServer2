using System;

namespace GameDataParser.Crypto.Common {
    [Flags]
    public enum Encryption : uint {
        // Standard crypto: Base64 Encoded + AES Encrypted buffers.
        Aes = 0xEE000000,
        // Zlib compression.
        // NOTE: The first bit is the level of compression used.
        Zlib = 0x00000009,
        // Alternative crypto: XOR Encrypted buffers.
        Xor = 0xFF000000,
    }
}