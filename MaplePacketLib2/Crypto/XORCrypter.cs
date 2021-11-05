namespace MaplePacketLib2.Crypto;

public class XORCrypter : ICrypter
{
    private const int INDEX = 2;

    private readonly byte[] Table;

    public XORCrypter(uint version)
    {
        Table = new byte[2];

        // Init
        Rand32 rand1 = new(version);
        Rand32 rand2 = new(2 * version);

        Table[0] = (byte) (rand1.RandomFloat() * 255.0f);
        Table[1] = (byte) (rand2.RandomFloat() * 255.0f);
    }

    public static uint GetIndex(uint version)
    {
        return (version + INDEX) % 3 + 1;
    }

    public void Encrypt(byte[] src)
    {
        Encrypt(src, 0, src.Length);
    }

    public void Encrypt(byte[] src, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            src[i] ^= Table[i & 1];
        }
    }

    public void Decrypt(byte[] src)
    {
        Decrypt(src, 0, src.Length);
    }

    public void Decrypt(byte[] src, int start, int end)
    {
        for (int i = start; i < end; i++)
        {
            src[i] ^= Table[i & 1];
        }
    }
}
