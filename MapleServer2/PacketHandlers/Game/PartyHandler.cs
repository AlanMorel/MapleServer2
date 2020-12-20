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

        public override void Handle(GameSession session, PacketReader packet)
        {
            int mode = (int)packet.ReadByte(); //Mode

            switch (mode)
            {
                //Party invite
                case 1:
                    HandleInvite(session, packet);
                    break;
                //Party join
                case 2:
                    HandleJoin(session, packet);
                    break;
                //Party leave
                case 3:
                    HandleLeave(session, packet);
                    break;
                //Kick player
                case 4:
                    HandleKick(session, packet);
                    break;
                //Set party leader
                case 17:
                    HandleSetLeader(session, packet);
                    break;
                //Vote kicking
                case 45:
                    HandleVoteKick(session, packet);
                    break;
                //Ready check
                case 46:
                    HandleReadyCheck(session, packet);
                    break;

            }
        }
        private void HandleInvite(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();

            Player other = GameServer.Storage.GetPlayerByName(target);
            if (other != null)
            {
                if (other.PartyId == 0)
                {
                    other.Session.Send(PartyPacket.SendInvite(session.Player));
                    if (session.Player.PartyId == 0)
                    {
                        session.Send(PartyPacket.Create(session.Player));
                    }
                    //pSession.Send(ChatPacket.Send(session.Player, "You were invited to a party by " + session.Player.Name, ChatType.NoticeAlert));
                }
                else
                {
                    session.Send(ChatPacket.Send(session.Player, other.Session.Player.Name + " is already in a party.", ChatType.NoticeAlert2));
                }
            }

        }

        private void HandleJoin(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString(); //Who invited the player
            int accept = packet.ReadByte(); //If the player accepted
            int unknown = packet.ReadInt(); //Something something I think it's dungeon not sure

            Player partyLeader = GameServer.Storage.GetPlayerByName(target);
            if (partyLeader != null)
            {
                GameSession leaderSession = partyLeader.Session;
                if (accept == 1)
                {
                    Party party = GameServer.PartyManager.GetPartyByLeader(partyLeader);
                    if (party != null)
                    {
                        //Existing party, add joining player to all other party members.
                        party.BroadcastPacketParty(PartyPacket.Join(session.Player));
                        party.BroadcastPacketParty(PartyPacket.UpdatePlayer(session.Player));
                        party.BroadcastPacketParty(PartyPacket.UpdateHitpoints(session.Player));
                        party.AddPlayer(session.Player);
                    }
                    else
                    {
                        //Create new party
                        Party newParty = new Party(GuidGenerator.Int(), 10, partyLeader, new HashSet<Player> { partyLeader, session.Player });
                        GameServer.PartyManager.AddParty(newParty);

                        //Send the party leader all the stuff for the joining player
                        leaderSession.Send(PartyPacket.Join(session.Player));
                        leaderSession.Send(PartyPacket.UpdatePlayer(session.Player));
                        leaderSession.Send(PartyPacket.UpdateHitpoints(session.Player));

                        leaderSession.Send(PartyPacket.UpdateHitpoints(partyLeader));

                        partyLeader.PartyId = newParty.Uid;

                        party = newParty;
                    }

                    session.Player.PartyId = party.Uid;

                    //Create existing party based on the list of party members
                    session.Send(PartyPacket.CreateExisting(partyLeader, party.Players));
                    session.Send(PartyPacket.UpdatePlayer(session.Player));
                    foreach (Player partyMember in party.Players)
                    {
                        //Don't know why you have to skip the first character in the list
                        if (partyMember.CharacterId != party.Players.First().CharacterId)
                        {
                            //Adds the party member to the UI
                            session.Send(PartyPacket.Join(partyMember));
                        }
                        //Update the HP for each party member.
                        session.Send(PartyPacket.UpdateHitpoints(partyMember));
                    }
                    //Sometimes the party leader doesn't get set correctly. Not sure how to fix.
                }
                else
                {
                    //Send Decline message to inviting player
                    leaderSession.Send(ChatPacket.Send(partyLeader, session.Player.Name + " declined the invitation.", ChatType.NoticeAlert2));
                }
            }
        }

        private void HandleSetLeader(GameSession session, PacketReader packet)
        {
            string target = packet.ReadUnicodeString();
            Player newLeader = GameServer.Storage.GetPlayerByName(target);
            if (newLeader != null)
            {
                Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
                if (party != null)
                {
                    party.BroadcastPacketParty(PartyPacket.SetLeader(newLeader));
                    party.Leader = newLeader;
                }
            }
        }

        private void HandleLeave(GameSession session, PacketReader packet)
        {
            Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
            session.Send(PartyPacket.Leave(session.Player, 1)); //1 = You're the player leaving
            session.Player.PartyId = 0;
            party.RemovePlayer(session.Player);
            party.BroadcastPacketParty(PartyPacket.Leave(session.Player, 0));
            if (party.Leader.CharacterId == session.Player.CharacterId)
            {
                //Set new leader
                Player newLeader = party.Players.First();
                party.BroadcastPacketParty(PartyPacket.SetLeader(party.Players.First()));
                party.Leader = newLeader;
            }
            if (party.Players.Count < 2)
            {
                party.BroadcastParty(session =>
                {
                    session.Player.PartyId = 0;
                    session.Send(PartyPacket.Disband());
                });
                GameServer.PartyManager.RemoveParty(party);
            }
        }

        private void HandleKick(GameSession session, PacketReader packet)
        {
            long char_id = packet.ReadLong();
            Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
            if (party != null)
            {
                Player kickedPlayer = GameServer.Storage.GetPlayerById(char_id);
                if (kickedPlayer != null)
                {
                    party.BroadcastPacketParty(PartyPacket.Kick(kickedPlayer));
                    party.RemovePlayer(kickedPlayer);
                    kickedPlayer.PartyId = 0;
                    if (party.Leader.CharacterId == kickedPlayer.CharacterId)
                    {
                        //Set new leader
                        Player newLeader = party.Players.First();
                        party.BroadcastPacketParty(PartyPacket.SetLeader(newLeader));
                        party.Leader = newLeader;
                    }
                    if (party.Players.Count < 2)
                    {
                        party.BroadcastParty(session =>
                        {
                            session.Player.PartyId = 0;
                            session.Send(PartyPacket.Disband());
                        });
                        GameServer.PartyManager.RemoveParty(party);

                    }
                }
            }
        }

        private void HandleVoteKick(GameSession session, PacketReader packet)
        {
            long char_id = packet.ReadLong();
            Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
            if (party != null)
            {
                Player kickedPlayer = GameServer.Storage.GetPlayerById(char_id);
                if (kickedPlayer != null)
                {
                    party.BroadcastPacketParty(ChatPacket.Send(session.Player, session.Player.Name + " voted to kick " + kickedPlayer.Name, ChatType.NoticeAlert3));
                    //TODO: Keep a counter of vote kicks for a player?
                }
            }
        }

        private void HandleReadyCheck(GameSession session, PacketReader packet)
        {
            Party party = GameServer.PartyManager.GetPartyByLeader(session.Player);
            if (party != null)
            {
                party.BroadcastPacketParty(PartyPacket.ReadyCheck(session.Player));
            }
        }
    }
}
