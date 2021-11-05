namespace MaplePacketLib2.Crypto;

// Left rotates data left by length / 2
public class RearrangeCrypter : ICrypter
{
    private const int INDEX = 1;

    public static uint GetIndex(uint version)
    {
        return (version + INDEX) % 3 + 1;
    }

    public void Encrypt(byte[] src)
    {
        int len = src.Length >> 1;
        for (int i = 0; i < len; i++)
        {
            byte swap = src[i];
            src[i] = src[i + len];
            src[i + len] = swap;
        }
    }

    public void Encrypt(byte[] src, int start, int end)
    {
        int len = end - start >> 1;
        int max = start + len;
        for (int i = start; i < max; i++)
        {
            byte swap = src[i];
            src[i] = src[i + len];
            src[i + len] = swap;
        }
    }

    public void Decrypt(byte[] src)
    {
        int len = src.Length >> 1;
        for (int i = 0; i < len; i++)
        {
            byte swap = src[i];
            src[i] = src[i + len];
            src[i + len] = swap;
        }
    }

    public void Decrypt(byte[] src, int start, int end)
    {
        int len = end - start >> 1;
        int max = start + len;
        for (int i = start; i < max; i++)
        {
            byte swap = src[i];
            src[i] = src[i + len];
            src[i + len] = swap;
        }
    }
}
