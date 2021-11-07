namespace MaplePacketLib2.Crypto;

public interface ICrypter
{
    void Encrypt(byte[] src);
    void Encrypt(byte[] src, int start, int end);
    void Decrypt(byte[] src);
    void Decrypt(byte[] src, int start, int end);
}
