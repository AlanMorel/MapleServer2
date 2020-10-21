using System;
using GameDataParser.Crypto.Common;

namespace GameDataParser.Crypto.Keys {
    public static class CipherKeys {
        private const int BITS = 128; //128-bit AES
        private const int IV_LEN = 16; //16-byte IV (CTR)
        private const int KEY_LEN = 32; //32-byte UserKey
        private const int XOR_LEN = 2048; //2048-byte XOR Block

        private static readonly IMultiArray MS2F_KEY =
            new MultiArrayResource(Resources.ResourceManager, "MS2F_Key", BITS, KEY_LEN);
        private static readonly IMultiArray MS2F_IV =
            new MultiArrayResource(Resources.ResourceManager, "MS2F_IV", BITS, IV_LEN);
        private static readonly Lazy<byte[]> MS2F_XOR =
            new Lazy<byte[]>(() => (byte[]) Resources.ResourceManager.GetObject("MS2F_XOR"));

        private static readonly IMultiArray NS2F_KEY =
            new MultiArrayResource(Resources.ResourceManager, "NS2F_Key", BITS, KEY_LEN);
        private static readonly IMultiArray NS2F_IV =
            new MultiArrayResource(Resources.ResourceManager, "NS2F_IV", BITS, IV_LEN);
        private static readonly Lazy<byte[]> NS2F_XOR =
            new Lazy<byte[]>(() => (byte[]) Resources.ResourceManager.GetObject("NS2F_XOR"));

        private static readonly IMultiArray OS2F_KEY =
            new MultiArrayResource(Resources.ResourceManager, "OS2F_Key", BITS, KEY_LEN);
        private static readonly IMultiArray OS2F_IV =
            new MultiArrayResource(Resources.ResourceManager, "OS2F_IV", BITS, IV_LEN);
        private static readonly Lazy<byte[]> OS2F_XOR =
            new Lazy<byte[]>(() => (byte[]) Resources.ResourceManager.GetObject("OS2F_XOR"));

        private static readonly IMultiArray PS2F_KEY =
            new MultiArrayResource(Resources.ResourceManager, "PS2F_Key", BITS, KEY_LEN);
        private static readonly IMultiArray PS2F_IV =
            new MultiArrayResource(Resources.ResourceManager, "PS2F_IV", BITS, IV_LEN);
        private static readonly Lazy<byte[]> PS2F_XOR =
            new Lazy<byte[]>(() => (byte[]) Resources.ResourceManager.GetObject("PS2F_XOR"));

        /*
         * Outputs the Key and IV blocks for the specified version.
         *
         * @param version The pack file version
         * @param uKeyOffset The key index to output (this is compressedSize)
         * @param userKey The outputted Key block
         * @param ivChain The outputted IV (CTR) block
         *
        */
        public static void GetKeyAndIV(PackVersion version, uint keyOffset, out byte[] userKey, out byte[] ivChain) {
            IMultiArray key;
            IMultiArray iv;

            switch (version) {
                case PackVersion.MS2F:
                    key = MS2F_KEY;
                    iv = MS2F_IV;
                    break;
                case PackVersion.NS2F:
                    key = NS2F_KEY;
                    iv = NS2F_IV;
                    break;
                case PackVersion.OS2F:
                    key = OS2F_KEY;
                    iv = OS2F_IV;
                    break;
                case PackVersion.PS2F:
                    key = PS2F_KEY;
                    iv = PS2F_IV;
                    break;
                default: {
                    throw new Exception("ERROR generating Key/IV: the specified package version does not exist!");
                }
            }

            userKey = new byte[KEY_LEN];
            ivChain = new byte[IV_LEN];
            for (int i = 0; i < KEY_LEN; i++) {
                userKey[i] = key[(keyOffset & 0x7F)][i];

                if (i < IV_LEN) {
                    ivChain[i] = iv[(keyOffset & 0x7F)][i];
                }
            }
        }

        /*
         * Outputs the specific XOR key for the given version.
         *
         * @param version The pack file version
         * @param key The outputted key block
         *
        */
        public static void GetXorKey(PackVersion version, out byte[] key) {
            switch (version) {
                case PackVersion.MS2F:
                    key = MS2F_XOR.Value;
                    break;
                case PackVersion.NS2F:
                    key = NS2F_XOR.Value;
                    break;
                case PackVersion.OS2F:
                    key = OS2F_XOR.Value;
                    break;
                case PackVersion.PS2F:
                    key = PS2F_XOR.Value;
                    break;
                default: {
                    // Nexon always defaults to MS2F here.
                    key = MS2F_XOR.Value;
                    break;
                }
            }
        }
    }
}