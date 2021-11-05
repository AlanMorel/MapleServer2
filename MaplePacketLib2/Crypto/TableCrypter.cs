namespace MaplePacketLib2.Crypto;

public class TableCrypter : ICrypter
{
    private const int INDEX = 3;

    private const int TABLE_SIZE = 256;

    private readonly byte[] Decrypted;
    private readonly byte[] Encrypted;

    public TableCrypter(uint version)
    {
        Decrypted = new byte[TABLE_SIZE];
        Encrypted = new byte[TABLE_SIZE];

        // Init
        for (int i = 0; i < TABLE_SIZE; i++)
        {
            Encrypted[i] = (byte) i;
        }
        Shuffle(Encrypted, version);
        for (int i = 0; i < TABLE_SIZE; i++)
        {
            Decrypted[Encrypted[i]] = (byte) i;
        }
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
            src[i] = Encrypted[src[i]];
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
            src[i] = Decrypted[src[i]];
        }
    }

    private static void Shuffle(byte[] data, uint version)
    {
        Rand32 rand32 = new((uint) Math.Pow(version, 2));
        for (int i = TABLE_SIZE - 1; i >= 1; i--)
        {
            byte rand = (byte) (rand32.Random() % (i + 1));

            byte swap = data[i];
            data[i] = data[rand];
            data[rand] = swap;
        }
    }
}
