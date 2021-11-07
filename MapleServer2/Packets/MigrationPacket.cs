using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MigrationPacket
{
    public static PacketWriter LoginToGame(IPEndPoint endpoint, AuthData authTokens)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LOGIN_TO_GAME);
        pWriter.WriteByte(); // 0 = Success
        pWriter.WriteBytes(endpoint.Address.GetAddressBytes()); // ip
        pWriter.Write((ushort) endpoint.Port); // port
        pWriter.WriteInt(authTokens.TokenA);
        pWriter.WriteInt(authTokens.TokenB); // Some key
        pWriter.WriteInt(62000000); // Map

        return pWriter;
    }

    public static PacketWriter LoginToGameError(string message)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LOGIN_TO_GAME);
        pWriter.WriteByte(1); // !0 = Error
        pWriter.WriteUnicodeString(message);

        return pWriter;
    }

    public static PacketWriter GameToLogin(IPEndPoint endpoint, AuthData authTokens)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_TO_LOGIN);
        pWriter.WriteByte(); // 0 = Success
        pWriter.WriteBytes(endpoint.Address.GetAddressBytes()); // ip
        pWriter.Write((ushort) endpoint.Port); // port
        pWriter.WriteInt(authTokens.TokenA);
        pWriter.WriteInt(authTokens.TokenB);

        return pWriter;
    }

    public static PacketWriter GameToLoginError()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_TO_LOGIN);
        pWriter.WriteByte(1); // !0 = Error

        return pWriter;
    }

    public static PacketWriter GameToGame(IPEndPoint endpoint, AuthData authTokens, Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_TO_GAME);
        pWriter.WriteByte(); // 0 = Success
        pWriter.WriteInt(authTokens.TokenA);
        pWriter.WriteInt(authTokens.TokenB);
        pWriter.WriteBytes(endpoint.Address.GetAddressBytes());
        pWriter.Write((ushort) endpoint.Port);
        pWriter.WriteInt(player.MapId); // Map
        pWriter.WriteByte(); //unknown

        return pWriter;
    }

    public static PacketWriter GameToGameError()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GAME_TO_GAME);
        pWriter.WriteByte(1); // !0 = Error

        return pWriter;
    }
}
