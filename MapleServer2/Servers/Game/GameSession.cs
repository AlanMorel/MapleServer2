using System.Diagnostics;
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
        FieldManager = FieldManagerFactory.GetManager(player);
        FieldPlayer = FieldManager.RequestFieldObject(player);
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
            FieldManager = FieldManagerFactory.GetManager(player);
            FieldPlayer = FieldManager.RequestFieldObject(Player);
        }

        FieldManager.AddPlayer(this, FieldPlayer); // Add player
    }

    public override void EndSession()
    {
        FieldManagerFactory.Release(FieldManager.MapId, FieldManager.InstanceId, Player);

        FieldManager.RemovePlayer(this, FieldPlayer);
        GameServer.PlayerManager.RemovePlayer(FieldPlayer.Value);

        // if session is changing channels, dont send the logout message
        if (Player.IsChangingChannel)
        {
            return;
        }

        GameServer.BuddyManager.SetFriendSessions(Player);

        if (Player.Party != null)
        {
            Player.Party.CheckOffineParty(Player);
        }

        Player.UpdateBuddies();
    }

    public void ReleaseField(Player player)
    {
        FieldManagerFactory.Release(FieldManager.MapId, FieldManager.InstanceId, player);
    }
}
