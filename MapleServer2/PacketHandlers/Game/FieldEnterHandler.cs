using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class FieldEnterHandler : GamePacketHandler<FieldEnterHandler>
{
    public override RecvOp OpCode => RecvOp.ResponseFieldEnter;

    public override void Handle(GameSession session, PacketReader packet)
    {
        packet.ReadInt(); // ?

        Player player = session.Player;
        Account account = player.Account;

        // session.Send(ReconnectPacket.Send());
        session.EnterField(player);
        session.Send(StatPacket.SetStats(player.FieldPlayer));

        session.Send(TimeScalePacket.SetTimeScale(false, 1, 1, 3, 0)); // ??
        session.Send(UserStatePacket.Send(player.FieldPlayer));

        session.Send(LapenshardPacket.Load(player.Inventory.LapenshardStorage));

        session.Send(EmotePacket.LoadEmotes(player));

        session.Send(MacroPacket.LoadControls(player.Macros));

        // wedding

        session.Send(ResponseCubePacket.DecorationScore(account.Home));
        session.Send(ResponseCubePacket.LoadHome(player.FieldPlayer.ObjectId, player.Account.Home));
        session.Send(ResponseCubePacket.ReturnMap(player.ReturnMapId));

        session.Send(RevivalCountPacket.Send(0));
        session.Send(RevivalConfirmPacket.Send(player.FieldPlayer.ObjectId, 0));

        session.Send(StatPointPacket.WriteTotalStatPoints(player));
        session.Send(StatPointPacket.WriteTotalStatPoints(player)); // This packet is sent twice on GMS, not sure why 
        session.Send(StatPointPacket.WriteStatPointDistribution(player));
        session.Send(SkillPointPacket.ExtraSkillPoints(player));

        if (account.IsVip())
        {
            session.Send(BuffPacket.SendBuff(0,
                new(100000014, player.FieldPlayer.ObjectId, player.FieldPlayer.ObjectId, 1, (int) account.VIPExpiration, 1)));
            session.Send(PremiumClubPacket.ActivatePremium(player.FieldPlayer, account.VIPExpiration));
        }

        session.Send(DungeonListPacket.DungeonList());

        IEnumerable<Cube> cubes = session.FieldManager.State.Cubes.Values
            .Where(x => x.Value.PlotNumber == 1 && x.Value.Item.HousingCategory is ItemHousingCategory.Farming or ItemHousingCategory.Ranching)
            .Select(x => x.Value);
        foreach (Cube cube in cubes)
        {
            session.Send(FunctionCubePacket.UpdateFunctionCube(cube.CoordF.ToByte(), 2, 1));
        }

        if (player.Party is not null)
        {
            session.Send(PartyPacket.UpdatePlayer(player));
        }

        GlobalEvent globalEvent = GameServer.GlobalEventManager.GetCurrentEvent();
        if (globalEvent is not null && !MapMetadataStorage.MapIsInstancedOnly(player.MapId))
        {
            session.Send(GlobalPortalPacket.Notice(globalEvent));
        }

        FieldWar fieldWar = GameServer.FieldWarManager.CurrentFieldWar;
        if (fieldWar is not null && !MapMetadataStorage.MapIsInstancedOnly(player.MapId) && fieldWar.MapId != player.MapId)
        {
            session.Send(FieldWarPacket.LegionPopup(fieldWar.Id, fieldWar.EntryClosureTime.ToUnixTimeSeconds()));
        }


        TrophyManager.OnMapEntered(player, player.MapId);

        QuestManager.OnMapEnter(player, player.MapId);

        player.RecomputeStats();
        player.InitializeEffects();
    }
}
