using System.Diagnostics;
using Maple2Storage.Types;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Managers;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Servers.Game;

public class GameSession : Session
{
    protected override PatchType Type => PatchType.Ignore;

    public int ServerTick;
    public int ClientTick;

    public Player Player;

    public FieldManager FieldManager { get; private set; }
    private readonly FieldManagerFactory FieldManagerFactory;

    public GameSession(FieldManagerFactory fieldManagerFactory)
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
        Debug.Assert(player.FieldPlayer == null, "Not allowed to reinitialize player.");

        Player = player;
        FieldManager = FieldManagerFactory.GetManager(player);
        player.FieldPlayer = FieldManager.RequestCharacter(player);
        player.LastLogTime = TimeInfo.Now();
    }

    public void EnterField(Player player)
    {
        // If moving maps, need to get the FieldManager for new map
        if (player.MapId != FieldManager.MapId || player.InstanceId != FieldManager.InstanceId)
        {
            if (FieldManagerFactory.Release(FieldManager.MapId, FieldManager.InstanceId, player))
            {
                DungeonSession dungeonSession = GameServer.DungeonManager.GetDungeonSessionBySessionId(player.DungeonSessionId);

                //If instance is destroyed, reset dungeonSession
                //further conditions for dungeon completion could be checked here.
                if (dungeonSession != null && dungeonSession.IsDungeonSessionMap(FieldManager.MapId)) //check if the destroyed map was a dungeon map
                {
                    GameServer.DungeonManager.ResetDungeonSession(player, dungeonSession);
                }
            }

            // Initialize for new Map
            FieldManager = FieldManagerFactory.GetManager(player);
            player.FieldPlayer = FieldManager.RequestCharacter(player);
        }

        FieldManager.AddPlayer(this);
    }

    protected override void EndSession(bool logoutNotice)
    {
        if (Player is null || FieldManager is null || FieldManagerFactory is null)
        {
            return;
        }

        FieldManagerFactory.Release(FieldManager.MapId, FieldManager.InstanceId, Player);

        FieldManager.RemovePlayer(this);
        GameServer.PlayerManager.RemovePlayer(Player);

        Player.OnlineCTS.Cancel();
        Player.OnlineTimeThread = null;

        CoordF safeCoord = Player.SafeBlock;
        safeCoord.Z += Block.BLOCK_SIZE;
        Player.SavedCoord = safeCoord;

        // if session is not changing channels or servers, send the logout message
        if (logoutNotice)
        {
            Player.Session = null;
            GameServer.BuddyManager.SetFriendSessions(Player);

            Player.Party?.CheckOfflineParty(Player);

            Player.Guild?.BroadcastPacketGuild(GuildPacket.MemberLoggedOff(Player));

            Player.UpdateBuddies();

            foreach (Club club in Player.Clubs)
            {
                club?.BroadcastPacketClub(ClubPacket.LogoutNotice(Player, club));
            }

            foreach (GroupChat groupChat in Player.GroupChats)
            {
                groupChat?.BroadcastPacketGroupChat(GroupChatPacket.LogoutNotice(groupChat, Player));
                groupChat?.CheckOfflineGroupChat();
            }

            Player.IsMigrating = false;

            if (MapMetadataStorage.MapIsInstancedOnly(Player.MapId) && !MapMetadataStorage.MapIsTutorial(Player.MapId))
            {
                Player.SavedCoord = Player.ReturnCoord;
                Player.MapId = Player.ReturnMapId;
            }

            AuthData authData = Player.Account.AuthData;
            authData.OnlineCharacterId = 0;
            DatabaseManager.AuthData.UpdateOnlineCharacterId(authData);
        }

        List<GameEventUserValue> userTimeValues = Player.EventUserValues.Where(x => x.EventType == GameEventUserValueType.AttendanceAccumulatedTime).ToList();
        foreach (GameEventUserValue userValue in userTimeValues)
        {
            if (!long.TryParse(userValue.EventValue, out long timeAccumulated))
            {
                timeAccumulated = 0;
            }

            timeAccumulated += TimeInfo.Now() - Player.LastLogTime;
            userValue.EventValue = timeAccumulated.ToString();
            DatabaseManager.GameEventUserValue.Update(userValue);
        }

        Player.LastLogTime = TimeInfo.Now();
        Player.Account.LastLogTime = TimeInfo.Now();
        if (Player.GuildMember is not null)
        {
            Player.GuildMember.LastLogTimestamp = TimeInfo.Now();
        }

        DatabaseManager.Characters.Update(Player);
    }
}
