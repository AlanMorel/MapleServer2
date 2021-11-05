namespace MaplePacketLib2.Crypto;

public class Rand32
{
    private uint S1;
    private uint S2;
    private uint S3;

    public Rand32(uint seed)
    {
        uint rand = CrtRand(seed);

        S1 = seed | 0x100000;
        S2 = rand | 0x1000;
        S3 = CrtRand(rand) | 0x10;
    }

    public static uint CrtRand(uint seed)
    {
        return 214013 * seed + 2531011;
    }

    public uint Random()
    {
        S1 = S1 << 12 & 0xFFFFE000 ^ S1 >> 6 & 0x00001FFF ^ S1 >> 19;
        S2 = S2 << 4 & 0xFFFFFF80 ^ S2 >> 23 & 0x0000007F ^ S2 >> 25;
        S3 = S3 << 17 & 0xFFE00000 ^ S3 >> 8 & 0x001FFFFF ^ S3 >> 11;

        return S1 ^ S2 ^ S3;
    }

    public float RandomFloat()
    {
        uint bits = Random() & 0x007FFFFF | 0x3F800000;

        return BitConverter.ToSingle(BitConverter.GetBytes(bits), 0) - 1.0f;
    }
}
