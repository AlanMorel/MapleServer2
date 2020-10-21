using System;

namespace MaplePacketLib2.Crypto {
    public class Rand32 {
        private uint s1;
        private uint s2;
        private uint s3;

        public Rand32(uint seed) {
            uint rand = CrtRand(seed);

            this.s1 = seed | 0x100000;
            this.s2 = rand | 0x1000;
            this.s3 = CrtRand(rand) | 0x10;
        }

        public static uint CrtRand(uint seed) {
            return 214013 * seed + 2531011;
        }

        public uint Random() {
            s1 = ((s1 << 12) & 0xFFFFE000) ^ ((s1 >> 6) & 0x00001FFF) ^ (s1 >> 19);
            s2 = ((s2 << 4) & 0xFFFFFF80) ^ ((s2 >> 23) & 0x0000007F) ^ (s2 >> 25);
            s3 = ((s3 << 17) & 0xFFE00000) ^ ((s3 >> 8) & 0x001FFFFF) ^ (s3 >> 11);

            return s1 ^ s2 ^ s3;
        }

        public float RandomFloat() {
            uint bits = (Random() & 0x007FFFFF) | 0x3F800000;

            return BitConverter.ToSingle(BitConverter.GetBytes(bits), 0) - 1.0f;
        }
    }
}
