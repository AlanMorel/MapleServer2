using System.Diagnostics;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MigrationPacket
{
    public static PacketWriter LoginToGame(IPEndPoint endpoint, int mapId, AuthData authData)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LoginToGame);
        pWriter.WriteByte(); // 0 = Success
        pWriter.WriteBytes(endpoint.Address.GetAddressBytes());
        pWriter.Write((ushort) endpoint.Port);
        pWriter.WriteInt(authData.TokenA);
        pWriter.WriteInt(authData.TokenB);
        pWriter.WriteInt(mapId);

        return pWriter;
    }

    public static PacketWriter LoginToGameError(string message)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LoginToGame);
        pWriter.WriteByte(1); // !0 = Error
        pWriter.WriteUnicodeString(message);

        return pWriter;
    }

    public static PacketWriter GameToLogin(IPEndPoint endpoint, AuthData authData)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GameToLogin);
        pWriter.WriteByte(); // 0 = Success
        pWriter.WriteBytes(endpoint.Address.GetAddressBytes());
        pWriter.Write((ushort) endpoint.Port);
        pWriter.WriteInt(authData.TokenA);
        pWriter.WriteInt(authData.TokenB);

        return pWriter;
    }

    public static PacketWriter GameToLoginError()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GameToLogin);
        pWriter.WriteByte(1); // !0 = Error

        return pWriter;
    }

    public static PacketWriter GameToGame(IPEndPoint endpoint, Player player)
    {
        Debug.Assert(player.Account.AuthData != null, "player.Account.AuthData != null");

        PacketWriter pWriter = PacketWriter.Of(SendOp.GameToGame);
        pWriter.WriteByte(); // 0 = Success
        pWriter.WriteInt(player.Account.AuthData.TokenA);
        pWriter.WriteInt(player.Account.AuthData.TokenB);
        pWriter.WriteBytes(endpoint.Address.GetAddressBytes());
        pWriter.Write((ushort) endpoint.Port);
        pWriter.WriteInt(player.MapId);
        pWriter.WriteByte(); //unknown

        return pWriter;
    }

    public static PacketWriter GameToGameError()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.GameToGame);
        pWriter.WriteByte(1); // !0 = Error

        return pWriter;
    }
}
