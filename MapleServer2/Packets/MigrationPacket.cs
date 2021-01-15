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
            return PacketWriter.Of(SendOp.LOGIN_TO_GAME)
                .WriteByte() // 0 = Success
                .Write(endpoint.Address.GetAddressBytes()) // ip
                .WriteUShort((ushort) endpoint.Port) // port
                .WriteInt(authTokens.TokenA)
                .WriteInt(authTokens.TokenB) // Some key
                .WriteInt(62000000); // Map
        }

        public static Packet LoginToGameError(string message)
        {
            return PacketWriter.Of(SendOp.LOGIN_TO_GAME)
                .WriteByte(1) // !0 = Error
                .WriteUnicodeString(message);
        }

        public static Packet GameToLogin(IPEndPoint endpoint, AuthData authTokens)
        {
            return PacketWriter.Of(SendOp.GAME_TO_LOGIN)
                .WriteByte() // 0 = Success
                .Write(endpoint.Address.GetAddressBytes()) // ip
                .WriteUShort((ushort) endpoint.Port) // port
                .WriteInt(authTokens.TokenA)
                .WriteInt(authTokens.TokenB);
        }

        public static Packet GameToLoginError()
        {
            return PacketWriter.Of(SendOp.GAME_TO_LOGIN)
                .WriteByte(1); // !0 = Error
        }

        public static Packet GameToGame(IPEndPoint endpoint, AuthData authTokens)
        {
            return PacketWriter.Of(SendOp.GAME_TO_GAME)
                .WriteByte() // 0 = Success
                .WriteInt(authTokens.TokenA)
                .WriteInt(authTokens.TokenB)
                .Write(endpoint.Address.GetAddressBytes())
                .WriteUShort((ushort) endpoint.Port)
                .WriteInt() // Map
                .WriteByte();
        }

        public static Packet GameToGameError()
        {
            return PacketWriter.Of(SendOp.GAME_TO_GAME)
                .WriteByte(1); // !0 = Error
        }
    }
}
