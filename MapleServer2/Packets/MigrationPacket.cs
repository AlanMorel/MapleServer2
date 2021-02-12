using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;

namespace MapleServer2.Packets
{
    public static class MigrationPacket
    {
        public static Packet LoginToGame(IPEndPoint endpoint, AuthData authTokens)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.LOGIN_TO_GAME);
            pWriter.WriteByte(); // 0 = Success
            pWriter.Write(endpoint.Address.GetAddressBytes()); // ip
            pWriter.WriteUShort((ushort) endpoint.Port); // port
            pWriter.WriteInt(authTokens.TokenA);
            pWriter.WriteInt(authTokens.TokenB); // Some key
            pWriter.WriteInt(62000000); // Map

            return pWriter;
        }

        public static Packet LoginToGameError(string message)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.LOGIN_TO_GAME);
            pWriter.WriteByte(1); // !0 = Error
            pWriter.WriteUnicodeString(message);

            return pWriter;
        }

        public static Packet GameToLogin(IPEndPoint endpoint, AuthData authTokens)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_TO_LOGIN);
            pWriter.WriteByte(); // 0 = Success
            pWriter.Write(endpoint.Address.GetAddressBytes()); // ip
            pWriter.WriteUShort((ushort) endpoint.Port); // port
            pWriter.WriteInt(authTokens.TokenA);
            pWriter.WriteInt(authTokens.TokenB);

            return pWriter;
        }

        public static Packet GameToLoginError()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_TO_LOGIN);
            pWriter.WriteByte(1); // !0 = Error

            return pWriter;
        }

        public static Packet GameToGame(IPEndPoint endpoint, AuthData authTokens)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_TO_GAME);
            pWriter.WriteByte(); // 0 = Success
            pWriter.WriteInt(authTokens.TokenA);
            pWriter.WriteInt(authTokens.TokenB);
            pWriter.Write(endpoint.Address.GetAddressBytes());
            pWriter.WriteUShort((ushort) endpoint.Port);
            pWriter.WriteInt(); // Map
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet GameToGameError()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_TO_GAME);
            pWriter.WriteByte(1); // !0 = Error

            return pWriter;
        }
    }
}
