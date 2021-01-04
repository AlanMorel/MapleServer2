using System.Collections.Generic;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class GuildHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.GUILD;

        public GuildHandler(ILogger<GuildHandler> logger) : base(logger) { }

        private enum GuildMode : byte
        {
            Create = 0x1,
            CheckIn = 0xF,
            EnterHouse = 0x64,
            GuildWindow = 0x54,
            List = 0x55,
            GuildNotice = 0x3E,
            GuildDonate = 0x6E,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            GuildMode mode = (GuildMode) packet.ReadByte();
            switch (mode)
            {
                case GuildMode.Create:
                    HandleCreate(session, packet);
                    break;
                case GuildMode.CheckIn:
                    HandleCheckIn(session, packet);
                    break;
                case GuildMode.EnterHouse:
                    HandleEnterHouse(session, packet);
                    break;
                case GuildMode.GuildWindow:
                    HandleGuildWindowRequest(session, packet);
                    break;
                case GuildMode.List:
                    HandleList(session, packet);
                    break;
                case GuildMode.GuildNotice:
                    HandleGuildNotice(session, packet);
                    break;
                case GuildMode.GuildDonate:
                    HandleGuildDonate(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleCreate(GameSession session, PacketReader packet)
        {
            string guildName = packet.ReadUnicodeString();

            session.Player.Mesos -= 2000;

            session.Send(MesosPacket.UpdateMesos(session));
            session.Send(GuildPacket.Invite(session.Player, guildName));
            session.Send(GuildPacket.Create(guildName));
            session.Send(GuildPacket.UpdateGuild(session, guildName));
            session.Send(GuildPacket.Create2(session.Player, guildName));
            session.Send(GuildPacket.Create3(session.Player, guildName));

            Guild newGuild = new(guildName, new List<Player> { session.Player });
            GameServer.GuildManager.AddGuild(newGuild);

            newGuild.AddMember(session.Player);
        }

        private void HandleCheckIn(GameSession session, PacketReader packet)
        {
            session.Send(GuildPacket.CheckInConfirm());
            // TODO: Send Guild Coins
            // TODO: Can only check in once a day
            session.Send(GuildPacket.UpdateGuildFunds());
            session.Send(GuildPacket.UpdateGuildExp());
            session.Send(GuildPacket.UpdateGuildFunds2());
            session.Send(GuildPacket.UpdatePlayerContribution(session.Player));
            session.Send(GuildPacket.UpdatePlayerContribution(session.Player));
        }

        private void HandleGuildWindowRequest(GameSession session, PacketReader packet)
        {
            session.Send(GuildPacket.GuildWindow());
        }

        private void HandleEnterHouse(GameSession session, PacketReader packet)
        {
            // TODO
        }

        private void HandleList(GameSession session, PacketReader packet)
        {
            session.Send(GuildPacket.List());
        }

        private void HandleGuildNotice(GameSession session, PacketReader packet)
        {
            packet.ReadByte();
            string notice = packet.ReadUnicodeString();

            session.Send(GuildPacket.GuildNoticeConfirm(notice));
            session.Send(GuildPacket.GuildNoticeChange(session.Player, notice)); // TODO: Change to Broadcast to Guild
        }

        private void HandleGuildDonate(GameSession session, PacketReader packet)
        {
            int donateQuantity = packet.ReadInt();

            session.Send(GuildPacket.UpdateGuildFunds());
            session.Send(GuildPacket.UpdatePlayerDonation());
            session.Send(GuildPacket.UpdateGuildFunds2());
            session.Send(GuildPacket.UpdatePlayerContribution(session.Player));
        }
    }
}
