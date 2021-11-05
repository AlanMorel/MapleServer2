using System.Collections.Immutable;
using System.Net;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Login;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Login;

public class ServerEnterPacketHandler : LoginPacketHandler
{
    public override RecvOp OpCode => RecvOp.RESPONSE_SERVER_ENTER;

    // TODO: This data needs to be dynamic
    private readonly ImmutableList<IPEndPoint> ServerIPs;
    private readonly string ServerName;

    public ServerEnterPacketHandler() : base()
    {
        ImmutableList<IPEndPoint>.Builder builder = ImmutableList.CreateBuilder<IPEndPoint>();
        string ipAddress = Environment.GetEnvironmentVariable("IP");
        int port = int.Parse(Environment.GetEnvironmentVariable("LOGIN_PORT"));
        builder.Add(new(IPAddress.Parse(ipAddress), port));

        ServerIPs = builder.ToImmutable();
        ServerName = Environment.GetEnvironmentVariable("NAME");
    }

    public override void Handle(LoginSession session, PacketReader packet)
    {
        List<Banner> banners = DatabaseManager.Banners.FindAllBanners();
        session.Send(BannerListPacket.SetBanner(banners));
        session.Send(ServerListPacket.SetServers(ServerName, ServerIPs));

        List<Player> characters = DatabaseManager.Characters.FindAllByAccountId(session.AccountId);

        Account account = DatabaseManager.Accounts.FindById(session.AccountId);
        session.Send(CharacterListPacket.SetMax(account.CharacterSlots));
        session.Send(CharacterListPacket.StartList());
        // Send each character data
        session.Send(CharacterListPacket.AddEntries(characters));
        session.Send(CharacterListPacket.EndList());
    }
}
