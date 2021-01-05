namespace MaplePacketLib2.Crypto
{
    public interface ICrypter
    {
        void Encrypt(byte[] src);
        void Decrypt(byte[] src);
    }
}
