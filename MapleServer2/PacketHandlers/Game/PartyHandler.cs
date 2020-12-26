using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace MapleServer2.PacketHandlers.Game
{
    public class PartyHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.PARTY;

        public PartyHandler(ILogger<PartyHandler> logger) : base(logger) { }

        private enum PartyMode : byte
        {
            Invite = 0x1,
            Join = 0x2,
            Leave = 0x3,
            Kick = 0x4,
            SetLeader = 0x11,
            FinderJoin = 0x17,
            VoteKick = 0x2D,
            ReadyCheck = 0x2E,
            ReadyCheckUpdate = 0x30
        };

        public override void Handle(GameSession session, PacketReader packet)
        {
            PartyMode mode = (PartyMode)packet.ReadByte(); //Mode

            switch (mode)
            {
                case PartyMode.Invite:
                    HandleInvite(session, packet);
                    break;
                case PartyMode.Join:
                    HandleJoin(session, packet);
                    break;
                case PartyMode.Leave:
                    HandleLeave(session, packet);
                    break;
                case PartyMode.Kick:
                    HandleKick(session, packet);
                    break;
                case PartyMode.SetLeader:
                    HandleSetLeader(session, packet);
                    break;
                case PartyMode.FinderJoin:
                    HandleFinderJoin(session, packet);
                    break;
                case PartyMode.VoteKick:
                    HandleVoteKick(session, packet);
                    break;
                case PartyMode.ReadyCheck:
                    HandleStartReadyCheck(session, packet);
                    break;
                case PartyMode.ReadyCheckUpdate:
                    HandleReadyCheckUpdate(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleInvite(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();

            Player other = GameServer.Storage.GetPlayerByName(target);
            if (other == null)
            {
                return;
            }
            if (other.PartyId != 0)
            {
                session.Send(ChatPacket.Send(session.Player, other.Session.Player.Name + " is already in a party.", ChatType.NoticeAlert2));
                return;
            }

            Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
            if (party == null)
            {
                session.Send(PartyPacket.Create(session.Player));
            }
            else if ((party.Members.Count + 1) >= party.MaxMembers)
            {
                session.Send(ChatPacket.Send(session.Player, "Your party is full!", ChatType.NoticeAlert2));
                return;
            }
            other.Session.Send(PartyPacket.SendInvite(session.Player));
        }

        private void HandleJoin(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString(); //Who invited the player
            bool accept = packet.ReadBool(); //If the player accepted
            int unknown = packet.ReadInt(); //Something something I think it's dungeon not sure
            JoinParty(session, target, accept, unknown);
        }

        private void JoinParty(GameSession session, string leaderName, bool accept, int unknown)
        {
            Player partyLeader = GameServer.Storage.GetPlayerByName(leaderName);
            if (partyLeader == null)
            {
                return;
            }

            GameSession leaderSession = partyLeader.Session;

            if (!accept)
            {
                //Send Decline message to inviting player
                leaderSession.Send(ChatPacket.Send(partyLeader, session.Player.Name + " declined the invitation.", ChatType.NoticeAlert2));
                return;
            }

            Party party = GameServer.PartyManager.GetPartyByLeader(partyLeader);
            if (party != null)
            {
                //Existing party, add joining player to all other party members.
                party.BroadcastPacketParty(PartyPacket.Join(session.Player));
                party.BroadcastPacketParty(PartyPacket.UpdatePlayer(session.Player));
                party.BroadcastPacketParty(PartyPacket.UpdateHitpoints(session.Player));
                party.AddMember(session.Player);

                if (party.PartyFinderId != 0)
                {
                    if (party.Members.Count >= party.MaxMembers)
                    {
                        party.PartyFinderId = 0; //Hide from party finder if full
                        party.BroadcastPacketParty(MatchPartyPacket.RemoveListing(party));
                        party.BroadcastPacketParty(MatchPartyPacket.SendListings(GameServer.PartyManager.GetPartyFinderList(session.Player)));
                        party.BroadcastPacketParty(PartyPacket.MatchParty(null));
                    }
                    else
                    {
                        session.Send(MatchPartyPacket.CreateListing(party)); //Add recruitment listing for newly joining player
                        session.Send(PartyPacket.MatchParty(party));
                    }
                }
            }
            else
            {
                //Create new party
                Party newParty = new Party(10, new List<Player> { partyLeader, session.Player });
                GameServer.PartyManager.AddParty(newParty);

                //Send the party leader all the stuff for the joining player
                leaderSession.Send(PartyPacket.Join(session.Player));
                leaderSession.Send(PartyPacket.UpdatePlayer(session.Player));
                leaderSession.Send(PartyPacket.UpdateHitpoints(session.Player));

                leaderSession.Send(PartyPacket.UpdateHitpoints(partyLeader));

                partyLeader.PartyId = newParty.Id;

                party = newParty;
            }

            session.Player.PartyId = party.Id;

            //Create existing party based on the list of party members
            session.Send(PartyPacket.CreateExisting(partyLeader, party.Members));
            session.Send(PartyPacket.UpdatePlayer(session.Player));
            foreach (Player partyMember in party.Members)
            {
                //Skip first character because of the scuffed Create packet. For now this is a workaround and functions the same.
                if (partyMember.CharacterId != party.Members.First().CharacterId)
                {
                    //Adds the party member to the UI
                    session.Send(PartyPacket.Join(partyMember));
                }
                //Update the HP for each party member.
                session.Send(PartyPacket.UpdateHitpoints(partyMember));
            }
        }

        private void HandleLeave(GameSession session, PacketReader packet)
        {
            Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
            session.Send(PartyPacket.Leave(session.Player, 1)); //1 = You're the player leaving
            if (party == null)
            {
                return;
            }
            party.RemoveMember(session.Player);
        }

        private void HandleSetLeader(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();

            Player newLeader = GameServer.Storage.GetPlayerByName(target);
            if (newLeader == null)
            {
                return;
            }

            Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
            if (party == null)
            {
                return;
            }

            party.BroadcastPacketParty(PartyPacket.SetLeader(newLeader));
            party.Leader = newLeader;
            party.Members.Remove(newLeader);
            party.Members.Insert(0, newLeader);
        }

        private void HandleFinderJoin(GameSession session, PacketReader packet)
        {
            int partyId = packet.ReadInt();
            string leaderName = packet.ReadUnicodeString();
            long partyFinderId = packet.ReadLong();

            Party party = GameServer.PartyManager.GetPartyById(partyId);
            if (party == null || !party.Approval)
            {
                return;
            }
            if (session.Player.PartyId != 0)
            {
                //Disband old party
                Party oldParty = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
                oldParty.PartyFinderId = 0;
                oldParty.CheckDisband();
            }
            //Join party
            JoinParty(session, leaderName, true, 0);
        }

        private void HandleKick(GameSession session, PacketReader packet)
        {
            long charId = packet.ReadLong();

            Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
            if (party == null)
            {
                return;
            }

            Player kickedPlayer = GameServer.Storage.GetPlayerById(charId);
            if (kickedPlayer == null)
            {
                return;
            }

            party.BroadcastPacketParty(PartyPacket.Kick(kickedPlayer));
            party.RemoveMember(kickedPlayer);
        }

        private void HandleVoteKick(GameSession session, PacketReader packet)
        {
            long charId = packet.ReadLong();

            Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
            if (party == null)
            {
                return;
            }

            Player kickedPlayer = GameServer.Storage.GetPlayerById(charId);
            if (kickedPlayer == null)
            {
                return;
            }

            party.BroadcastPacketParty(ChatPacket.Send(session.Player, session.Player.Name + " voted to kick " + kickedPlayer.Name, ChatType.NoticeAlert3));
            //TODO: Keep a counter of vote kicks for a player?
        }

        private void HandleStartReadyCheck(GameSession session, PacketReader packet)
        {
            Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
            if (party == null)
            {
                return;
            }
            party.BroadcastPacketParty(PartyPacket.StartReadyCheck(session.Player, party.Members, party.ReadyChecks++));
            party.RemainingMembers = party.Members.Count - 1;
        }

        private void HandleReadyCheckUpdate(GameSession session, PacketReader packet)
        {
            int checkNum = packet.ReadInt() + 1; //+ 1 is because the ReadyChecks variable is always 1 ahead
            byte accept = packet.ReadByte();

            Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
            if (party == null)
            {
                return;
            }
            if (checkNum != party.ReadyChecks)
            {
                return;
            }
            party.BroadcastPacketParty(PartyPacket.ReadyCheck(session.Player, accept));
            party.RemainingMembers -= 1;
            if (party.RemainingMembers == 0)
            {
                party.BroadcastPacketParty(PartyPacket.EndReadyCheck());
            }
        }
    }
}
