using System.Diagnostics;
using MapleServer2.Enums;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game
{
    public class GameSession : Session
    {
        protected override SessionType Type => SessionType.Game;

        public int ServerTick;
        public int ClientTick;

        public IFieldObject<Player> FieldPlayer { get; private set; }
        public Player Player => FieldPlayer.Value;

        public FieldManager FieldManager { get; private set; }
        private readonly FieldManagerFactory FieldManagerFactory;

        public GameSession(FieldManagerFactory fieldManagerFactory) : base()
        {
            FieldManagerFactory = fieldManagerFactory;
        }

        public void SendNotice(string message)
        {
            Send(ChatPacket.Send(Player, message, ChatType.NoticeAlert));
        }

        // Called first time when starting a new session
        public void InitPlayer(Player player)
        {
            Debug.Assert(FieldPlayer == null, "Not allowed to reinitialize player.");
            FieldManager = FieldManagerFactory.GetManager(player.MapId, instanceId: 0);
            FieldPlayer = FieldManager.RequestFieldObject(player);
            GameServer.Storage.AddPlayer(player);
            GameServer.BuddyManager.SetFriendSessions(player);

            Party party = GameServer.PartyManager.GetPartyByMember(player.CharacterId);
            if (party != null)
            {
                party.BroadcastPacketParty(PartyPacket.LoginNotice(player), this);
            }

            player.BuddyList.ForEach(buddy =>
            {
                if (buddy.Friend?.Session?.Connected() ?? false)
                {
                    Buddy myBuddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddy.SharedId);
                    buddy.Friend.Session.Send(BuddyPacket.LoginLogoutNotification(myBuddy));
                    buddy.Friend.Session.Send(BuddyPacket.UpdateBuddy(myBuddy));
                }
            });
        }

        public void EnterField(Player player)
        {
            // If moving maps, need to get the FieldManager for new map
            if (player.MapId != FieldManager.MapId || player.InstanceId != FieldManager.InstanceId)
            {
                FieldManager.RemovePlayer(this, FieldPlayer); // Leave previous field

                if (FieldManagerFactory.Release(FieldManager.MapId, FieldManager.InstanceId, player))
                {
                    //If instance is destroyed, reset dungeonSession
                    DungeonSession dungeonSession = GameServer.DungeonManager.GetDungeonSessionByInstanceId(FieldManager.InstanceId);
                    //check if the destroyed map was a dungeon map
                    if (dungeonSession != null && FieldManager.InstanceId == dungeonSession.DungeonInstanceId
                        && dungeonSession.IsDungeonSessionMap(FieldManager.MapId))
                    {
                        GameServer.DungeonManager.ResetDungeonSession(player, dungeonSession);
                    }
                }

                // Initialize for new Map
                FieldManager = FieldManagerFactory.GetManager(player.MapId, player.InstanceId);
                FieldPlayer = FieldManager.RequestFieldObject(Player);
            }

            FieldManager.AddPlayer(this, FieldPlayer); // Add player
        }

        public override void EndSession()
        {
            Player.Session = null;
            GameServer.BuddyManager.SetFriendSessions(Player);
            FieldManager.RemovePlayer(this, FieldPlayer);
            GameServer.Storage.RemovePlayer(FieldPlayer.Value);
            // Should we Join the thread to wait for it to complete?

            if (Player.Party != null)
            {
                Player.Party.CheckOffineParty(Player);
            }

            Player.BuddyList.ForEach(buddy =>
            {
                if (buddy.Friend?.Session?.Connected() ?? false)
                {
                    Buddy myBuddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddy.SharedId);
                    buddy.Friend.Session.Send(BuddyPacket.LoginLogoutNotification(myBuddy));
                    buddy.Friend.Session.Send(BuddyPacket.UpdateBuddy(myBuddy));
                }
            });
        }

        public void ReleaseField(Player player)
        {
            FieldManagerFactory.Release(FieldManager.MapId, FieldManager.InstanceId, player);
        }
    }
}
